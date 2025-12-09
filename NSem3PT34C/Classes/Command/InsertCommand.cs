using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSem3PT34.Classes.Structure;

namespace NSem3PT34.Classes.Command
{
    public class InsertCommand : ICommand
    {
        private int physicalIndex;
        private Composition comp;
        private Glyph glyph;

        public InsertCommand(Composition comp, Glyph glyph, int from)
        {
            this.comp = comp;
            this.glyph = glyph;
            this.physicalIndex = from;
        }

        public bool Execute()
        {
            this.comp.Insert(this.glyph, this.physicalIndex);
            this.comp.ModelChanged();
            return true;
        }

        public void UnExecute()
        {
            this.comp.Remove(this.physicalIndex);
            this.comp.ModelChanged();
        }

        public bool CanUndo()
        {
            return true;
        }
    }
}
