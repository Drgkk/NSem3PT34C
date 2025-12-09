using NSem3PT34.Classes.Util;
using NSem3PT34.Classes.Visitor;
using NSem3PT34.Classes.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;

namespace NSem3PT34.Classes.Structure
{
    public class Row : Glyph
    {
        private int startIndex;
        private int endIndex;
        private double left;
        private double top;
        private List<UiGlyph> uiGlyphs;

        public Row()
        {
            this.uiGlyphs = new List<UiGlyph>();
        }

        public List<UiGlyph> GetUiGlyphs()
        {
            return this.uiGlyphs;
        }

        public override double GetHeight()
        {
            double height = 0;
            foreach (UiGlyph uiGlyph in uiGlyphs)
            {
                if (height < uiGlyph.GetGlyph().GetHeight())
                {
                    height = uiGlyph.GetGlyph().GetHeight();
                }
            }

            double lineGap = 0;
            return height + lineGap * height;
        }

        public override double GetWidth()
        {
            double width = 2;
            foreach (UiGlyph uiGlyph in uiGlyphs)
            {
                width += uiGlyph.GetGlyph().GetWidth();
            }

            return width;
        }

        public override void Draw(DrawingContext dc, double x, double y)
        {
            double currentLeft = x;
            double maxHeightRow = Bounds().Height;
            foreach (UiGlyph uiGlyph in uiGlyphs)
            {
                uiGlyph.GetGlyph().Draw(dc, currentLeft, y + maxHeightRow - uiGlyph.GetGlyph().Bounds().Height);
                currentLeft += uiGlyph.GetGlyph().Bounds().Width + 2;
            }
        }

        public override Rect Bounds()
        {
            double width = 0;
            double height = 0;
            double y = Int32.MaxValue;
            foreach (UiGlyph uiGlyph in uiGlyphs)
            {
                width += uiGlyph.GetGlyph().Bounds().Width;
                height = height > uiGlyph.GetGlyph().Bounds().Height ? height : uiGlyph.GetGlyph().Bounds().Height;
                y = y < uiGlyph.GetPosition().Y ? y : uiGlyph.GetPosition().Y;
            }

            return new Rect(new Point(0, y), new Size(width, height));
        }

        public override void Select(DrawingContext graphics, SolidColorBrush hightlightColor,
            SolidColorBrush fontColor, double x, double y)
        {
            this.Select(graphics, hightlightColor, fontColor, x, y, 0, this
                    .GetUiGlyphs().Count() - 1);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool DoesBreakLine(double currentLeft, double frameWidth, double glyphWidth)
        {
            return false;
        }


        public override void SetFont(Font font)
        {
        }

        public override Font? GetFont()
        {
            return null;
        }

        public void Select(DrawingContext dc, SolidColorBrush hightlightColor,
            SolidColorBrush fontColor, double x, double y, int start, int end)
        {
            try
            {
                double maxHeightRow = Bounds().Height;
                for (int i = start; i <= end; i++)
                {
                    UiGlyph uiGlyph = this.GetUiGlyphs()[i];
                    uiGlyph.GetGlyph().Select(dc, hightlightColor, fontColor,
                        uiGlyph.GetPosition().X,
                        uiGlyph.GetPosition().Y + maxHeightRow - uiGlyph.GetGlyph().Bounds().Height);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public int GetStartIndex()
        {
            return startIndex;
        }

        public void SetStartIndex(int startIndex)
        {
            this.startIndex = startIndex;
        }

        public int GetEndIndex()
        {
            return endIndex;
        }

        public void SetEndIndex(int endIndex)
        {
            this.endIndex = endIndex;
        }

        public double GetLeft()
        {
            return left;
        }

        public void SetLeft(double left)
        {
            this.left = left;
        }

        public double GetTop()
        {
            return top;
        }

        public void SetTop(double top)
        {
            this.top = top;
        }

    }
}
