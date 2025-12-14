using NSem3PT34.Classes;
using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Moq;
using NSem3PT34.Classes.Util;

namespace TestProject1.StructureTests
{
    public class CharGlyphTest
    {
        private Font DefaultFont() => new Font("Times New Roman", FontStyle.Normal, 14);

        [Fact]
        public void Draw_SingleChar_DrawsGlyph()
        {
            var glyph = new CharGlyph('A', DefaultFont());
            var dg = new DrawingGroup();

            using (var dc = dg.Open())
            {
                glyph.Draw(dc, 10, 5);
            }

            Assert.True(dg.Children.Count > 0);
        }

        [Fact]
        public void Select_SingleGlyph_DrawsRectangleAndText()
        {
            var glyph = new CharGlyph('B', DefaultFont());
            var dg = new DrawingGroup();
            var highlight = new SolidColorBrush(Colors.Yellow);
            var fontBrush = new SolidColorBrush(Colors.Black);

            using (var dc = dg.Open())
            {
                glyph.Select(dc, highlight, fontBrush, 3, 4);
            }

            Assert.True(dg.Children.Count >= 2);
        }

        [Fact]
        public void Bounds_SingleCharachter_CorrespondWithWidthAndHeight()
        {
            var font = DefaultFont();
            var glyphA = new CharGlyph('A', font);
            var glyphC = new CharGlyph('c', font);
            var glyphSpace = new CharGlyph(' ', font);

            var boundsA = glyphA.Bounds();
            var boundsC = glyphC.Bounds();
            var boundsSpace = glyphSpace.Bounds();

            Assert.Equal(boundsA.Width, glyphA.GetWidth());
            Assert.Equal(boundsA.Height, glyphA.GetHeight());

            Assert.Equal(boundsC.Width, boundsSpace.Width);
            Assert.Equal(boundsC.Height, boundsSpace.Height);
        }

        [Fact]
        public void Accept_CallsVisitorVisit()
        {
            var glyph = new CharGlyph('Y', DefaultFont());
            var visitor = new Mock<IVisitor>();

            glyph.Accept(visitor.Object);

            visitor.Verify(s => s.Visit(glyph), Times.Once);
        }

        [Fact]
        public void DoesBreakLine_ReturnsExpectedValues()
        {
            var glyph = new CharGlyph('x', DefaultFont());
            double glyphWidth = glyph.GetWidth();

            bool breaks = glyph.DoesBreakLine(50, 50 + 2 * glyphWidth - 1, glyphWidth);
            bool noBreak = glyph.DoesBreakLine(0, 1000, glyphWidth);

            Assert.True(breaks);
            Assert.False(noBreak);
        }

        [Fact]
        public void EqualsAndGetHashCode_WorkByCharAndFont()
        {
            var f1 = DefaultFont();
            var f2 = new Font("Times New Roman", FontStyle.Normal, 14);
            var g1 = new CharGlyph('Q', f1);
            var g2 = new CharGlyph('Q', f2);
            var g3 = new CharGlyph('R', f1);

            Assert.True(g1.Equals(g2));
            Assert.Equal(g1.GetHashCode(), g2.GetHashCode());
            Assert.False(g1.Equals(g3));
            Assert.False(g1.Equals(null));
            Assert.False(g1.Equals("not a glyph"));
        }

       
    }
}
