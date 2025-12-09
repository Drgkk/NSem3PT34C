using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NSem3PT34.Classes.Util;
using NSem3PT34.Classes.VM;
using NSem3PT34C.Classes.Structure;

namespace NSem3PT34.Classes.Structure
{
    public class SimpleCompositor : ICompositor
    {
        public SimpleCompositor()
        {
        }

        public List<Row> Compose(List<Glyph> glyphs, ViewEventArgs args)
        {
            List<Row> rows = new List<Row>();
            if (glyphs == null || glyphs.Count == 0)
            {
                return rows;
            }

            Row currentRow = new Row();
            double currentTop = args.GetTop();
            currentRow.SetStartIndex(0);
            currentRow.SetLeft(args.GetLeft());
            currentRow.SetTop(currentTop);
            rows.Add(currentRow);
            double currentLeft = args.GetLeft();
            for (int i = 0; i < glyphs.Count; i++)
            {
                Glyph glyph = glyphs[i];
                Point position = new Point(currentLeft, currentTop);
                UiGlyph uiGlyph = new UiGlyph(glyph, position, i);
                currentRow.GetUiGlyphs().Add(uiGlyph);
                if (glyph.DoesBreakLine(currentLeft, args.GetFrameWidth(), uiGlyph.GetGlyph().Bounds().Width))
                {
                    currentRow.SetEndIndex(i);
                    if (i == glyphs.Count - 1)
                    {
                        break;
                    }

                    currentTop += currentRow.GetHeight();
                    currentRow = new Row();
                    currentRow.SetLeft(args.GetLeft());
                    currentRow.SetTop(currentTop);
                    currentRow.SetStartIndex(i + 1);
                    rows.Add(currentRow);
                    currentLeft = args.GetLeft();
                    position = new Point(currentLeft, currentTop);
                }
                else
                {
                    currentLeft += uiGlyph.GetGlyph().Bounds().Width + 2;
                }
            }

            return rows;
        }

    }
}
