using NSem3PT34.Classes.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSem3PT34C.Classes.Structure;

namespace NSem3PT34.Classes.Visitor
{
    public interface IVisitor
    {
        public void Visit(CharGlyph ch);

        public void Visit(Row row);

        public void Visit(BreakGlyph bg);
    }
}
