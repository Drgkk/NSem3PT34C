using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Util;
using NSem3PT34.Classes.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NSem3PT34C.Classes.VM
{
    public abstract class Document
    {
        private List<Row> rows;

        public Document()
        {
            this.rows = new List<Row>();
        }

        public Document(List<Row> rows)
        {
            this.rows = rows;
        }

        public List<Row> GetRows()
        {
            return this.rows;
        }

        public void SetRows(List<Row> rows)
        {
            this.rows = rows;
        }

        public abstract void Draw(List<Row> rows, ViewEventArgs args, int from);

        public abstract bool NeedScrolling(double frameHeight);

        public void UpdateLogicalLocations(ViewEventArgs args, int index)
        {
            int i, j;
            Point dummyPoint = new Point(Int32.MinValue, Int32.MinValue);
            for (i = 0; i < index; i++)
            {
                Row currentRow = this.GetRows()[i];
                currentRow.SetTop(Int32.MinValue);
                currentRow.SetLeft(Int32.MinValue);
                foreach (UiGlyph uiGlyph in currentRow.GetUiGlyphs())
                {
                    uiGlyph.SetPosition(dummyPoint);
                }
            }

            double currentTop = args.GetTop();
            double currentLeft = args.GetLeft();
            for (j = i; j < this.GetRows().Count; j++)
            {
                Row currentRow = this.GetRows()[j];
                currentRow.SetTop(currentTop);
                currentRow.SetLeft(currentLeft);
                foreach (UiGlyph uiGlyph in currentRow.GetUiGlyphs())
                {
                    Point position = new Point(currentLeft, currentTop);
                    uiGlyph.SetPosition(position);
                    currentLeft += uiGlyph.GetGlyph().GetWidth() + 2;
                }

                currentTop += currentRow.GetHeight();
                currentLeft = args.GetLeft();
            }
        }
    }
}
