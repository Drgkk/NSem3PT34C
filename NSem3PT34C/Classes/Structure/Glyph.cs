
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using NSem3PT34.Classes.Util;
using NSem3PT34.Classes.Visitor;

namespace NSem3PT34.Classes
{
    public abstract class Glyph
    {
        public abstract void Draw(DrawingContext dc, double x, double y);
        public abstract void Select(DrawingContext dc, SolidColorBrush highlightColor, SolidColorBrush fontColor, double x, double y);
        public abstract double GetWidth();
        public abstract double GetHeight();
        public abstract Rect Bounds();
        public abstract Font? GetFont();
        public abstract void SetFont(Font font);
        public abstract void Accept(IVisitor visitor);
        public abstract bool DoesBreakLine(double currentLeft, double frameWidth, double glyphWidth);
    }
}
