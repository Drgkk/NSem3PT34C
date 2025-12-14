using NSem3PT34.Classes.Structure;
using NSem3PT34C.Classes.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSem3PT34.Classes.Util;

namespace TestProject1.CommandTests
{
    public class ChangeFontSizeCommandTest
    {
        [Fact]
        public void Execute_CorrectParameters_ChangesFontSizes()
        {
            int startFrom = 1;
            int endAt = 5;
            int changeTo = 25;
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

            ChangeFontSizeCommand changeFontSizeCmd = new ChangeFontSizeCommand(null, comp, startFrom, endAt, changeTo);


            changeFontSizeCmd.Execute();

            for (int i = startFrom; i <= endAt; i++)
            {
                var font = comp.GetChildren()[i].GetFont().Value;

                Assert.Equal(changeTo, font.Size);
                Assert.Equal("Times New Roman", font.Name);
            }
        }

        [Fact]
        public void Constructor_OutOfRange_ThrowsException()
        {
            int startFrom = 1;
            int endAt = 12;
            int changeTo = 25;
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

            var act = () => new ChangeFontSizeCommand(null, comp, startFrom, endAt, changeTo);
            Assert.Throws<ArgumentOutOfRangeException>(act);

        }

        [Fact]
        public void Unexecute_CorrectParameters_ReturnsFontNames()
        {
            int startFrom = 1;
            int endAt = 5;
            int changeTo = 25;
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

            ChangeFontSizeCommand changeFontSizeCmd = new ChangeFontSizeCommand(null, comp, startFrom, endAt, changeTo);


            changeFontSizeCmd.Execute();
            changeFontSizeCmd.UnExecute();
            for (int i = startFrom; i <= endAt; i++)
            {
                var font = comp.GetChildren()[i].GetFont().Value;
                Assert.Equal(14, font.Size);
                Assert.Equal("Times New Roman", font.Name);
            }

        }

        [Fact]
        public void CanUndo_Always_ReturnsTrue()
        {
            int startFrom = 1;
            int endAt = 5;
            string changeTo = "Arial";
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

            ChangeFontCommand changeFontCmd = new ChangeFontCommand(null, comp, startFrom, endAt, changeTo);


            Assert.True(changeFontCmd.CanUndo());

        }
    }
}
