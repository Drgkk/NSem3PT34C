using Moq;
using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Visitor;
using NSem3PT34.Classes.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using NSem3PT34.Classes;
using NSem3PT34.Classes.Util;
using FontStyle = System.Windows.FontStyle;

namespace TestProject1.StructureTests
{
    public class RowTest
    {
        private Font DefaultFont() => new Font("Times New Roman", NSem3PT34.Classes.Util.FontStyle.Normal, 14);

        [Fact]
        public void GetHeight_Returns_MaxOfMockGlyphHeights()
        {
            var row = new Row();

            var g1 = new Mock<Glyph>();
            var g2 = new Mock<Glyph>();

            g1.Setup(g => g.GetHeight()).Returns(5.0);
            g2.Setup(g => g.GetHeight()).Returns(9.0);

            row.GetUiGlyphs().Add(new UiGlyph(g1.Object, new Point(0, 10), 0));
            row.GetUiGlyphs().Add(new UiGlyph(g2.Object, new Point(20, 3), 1));

            var height = row.GetHeight();

            Assert.Equal(9.0, height);
        }

        [Fact]
        public void GetWidth_SumsMockGlyphWidthsPlusSpacing()
        {
            var row = new Row();

            var g1 = new Mock<Glyph>();
            var g2 = new Mock<Glyph>();

            g1.Setup(g => g.GetWidth()).Returns(10.0);
            g2.Setup(g => g.GetWidth()).Returns(12.0);

            row.GetUiGlyphs().Add(new UiGlyph(g1.Object, new Point(0, 0), 0));
            row.GetUiGlyphs().Add(new UiGlyph(g2.Object, new Point(0, 0), 1));

            var width = row.GetWidth();

            Assert.Equal(2 + 10.0 + 12.0, width);
        }

        [Fact]
        public void Bounds_ComputesWidthHeightAndMinY_FromMocks()
        {
            var row = new Row();

            var g1 = new Mock<Glyph>();
            var g2 = new Mock<Glyph>();

            g1.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 7.0, 4.0));
            g2.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 13.0, 6.0));

            var p1 = new Point(5, 12);
            var p2 = new Point(10, 3);

            row.GetUiGlyphs().Add(new UiGlyph(g1.Object, p1, 0));
            row.GetUiGlyphs().Add(new UiGlyph(g2.Object, p2, 1));

            var bounds = row.Bounds();

            Assert.Equal(7.0 + 13.0, bounds.Width);
            Assert.Equal(6.0, bounds.Height);
            Assert.Equal(Math.Min(p1.Y, p2.Y), bounds.Y);
            Assert.Equal(0.0, bounds.X); 
        }

        [Fact]
        public void Draw_InvokesDraw_OnEachContainedGlyph()
        {
            var row = new Row();
            var mg1 = new Mock<Glyph>();
            var mg2 = new Mock<Glyph>();

            mg1.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 10.0, 6.0));
            mg2.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 12.0, 8.0));

            row.GetUiGlyphs().Add(new UiGlyph(mg1.Object, new Point(0, 10), 0));
            row.GetUiGlyphs().Add(new UiGlyph(mg2.Object, new Point(20, 5), 1));

            var dg = new DrawingGroup();
            using (var dc = dg.Open())
            {
                row.Draw(dc, 10.0, 7.0);
            }

            mg1.Verify(g => g.Draw(It.IsAny<DrawingContext>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
            mg2.Verify(g => g.Draw(It.IsAny<DrawingContext>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
        }

        [Fact]
        public void Select_InvokesSelect_OnEachContainedGlyph()
        {
            var row = new Row();
            var mg1 = new Mock<Glyph>();
            var mg2 = new Mock<Glyph>();

            mg1.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 10.0, 6.0));
            mg2.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 12.0, 8.0));

            var ui1 = new UiGlyph(mg1.Object, new Point(5, 12), 0);
            var ui2 = new UiGlyph(mg2.Object, new Point(20, 3), 1);
            row.GetUiGlyphs().Add(ui1);
            row.GetUiGlyphs().Add(ui2);

            var dg = new DrawingGroup();
            var highlight = new SolidColorBrush(System.Windows.Media.Colors.LightBlue);
            var fontBrush = new SolidColorBrush(System.Windows.Media.Colors.Black);

            using (var dc = dg.Open())
            {
                row.Select(dc, highlight, fontBrush, 0, 0);
            }

            mg1.Verify(g => g.Select(It.IsAny<DrawingContext>(), It.IsAny<SolidColorBrush>(), It.IsAny<SolidColorBrush>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
            mg2.Verify(g => g.Select(It.IsAny<DrawingContext>(), It.IsAny<SolidColorBrush>(), It.IsAny<SolidColorBrush>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
        }

        [Fact]
        public void Select_Range_SelectsOnlySpecifiedIndices()
        {
            var row = new Row();
            var mg1 = new Mock<Glyph>();
            var mg2 = new Mock<Glyph>();
            var mg3 = new Mock<Glyph>();

            mg1.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 10.0, 6.0));
            mg2.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 12.0, 8.0));
            mg3.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 12.0, 8.0));

            var ui1 = new UiGlyph(mg1.Object, new Point(5, 12), 0);
            var ui2 = new UiGlyph(mg2.Object, new Point(20, 3), 1);
            var ui3 = new UiGlyph(mg3.Object, new Point(28, 3), 2);
            row.GetUiGlyphs().Add(ui1);
            row.GetUiGlyphs().Add(ui2);
            row.GetUiGlyphs().Add(ui3);

            var dg = new DrawingGroup();
            var highlight = new SolidColorBrush(System.Windows.Media.Colors.LightBlue);
            var fontBrush = new SolidColorBrush(System.Windows.Media.Colors.Black);

            using (var dc = dg.Open())
            {
                row.Select(dc, highlight, fontBrush, 0, 0, 1, 2);
            }

            mg1.Verify(g => g.Select(It.IsAny<DrawingContext>(), It.IsAny<SolidColorBrush>(), It.IsAny<SolidColorBrush>(), It.IsAny<double>(), It.IsAny<double>()), Times.Never);
            mg2.Verify(g => g.Select(It.IsAny<DrawingContext>(), It.IsAny<SolidColorBrush>(), It.IsAny<SolidColorBrush>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
            mg3.Verify(g => g.Select(It.IsAny<DrawingContext>(), It.IsAny<SolidColorBrush>(), It.IsAny<SolidColorBrush>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);

        }

        [Fact]
        public void Accept_CallsVisitorVisit()
        {
            var row = new Row();
            var visitorMock = new Mock<IVisitor>();

            row.Accept(visitorMock.Object);

            visitorMock.Verify(v => v.Visit(row), Times.Once);
        }

        [Fact]
        public void DoesBreakLine_Always_ReturnsFalse()
        {
            var row = new Row();
            Assert.False(row.DoesBreakLine(0, 1, 1));
            Assert.False(row.DoesBreakLine(100, 10, 5));
        }

        [Fact]
        public void StartEndLeftTop_SettersAndGetters_Work()
        {
            var row = new Row();

            row.SetStartIndex(3);
            row.SetEndIndex(7);
            row.SetLeft(12.5);
            row.SetTop(99.9);

            Assert.Equal(3, row.GetStartIndex());
            Assert.Equal(7, row.GetEndIndex());
            Assert.Equal(12.5, row.GetLeft());
            Assert.Equal(99.9, row.GetTop());
        }
    }
}
