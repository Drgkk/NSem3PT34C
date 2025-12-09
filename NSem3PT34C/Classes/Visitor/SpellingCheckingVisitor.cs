using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Util;
using NSem3PT34.Classes.VM;
using NSem3PT34C.Classes.Structure;

namespace NSem3PT34.Classes.Visitor
{
    public class SpellingCheckingVisitor : IVisitor
    {
        private StringBuilder currentWord;
        private List<Glyph> currentGlyphs;
        private Dictionary<UiGlyph, Row> uiGlyphs;
        private UiGlyph lastAdded;
        private ISpellingErrorHandler spellingErrorHandler;

        public SpellingCheckingVisitor()
        {
            this.currentWord = new StringBuilder();
            this.currentGlyphs = new List<Glyph>();
            this.uiGlyphs = new Dictionary<UiGlyph, Row>();
        }

        public SpellingCheckingVisitor(ISpellingErrorHandler spellingErrorHandler) : this()
        {
            this.spellingErrorHandler = spellingErrorHandler;
        }

        public override void Visit(CharGlyph character)
        {
            if (char.IsLetter(character.GetChar())
                || char.IsDigit(character.GetChar()))
            {
                this.currentWord.Append(character.GetChar());
                this.currentGlyphs.Add(character);
            }
            else
            {
                this.uiGlyphs.Remove(lastAdded);
                this.SpellCheck();
                this.uiGlyphs.Clear();
            }
        }

        private void SpellCheck()
        {
            string word = this.currentWord.ToString();
            word = word.ToLower();
            if (!word.Equals("")
                && SpellChecker.GetInstance().IsMisspelled(word))
            {
                if (this.spellingErrorHandler != null)
                {
                    this.spellingErrorHandler.HandleSpellingError(this.uiGlyphs);
                }
            }
            this.currentWord = new StringBuilder();
            this.currentGlyphs.Clear();
        }


        public override void Visit(Row row)
        {
            List<UiGlyph> glyphs = row.GetUiGlyphs();
            foreach (UiGlyph uiGlyph in glyphs)
            {
                this.uiGlyphs.Add(uiGlyph, row);
                lastAdded = uiGlyph;
                uiGlyph.GetGlyph().Accept(this);
            }

        }

        public override void Visit(BreakGlyph bg)
        {
            this.uiGlyphs.Remove(lastAdded);
            this.SpellCheck();
            this.uiGlyphs.Clear();
        }
    }
}
