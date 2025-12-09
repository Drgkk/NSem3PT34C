using NSem3PT34.Classes.Util;
using NSem3PT34.Classes.Visitor;
using NSem3PT34.Classes.VM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using Color = System.Drawing.Color;
using FontStyle = NSem3PT34.Classes.Util.FontStyle;

namespace NSem3PT34.Classes.Structure
{
    public class CharGlyph : Glyph
    {
        private char ch;
        private Font font;

        public CharGlyph(char ch, Font font)
        {
            this.ch = ch;
            this.font = font;
        }
        public override void Draw(DrawingContext dc, double x, double y)
        {
            var typeFace = new Typeface(new FontFamily(font.Name), font.Style == FontStyle.Italic || font.Style == FontStyle.BoldItalic ? FontStyles.Italic : FontStyles.Normal,
                font.Style == FontStyle.Bold || font.Style == FontStyle.BoldItalic ? FontWeights.Bold : FontWeights.Normal, FontStretches.Normal);
            var ft = new FormattedText(
                ch.ToString(),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                typeFace,
                font.Size,
                System.Windows.Media.Brushes.Black);
            dc.DrawText(ft, new System.Windows.Point(x, y));
        }

        public override void Select(DrawingContext dc, SolidColorBrush highlightColor, SolidColorBrush fontColor, double x, double y)
        {
            char t;
            if ((int)ch == 32)
            {
                t = ' ';
            }
            else
            {
                t = ch;
            }
            Rect highlightRect = Bounds();
            highlightRect.Location = new Point(x, y);
            dc.DrawRectangle(highlightColor, new Pen(System.Windows.Media.Brushes.Black, 0), highlightRect);
            var typeFace = new Typeface(new FontFamily(font.Name), font.Style == FontStyle.Italic || font.Style == FontStyle.BoldItalic ? FontStyles.Italic : FontStyles.Normal,
                font.Style == FontStyle.Bold || font.Style == FontStyle.BoldItalic ? FontWeights.Bold : FontWeights.Normal, FontStretches.Normal);
            var ft = new FormattedText(
                t.ToString(),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                typeFace,
                font.Size,
                fontColor);
            dc.DrawText(ft, new System.Windows.Point(x, y));
        }

        public override Rect Bounds()
        {
            char t;
            if ((int)ch == 32)
            {
                t = 'c';
            }
            else
            {
                t = ch;
            }
            var typeFace = new Typeface(new FontFamily(font.Name), font.Style == FontStyle.Italic || font.Style == FontStyle.BoldItalic ? FontStyles.Italic : FontStyles.Normal,
                    font.Style == FontStyle.Bold || font.Style == FontStyle.BoldItalic ? FontWeights.Bold : FontWeights.Normal, FontStretches.Normal);
            var ft = new FormattedText(
                t.ToString(),
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                typeFace,
                font.Size,
                System.Windows.Media.Brushes.Black);
            return new Rect(new Point(0, 0), new Size(ft.Width, ft.Height));
        }
        public override double GetWidth()
        {

            return Bounds().Width;
        }

        public override double GetHeight()
        {
            return Bounds().Height;
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
            return (currentLeft + 2 * glyphWidth) >= frameWidth;
        }


        public char GetChar()
        {
            return this.ch;
        }

    }
}
