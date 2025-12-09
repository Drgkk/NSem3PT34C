using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Util;

namespace NSem3PT34.Classes.Command
{
    public class ToggleBoldCommand : ICommand
    {
        private DrawingContext graphics;
        private Composition comp;
        private int startFrom;
        private int endAt;
        private List<Font> previousFonts;

        public ToggleBoldCommand(DrawingContext graphics, Composition comp,
            int startFrom, int endAt)
        {
            this.graphics = graphics;
            this.comp = comp;
            this.startFrom = startFrom;
            this.endAt = endAt;
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
                    Font previousFont = this.comp.GetChildren()[i].GetFont().Value;
                    Font newFont = new Font(previousFont.Name,
                        FontStyle.Bold,
                        previousFont.Size);
                    if (previousFont.Style == FontStyle.Normal)
                    {
                        newFont = new Font(previousFont.Name,
                            FontStyle.Bold,
                            previousFont.Size);
                    }
                    else if (previousFont.Style == FontStyle.Bold)
                    {
                        newFont = new Font(previousFont.Name,
                            FontStyle.Normal,
                            previousFont.Size);
                    }
                    else if (previousFont.Style == FontStyle.Italic)
                    {
                        newFont = new Font(previousFont.Name,
                            FontStyle.BoldItalic,
                            previousFont.Size);
                    }
                    else if (previousFont.Style == FontStyle.BoldItalic)
                    {
                        newFont = new Font(previousFont.Name,
                            FontStyle.Italic,
                            previousFont.Size);
                    }
                    fonts.Add(newFont);
                }

                this.comp.UpdateFont(fonts, startFrom, endAt);
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
            this.comp.UpdateFont(this.previousFonts, this.startFrom, this.endAt);
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
                this.previousFonts
                    .Add(this.comp.GetChildren()[i].GetFont().Value);
            }
        }
    }
}
