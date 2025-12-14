using Moq;
using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Util;
using NSem3PT34.Classes.Visitor;
using NSem3PT34C.Classes.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FontStyle = NSem3PT34.Classes.Util.FontStyle;

namespace TestProject1.StructureTests
{
    public class BreakGlyphTest
    {
        private Font DefaultFont() => new Font("Times New Roman", FontStyle.Normal, 14);

        [Fact]
        public void Draw_BreakChar_DrawsNothing()
        {
            var glyph = new BreakGlyph(DefaultFont());
            var dg = new DrawingGroup();

            using (var dc = dg.Open())
            {
                glyph.Draw(dc, 10, 5);
            }

            Assert.True(dg.Children.Count == 0);
        }

        [Fact]
        public void Select_SingleGlyph_DrawsRectangle()
        {
            var glyph = new BreakGlyph(DefaultFont());
            var dg = new DrawingGroup();
            var highlight = new SolidColorBrush(Colors.Yellow);
            var fontBrush = new SolidColorBrush(Colors.Black);

            using (var dc = dg.Open())
            {
                glyph.Select(dc, highlight, fontBrush, 3, 4);
            }

            Assert.True(dg.Children.Count == 1);
        }

        [Fact]
        public void Bounds_BreakCharachter_CorrectWidthAndHeight()
        {
            var font = DefaultFont();
            var glyphA = new BreakGlyph(font);
            Assert.Equal(glyphA.Bounds(), new Rect(new Point(0, 0), new Size(1, 16.1)));
        }

        [Fact]
        public void Accept_CallsVisitorVisit()
        {
            var glyph = new BreakGlyph(DefaultFont());
            var visitor = new Mock<IVisitor>();

            glyph.Accept(visitor.Object);

            visitor.Verify(s => s.Visit(glyph), Times.Once);
        }

        [Fact]
        public void DoesBreakLine_Always_ReturnsTrue()
        {
            BreakGlyph glyph = new BreakGlyph(DefaultFont());

            Assert.True(glyph.DoesBreakLine(0, 0, 0));
        }

    }
}
