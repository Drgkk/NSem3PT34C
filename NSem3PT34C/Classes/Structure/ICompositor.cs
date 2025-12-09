using NSem3PT34.Classes.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSem3PT34.Classes.Structure
{
    public interface ICompositor
    {
        List<Row> Compose(List<Glyph> glyphs, ViewEventArgs args);
    }
}
