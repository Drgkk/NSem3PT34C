using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace NSem3PT34
{
    public class MyCanvas : FrameworkElement
    {
        public VisualCollection _children;

        public MyCanvas()
        {
            _children = new VisualCollection(this);
        }

        public DrawingContext RenderStart()
        {
            _children.Clear();
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            _children.Add(drawingVisual);

            return drawingContext;
        }

        public void RenderStop(DrawingContext dc)
        {
            dc.Close();
        }

        protected override int VisualChildrenCount => _children.Count;

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _children.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _children[index];
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double w = double.IsInfinity(availableSize.Width) ? MinWidth : Math.Max(MinWidth, availableSize.Width);
            double h = double.IsInfinity(availableSize.Height) ? MinHeight : Math.Max(MinHeight, availableSize.Height);
            return new Size(w, h);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }
    }
}
