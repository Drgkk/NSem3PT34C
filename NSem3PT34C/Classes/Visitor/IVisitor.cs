using NSem3PT34.Classes.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSem3PT34C.Classes.Structure;

namespace NSem3PT34.Classes.Visitor
{
    public abstract class IVisitor
    {
        public abstract void Visit(CharGlyph ch);

        public abstract void Visit(Row row);

        public abstract void Visit(BreakGlyph bg);
    }
}
