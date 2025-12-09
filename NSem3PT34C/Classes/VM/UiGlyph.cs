using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NSem3PT34.Classes.VM
{
    public class UiGlyph
    {
        private Glyph glyph;
        private Point position;
        private int physicalIndex;

        public UiGlyph(Glyph glyph, Point position, int physicalIndex)
        {
            this.glyph = glyph;
            this.position = position;
            this.physicalIndex = physicalIndex;
        }

        public Glyph GetGlyph()
        {
            return this.glyph;
        }

        public Point GetPosition()
        {
            return this.position;
        }

        public void SetPosition(Point position)
        {
            this.position = position;
        }

        public int GetPhysicalIndex()
        {
            return this.physicalIndex;
        }
    }
}
