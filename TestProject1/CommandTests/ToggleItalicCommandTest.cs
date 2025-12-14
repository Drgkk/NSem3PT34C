using NSem3PT34.Classes.Command;
using NSem3PT34.Classes.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSem3PT34.Classes.Util;

namespace TestProject1.CommandTests
{
    public class ToggleItalicCommandTest
    {
        [Fact]
        public void Execute_CorrectParameters_TogglesBold()
        {
            int startFrom = 1;
            int endAt = 5;
            Composition comp = new Composition();
            comp.Insert(new CharGlyph('h', new Font("Times New Roman", FontStyle.Normal, 14)), 0);
            comp.Insert(new CharGlyph('e', new Font("Times New Roman", FontStyle.Normal, 14)), 1);
            comp.Insert(new CharGlyph('l', new Font("Times New Roman", FontStyle.Normal, 14)), 2);
            comp.Insert(new CharGlyph('l', new Font("Times New Roman", FontStyle.Normal, 14)), 3);
            comp.Insert(new CharGlyph('o', new Font("Times New Roman", FontStyle.Normal, 14)), 4);
            comp.Insert(new CharGlyph('w', new Font("Times New Roman", FontStyle.Normal, 14)), 5);
            comp.Insert(new CharGlyph('o', new Font("Times New Roman", FontStyle.Normal, 14)), 6);
            comp.Insert(new CharGlyph('r', new Font("Times New Roman", FontStyle.Normal, 14)), 7);
            comp.Insert(new CharGlyph('l', new Font("Times New Roman", FontStyle.Normal, 14)), 8);
            comp.Insert(new CharGlyph('d', new Font("Times New Roman", FontStyle.Normal, 14)), 9);

            ToggleItalicCommand italicCommand = new ToggleItalicCommand(null, comp, startFrom, endAt);


            italicCommand.Execute();

            for (int i = startFrom; i <= endAt; i++)
            {
                var font = comp.GetChildren()[i].GetFont().Value;
                Assert.Equal(FontStyle.Italic, font.Style);
            }
        }

        [Fact]
        public void Execute_CorrectParameters_UntogglesBold()
        {
            int startFrom = 1;
            int endAt = 5;
            Composition comp = new Composition();
            comp.Insert(new CharGlyph('h', new Font("Times New Roman", FontStyle.Italic, 14)), 0);
            comp.Insert(new CharGlyph('e', new Font("Times New Roman", FontStyle.Italic, 14)), 1);
            comp.Insert(new CharGlyph('l', new Font("Times New Roman", FontStyle.Italic, 14)), 2);
            comp.Insert(new CharGlyph('l', new Font("Times New Roman", FontStyle.Italic, 14)), 3);
            comp.Insert(new CharGlyph('o', new Font("Times New Roman", FontStyle.Italic, 14)), 4);
            comp.Insert(new CharGlyph('w', new Font("Times New Roman", FontStyle.Italic, 14)), 5);
            comp.Insert(new CharGlyph('o', new Font("Times New Roman", FontStyle.Italic, 14)), 6);
            comp.Insert(new CharGlyph('r', new Font("Times New Roman", FontStyle.Italic, 14)), 7);
            comp.Insert(new CharGlyph('l', new Font("Times New Roman", FontStyle.Italic, 14)), 8);
            comp.Insert(new CharGlyph('d', new Font("Times New Roman", FontStyle.Italic, 14)), 9);

            ToggleItalicCommand italicCommand = new ToggleItalicCommand(null, comp, startFrom, endAt);


            italicCommand.Execute();

            for (int i = startFrom; i <= endAt; i++)
            {
                var font = comp.GetChildren()[i].GetFont().Value;
                Assert.Equal(FontStyle.Normal, font.Style);
            }
        }

