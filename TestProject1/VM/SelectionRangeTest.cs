using NSem3PT34C.Classes.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.VM
{
    public class SelectionRangeTest
    {
        [Fact]
        public void DefaultConstructor_InitializesAllToZero()
        {
            var r = new SelectionRange();

            Assert.Equal(0, r.GetStartRow());
            Assert.Equal(0, r.GetStartCol());
            Assert.Equal(0, r.GetEndRow());
            Assert.Equal(0, r.GetEndCol());
            Assert.True(r.IsSingleGlyphSelection());
        }

        [Fact]
        public void ParamConstructor_SetsAllValues()
        {
            var r = new SelectionRange(1, 2, 3, 4);

            Assert.Equal(1, r.GetStartRow());
            Assert.Equal(2, r.GetStartCol());
            Assert.Equal(3, r.GetEndRow());
            Assert.Equal(4, r.GetEndCol());
            Assert.False(r.IsSingleGlyphSelection());
        }

        [Fact]
        public void Setters_UpdateValuesCorrectly()
        {
            var r = new SelectionRange();
            r.SetStartRow(5);
            r.SetStartCol(6);
            r.SetEndRow(5);
            r.SetEndCol(6);

            Assert.Equal(5, r.GetStartRow());
            Assert.Equal(6, r.GetStartCol());
            Assert.Equal(5, r.GetEndRow());
            Assert.Equal(6, r.GetEndCol());
            Assert.True(r.IsSingleGlyphSelection());
        }

        [Fact]
        public void IsSingleGlyphSelection_BecomesFalse_WhenDifferent()
        {
            var r = new SelectionRange(0, 0, 0, 1);
            Assert.False(r.IsSingleGlyphSelection());

            r.SetEndCol(0);
            r.SetEndRow(1);
            Assert.False(r.IsSingleGlyphSelection());
        }
    }
}
