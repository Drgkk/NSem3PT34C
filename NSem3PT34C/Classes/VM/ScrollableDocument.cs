using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Util;

namespace NSem3PT34C.Classes.VM
{
    public class ScrollableDocument : Document
    {
        private Document document;

        public ScrollableDocument(Document document)
        {
            this.document = document;
        }

        public override void Draw(List<Row> rows, ViewEventArgs args, int from)
        {
            if (from >= rows.Count)
            {
                from = 0;
            }

            this.SetRows(rows);
            if (NeedScrolling(args.GetFrameHeight()) || from != 0)
            {
                DrawingContext dc = args.GetGraphics();
                var typeFace = new Typeface(new FontFamily("Arial"), FontStyles.Normal,
                    FontWeights.Bold, FontStretches.Normal);
                var ft = new FormattedText(
                    (from + 1).ToString(),
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    typeFace,
                    12,
                    System.Windows.Media.Brushes.Black);
                dc.DrawText(ft, new System.Windows.Point(0, 0));
            }
            this.document.Draw(rows, args, from);
        }

        public override bool NeedScrolling(double frameHeight)
        {
            bool needScrolling = false;
            double totalHeight = 0;
            foreach (Row row in this.document.GetRows())
            {
                totalHeight += row.GetHeight();
            }

            if (totalHeight > frameHeight)
            {
                needScrolling = true;
            }

            return needScrolling;
        }
    }
}
