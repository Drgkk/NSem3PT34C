using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSem3PT34C.Classes.VM
{
    public class SelectionRange
    {
        private int startRow;
        private int startCol;
        private int endRow;
        private int endCol;

        public SelectionRange()
        {
        }

        public SelectionRange(int startRow, int startCol, int endRow, int endCol)
        {
            this.SetStartRow(startRow);
            this.SetStartCol(startCol);
            this.SetEndRow(endRow);
            this.SetEndCol(endCol);
        }

        public int GetStartRow()
        {
            return startRow;
        }

        public void SetStartRow(int startRow)
        {
            this.startRow = startRow;
        }

        public int GetStartCol()
        {
            return startCol;
        }

        public void SetStartCol(int startCol)
        {
            this.startCol = startCol;
        }

        public int GetEndRow()
        {
            return endRow;
        }

        public void SetEndRow(int endRow)
        {
            this.endRow = endRow;
        }

        public int GetEndCol()
        {
            return endCol;
        }

        public void SetEndCol(int endCol)
        {
            this.endCol = endCol;
        }

        public bool IsSingleGlyphSelection()
        {
            return (this.startRow == this.endRow) && (this.startCol == this.endCol);
        }
    }
}
