using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using NSem3PT34.Classes;
using NSem3PT34.Classes.Util;
using NSem3PT34.Classes.Visitor;

namespace NSem3PT34C.Classes.Structure
{
    public class BreakGlyph : Glyph
    {
        private Font font;

        public BreakGlyph(Font font)
        {
            this.font = font;
        }
        public override void Draw(DrawingContext dc, double x, double y)
        {
            
        }

        public override void Select(DrawingContext dc, SolidColorBrush highlightColor, SolidColorBrush fontColor, double x, double y)
        {
            Rect highlightRect = Bounds();
            highlightRect.Location = new Point(x, y);
            dc.DrawRectangle(highlightColor, new Pen(System.Windows.Media.Brushes.Black, 0), highlightRect);
        }

        public override double GetWidth()
        {
            return Bounds().Width;
        }

        public override double GetHeight()
        {
            return Bounds().Height;
        }

        public override Rect Bounds()
        {
            char t = 'c';
            var typeFace = new Typeface(new FontFamily(font.Name), FontStyles.Normal,
                FontWeights.Normal, FontStretches.Normal);
            var ft = new FormattedText(
                t.ToString(),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                typeFace,
                font.Size,
                System.Windows.Media.Brushes.Black);
            return new Rect(new Point(0, 0), new Size(1, ft.Height));
        }

        public override Font? GetFont()
        {
            return this.font;
        }

        public override void SetFont(Font font)
        {
            this.font = font;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool DoesBreakLine(double currentLeft, double frameWidth, double glyphWidth)
        {
            return true;
        }

    }
}
