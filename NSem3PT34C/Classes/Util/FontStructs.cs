using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSem3PT34.Classes.Util
{
    public struct Font
    {
        public Font(string name, FontStyle fs, int size)
        {
            this.Name = name;
            this.Style = fs;
            this.Size = size;
        }
        public string Name { get; set; }
        public FontStyle Style { get; set; }
        public int Size { get; set; }
    }

    public enum FontStyle
    {
        Normal = 0,
        Bold = 1,
        Italic = 2,
        BoldItalic = 3
    }
}
