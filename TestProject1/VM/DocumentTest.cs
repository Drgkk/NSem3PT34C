using Moq;
using NSem3PT34.Classes;
using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Util;
using NSem3PT34.Classes.VM;
using NSem3PT34C.Classes.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TestProject1.VM
{
    public class DocumentTest
    {
        [Fact]
        public void UpdateLogicalLocations_SetsPositionsBasedOnGlyphWidths()
        {
            var doc = new Mock<Document>();
            var dg = new DrawingGroup();
            var dc = dg.Open();
            var args = new ViewEventArgs(dc, 4, 4, 300, 200);

            var row1 = new Row();
            var row2 = new Row();

            var g1 = new Mock<Glyph>();
            var g2 = new Mock<Glyph>();
            g1.Setup(g => g.GetWidth()).Returns(10.0);
            g2.Setup(g => g.GetWidth()).Returns(15.0);

            var ui1 = new UiGlyph(g1.Object, new Point(double.NaN, double.NaN), 0);
            var ui2 = new UiGlyph(g2.Object, new Point(double.NaN, double.NaN), 1);
            row1.GetUiGlyphs().Add(ui1);
            row1.GetUiGlyphs().Add(ui2);
            doc.Object.SetRows(new List<Row>{row1});
            doc.Object.UpdateLogicalLocations(args, 0);

            var pos1 = row1.GetUiGlyphs()[0].GetPosition();
            var pos2 = row1.GetUiGlyphs()[1].GetPosition();

            Assert.Equal(args.GetLeft(), pos1.X);
            Assert.Equal(args.GetTop(), pos1.Y);
            Assert.Equal(args.GetLeft() + g1.Object.GetWidth() + 2, pos2.X);
            Assert.Equal(args.GetTop(), pos2.Y);

            dc.Close();
        }
    }
}
