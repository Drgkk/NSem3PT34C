using NSem3PT34.Classes.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSem3PT34.Classes.Structure;

namespace NSem3PT34.Classes.Util
{
    public interface ISpellingErrorHandler
    {
        void HandleSpellingError(Dictionary<UiGlyph, Row> glyphs);
    }
}
