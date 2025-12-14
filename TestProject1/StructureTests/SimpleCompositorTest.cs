using Moq;
using NSem3PT34.Classes;
using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TestProject1.StructureTests
{
    public class SimpleCompositorTest
    {
        [Fact]
        public void Compose_NullOrEmptyList_ReturnsEmpty()
        {
            var comp = new SimpleCompositor();
            var dg = new DrawingGroup();
            using (var dc = dg.Open())
            {
                var args = new ViewEventArgs(dc, 0, 0, 100, 100);

                List<Row> resultNull = comp.Compose(null, args);
                List<Row> resultEmpty = comp.Compose(new List<Glyph>(), args);

                Assert.NotNull(resultNull);
                Assert.Empty(resultNull);

                Assert.NotNull(resultEmpty);
                Assert.Empty(resultEmpty);
            }
        }

        [Fact]
        public void Compose_AllGlyphs_NoBreaks_ProducesSingleRow_WithCorrectUiGlyphsAndPositions()
        {
            var comp = new SimpleCompositor();
            var dg = new DrawingGroup();
            using (var dc = dg.Open())
            {
                var g0 = new Mock<Glyph>();
                var g1 = new Mock<Glyph>();
                var g2 = new Mock<Glyph>();

                g0.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 5.0, 6.0));
                g1.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 7.0, 6.0));
                g2.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 4.0, 6.0));

                g0.Setup(g => g.DoesBreakLine(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(false);
                g1.Setup(g => g.DoesBreakLine(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(false);
                g2.Setup(g => g.DoesBreakLine(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(false);

                var glyphs = new List<Glyph> { g0.Object, g1.Object, g2.Object };

                double startTop = 10.0;
                double startLeft = 4.0;
                double frameWidth = 200.0;
                var args = new ViewEventArgs(dc, startTop, startLeft, frameWidth, 1000);

                var rows = comp.Compose(glyphs, args);

                Assert.Single(rows);
                var row = rows[0];
                Assert.Equal(0, row.GetStartIndex());
                
                Assert.Equal(3, row.GetUiGlyphs().Count);

                var ui0 = row.GetUiGlyphs()[0];
                var ui1 = row.GetUiGlyphs()[1];
                var ui2 = row.GetUiGlyphs()[2];

                Assert.Equal(startLeft, ui0.GetPosition().X);
                Assert.Equal(startTop, ui0.GetPosition().Y);

                double expectedX1 = startLeft + g0.Object.Bounds().Width + 2;
                double expectedX2 = expectedX1 + g1.Object.Bounds().Width + 2;

                Assert.Equal(expectedX1, ui1.GetPosition().X);
                Assert.Equal(expectedX2, ui2.GetPosition().X);

                Assert.Equal(0, ui0.GetPhysicalIndex());
                Assert.Equal(1, ui1.GetPhysicalIndex());
                Assert.Equal(2, ui2.GetPhysicalIndex());
            }
        }

        [Fact]
        public void Compose_BreakInMiddle_CreatesTwoRows_SetsEndAndStartIndicesAndTop()
        {
            var comp = new SimpleCompositor();
            var dg = new DrawingGroup();
            using (var dc = dg.Open())
            {
                var g0 = new Mock<Glyph>();
                var g1 = new Mock<Glyph>();
                var g2 = new Mock<Glyph>();

                g0.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 6.0, 5.0));
                g1.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 8.0, 7.0));
                g2.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 4.0, 3.0));

                g0.Setup(g => g.DoesBreakLine(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(false);
                g1.Setup(g => g.DoesBreakLine(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(true); 
                g2.Setup(g => g.DoesBreakLine(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(false);

                g0.Setup(g => g.GetHeight()).Returns(5.0);
                g1.Setup(g => g.GetHeight()).Returns(7.0);
                g2.Setup(g => g.GetHeight()).Returns(3.0);

                var glyphs = new List<Glyph> { g0.Object, g1.Object, g2.Object };

                double startTop = 2.0;
                double startLeft = 1.0;
                var args = new ViewEventArgs(dc, startTop, startLeft, 20.0, 100);

                var rows = comp.Compose(glyphs, args);

                Assert.Equal(2, rows.Count);

                var first = rows[0];
                var second = rows[1];

                Assert.Equal(2, first.GetUiGlyphs().Count);
                Assert.Equal(0, first.GetStartIndex());
                Assert.Equal(1, first.GetEndIndex());

                Assert.Equal(2, second.GetStartIndex());
                Assert.Equal(1, first.GetUiGlyphs()[1].GetPhysicalIndex()); 

                Assert.Single(second.GetUiGlyphs());
                Assert.Equal(2, second.GetUiGlyphs()[0].GetPhysicalIndex());

                double expectedSecondTop = startTop + first.GetHeight();
                Assert.Equal(expectedSecondTop, second.GetTop());

                Assert.Equal(startLeft, second.GetUiGlyphs()[0].GetPosition().X);
            }
        }

        [Fact]
        public void Compose_BreakOnLast_SetsEndIndex_AndDoesNotCreateNewRow()
        {
            var comp = new SimpleCompositor();
            var dg = new DrawingGroup();
            using (var dc = dg.Open())
            {
                var g0 = new Mock<Glyph>();
                var g1 = new Mock<Glyph>(); 

                g0.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 6.0, 5.0));
                g1.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 8.0, 7.0));

                g0.Setup(g => g.DoesBreakLine(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(false);
                g1.Setup(g => g.DoesBreakLine(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(true);

                g0.Setup(g => g.GetHeight()).Returns(5.0);
                g1.Setup(g => g.GetHeight()).Returns(7.0);

                var glyphs = new List<Glyph> { g0.Object, g1.Object };

                double startTop = 0;
                double startLeft = 0;
                var args = new ViewEventArgs(dc, startTop, startLeft, 10.0, 100);

                var rows = comp.Compose(glyphs, args);

                Assert.Single(rows);
                var first = rows[0];
                Assert.Equal(0, first.GetStartIndex());
                Assert.Equal(1, first.GetEndIndex());
                Assert.Equal(2, first.GetUiGlyphs().Count);
            }
        }
    }
}
