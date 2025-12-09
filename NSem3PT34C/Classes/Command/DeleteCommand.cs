using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSem3PT34.Classes.Structure;

namespace NSem3PT34.Classes.Command
{
    public class DeleteCommand : ICommand
    {
        private Composition comp;
        private int startFrom;
        private int endAt;
        private List<Glyph> buffer;

        public DeleteCommand(Composition comp, int startFrom, int endAt)
        {
            this.comp = comp;
            this.startFrom = startFrom;
            this.endAt = endAt;
            this.buffer = new List<Glyph>();
            for (int i = this.startFrom; i <= this.endAt; i++)
            {
                this.buffer.Add(this.comp.GetChildren()[i]);
            }
        }

        public bool Execute()
        {
            try
            {
                int i = this.endAt;
                while (i >= this.startFrom)
                {
                    this.comp.Remove(i);
                    i--;
                }
                this.comp.ModelChanged();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);                
                return false;
            }
        }

        public void UnExecute()
        {
            try
            {
                int i, j;
                for (i = startFrom, j = 0; i <= this.endAt; i++, j++)
                {
                    this.comp.Insert(this.buffer[j], i);
                }
                this.comp.ModelChanged();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public bool CanUndo()
        {
            return true;
        }
    }
}
