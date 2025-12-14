using Moq;
using NSem3PT34.Classes;
using NSem3PT34.Classes.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestProject1.VM
{
    public class UIGlyphTest
    {
        [Fact]
        public void Constructor_StoresGlyphPositionAndIndex()
        {
            var mockGlyph = new Mock<Glyph>().Object;
            var pos = new Point(12.5, 7.5);
            int index = 42;

            var uiGlyph = new UiGlyph(mockGlyph, pos, index);

            Assert.Same(mockGlyph, uiGlyph.GetGlyph());
            Assert.Equal(pos, uiGlyph.GetPosition());
            Assert.Equal(index, uiGlyph.GetPhysicalIndex());
        }

        [Fact]
        public void GetGlyph_ReturnsSameInstance()
        {
            var mockGlyph = new Mock<Glyph>().Object;
            var uiGlyph = new UiGlyph(mockGlyph, new Point(0, 0), 0);

            var returned = uiGlyph.GetGlyph();

            Assert.Same(mockGlyph, returned);
        }

        [Fact]
        public void GetPosition_And_SetPosition_Work()
        {
            var mockGlyph = new Mock<Glyph>().Object;
            var initial = new Point(1, 2);
            var updated = new Point(9.9, -3.3);

            var uiGlyph = new UiGlyph(mockGlyph, initial, 1);

            Assert.Equal(initial, uiGlyph.GetPosition());

            uiGlyph.SetPosition(updated);

            Assert.Equal(updated, uiGlyph.GetPosition());
        }

        [Fact]
        public void GetPhysicalIndex_ReturnsProvidedIndex()
        {
            var mockGlyph = new Mock<Glyph>().Object;
            var uiGlyph = new UiGlyph(mockGlyph, new Point(0, 0), 7);

            Assert.Equal(7, uiGlyph.GetPhysicalIndex());
        }
    }
}
