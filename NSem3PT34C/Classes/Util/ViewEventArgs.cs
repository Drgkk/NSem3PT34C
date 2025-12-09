using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NSem3PT34.Classes.Util
{
    public class ViewEventArgs
    {
        private DrawingContext graphics;
        private double top;
        private double left;
        private double frameWidth;
        private double frameHeight;

        public ViewEventArgs(DrawingContext graphics, double top, double left, double frameWidth, double frameHeight)
        {
            this.graphics = graphics;
            this.top = top;
            this.left = left;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
        }

        public DrawingContext GetGraphics()
        {
            return this.graphics;
        }

        public double GetTop()
        {
            return this.top;
        }

        public double GetLeft()
        {
            return this.left;
        }

        public double GetFrameWidth()
        {
            return this.frameWidth;
        }

        public double GetFrameHeight()
        {
            return this.frameHeight;
        }
    }
}