        [Fact]
        public void Constructor_OutOfRange_ThrowsException()
        {
            int startFrom = 1;
            int endAt = 12;
            Composition comp = new Composition();
            comp.Insert(new CharGlyph('h', new Font("Times New Roman", FontStyle.Bold, 14)), 0);
            comp.Insert(new CharGlyph('e', new Font("Times New Roman", FontStyle.Bold, 14)), 1);
            comp.Insert(new CharGlyph('l', new Font("Times New Roman", FontStyle.Bold, 14)), 2);
            comp.Insert(new CharGlyph('l', new Font("Times New Roman", FontStyle.Bold, 14)), 3);
            comp.Insert(new CharGlyph('o', new Font("Times New Roman", FontStyle.Bold, 14)), 4);
            comp.Insert(new CharGlyph('w', new Font("Times New Roman", FontStyle.Bold, 14)), 5);
            comp.Insert(new CharGlyph('o', new Font("Times New Roman", FontStyle.Bold, 14)), 6);
            comp.Insert(new CharGlyph('r', new Font("Times New Roman", FontStyle.Bold, 14)), 7);
            comp.Insert(new CharGlyph('l', new Font("Times New Roman", FontStyle.Bold, 14)), 8);
            comp.Insert(new CharGlyph('d', new Font("Times New Roman", FontStyle.Bold, 14)), 9);

            var act = () => new ToggleItalicCommand(null, comp, startFrom, endAt);
            Assert.Throws<ArgumentOutOfRangeException>(act);

        }

        [Fact]
        public void Unexecute_CorrectParameters_ReversesBoldness()
        {
            int startFrom = 1;
            int endAt = 7;
            Composition comp = new Composition();
            comp.Insert(new CharGlyph('h', new Font("Times New Roman", FontStyle.Normal, 14)), 0);
            comp.Insert(new CharGlyph('e', new Font("Times New Roman", FontStyle.Normal, 14)), 1);
            comp.Insert(new CharGlyph('l', new Font("Times New Roman", FontStyle.Normal, 14)), 2);
            comp.Insert(new CharGlyph('l', new Font("Times New Roman", FontStyle.Normal, 14)), 3);
            comp.Insert(new CharGlyph('o', new Font("Times New Roman", FontStyle.Normal, 14)), 4);
            comp.Insert(new CharGlyph('w', new Font("Times New Roman", FontStyle.Normal, 14)), 5);
            comp.Insert(new CharGlyph('o', new Font("Times New Roman", FontStyle.Normal, 14)), 6);
            comp.Insert(new CharGlyph('r', new Font("Times New Roman", FontStyle.Normal, 14)), 7);
            comp.Insert(new CharGlyph('l', new Font("Times New Roman", FontStyle.Normal, 14)), 8);
            comp.Insert(new CharGlyph('d', new Font("Times New Roman", FontStyle.Normal, 14)), 9);

            ToggleItalicCommand italicCommand = new ToggleItalicCommand(null, comp, startFrom, endAt);


            italicCommand.Execute();
            italicCommand.UnExecute();
            for (int i = startFrom; i <= endAt; i++)
            {
                var font = comp.GetChildren()[i].GetFont().Value;
                Assert.Equal(FontStyle.Normal, font.Style);
            }

        }

        [Fact]
        public void CanUndo_Always_ReturnsTrue()
        {
            int startFrom = 1;
            int endAt = 7;
            Composition comp = new Composition();
            comp.Insert(new CharGlyph('h', new Font("Times New Roman", FontStyle.Normal, 14)), 0);
            comp.Insert(new CharGlyph('e', new Font("Times New Roman", FontStyle.Normal, 14)), 1);
            comp.Insert(new CharGlyph('l', new Font("Times New Roman", FontStyle.Normal, 14)), 2);
            comp.Insert(new CharGlyph('l', new Font("Times New Roman", FontStyle.Normal, 14)), 3);
            comp.Insert(new CharGlyph('o', new Font("Times New Roman", FontStyle.Normal, 14)), 4);
            comp.Insert(new CharGlyph('w', new Font("Times New Roman", FontStyle.Normal, 14)), 5);
            comp.Insert(new CharGlyph('o', new Font("Times New Roman", FontStyle.Normal, 14)), 6);
            comp.Insert(new CharGlyph('r', new Font("Times New Roman", FontStyle.Normal, 14)), 7);
            comp.Insert(new CharGlyph('l', new Font("Times New Roman", FontStyle.Normal, 14)), 8);
            comp.Insert(new CharGlyph('d', new Font("Times New Roman", FontStyle.Normal, 14)), 9);

            ToggleItalicCommand italicCommand = new ToggleItalicCommand(null, comp, startFrom, endAt);


            Assert.True(italicCommand.CanUndo());

        }
    }
}
