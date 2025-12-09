using NSem3PT34.Classes.Command;
using NSem3PT34.Classes.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using NSem3PT34.Classes.Util;

namespace NSem3PT34C.Classes.Command
{
    public class ChangeFontCommand : ICommand
    {
        private DrawingContext graphics;
        private Composition cmp;
        private int startFrom;
        private int endAt;
        private List<Font> previousFonts;
        private string changeTo;

        public ChangeFontCommand(DrawingContext graphics, Composition cmp, int startFrom, int endAt, string changeTo)
        {
            this.graphics = graphics;
            this.cmp = cmp;
            this.startFrom = startFrom;
            this.endAt = endAt;
            this.changeTo = changeTo;
            this.LoadPreviousFonts();
        }

        public bool Execute()
        {
            bool val = true;
            try
            {
                List<Font> fonts = new List<Font>();
                for (int i = this.startFrom; i <= this.endAt; i++)
                {
                    Font previousFont = this.cmp.GetChildren()[i].GetFont().Value;
                    Font newFont = new Font(changeTo,
                        previousFont.Style, previousFont.Size);
                    fonts.Add(newFont);
                }

                this.cmp.UpdateFont(fonts, startFrom, endAt);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                val = false;
            }

            return val;
        }

        public void UnExecute()
        {
            this.cmp.UpdateFont(this.previousFonts, this.startFrom, this.endAt);
        }

        public bool CanUndo()
        {
            return true;
        }

        private void LoadPreviousFonts()
        {
            this.previousFonts = new List<Font>();
            for (int i = this.startFrom; i <= this.endAt; i++)
            {
                this.previousFonts.Add(this.cmp.GetChildren()[i].GetFont().Value);
            }
        }
    }
}
