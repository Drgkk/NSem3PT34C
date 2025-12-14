using NSem3PT34.Classes.Structure;
using NSem3PT34C.Classes.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSem3PT34.Classes;
using NSem3PT34.Classes.Command;
using NSem3PT34.Classes.Util;

namespace TestProject1.CommandTests
{
    public class DeleteCommandTest
    {
        [Fact]
        public void Execute_CorrectParameters_DeletesGlyphs()
        {
            int startFrom = 3;
            int endAt = 5;
            Composition comp = new Composition();
            List<Glyph> children = new List<Glyph>([
                new CharGlyph('h', new Font("Times New Roman", FontStyle.Normal, 14)),
                new CharGlyph('e', new Font("Times New Roman", FontStyle.Normal, 14)),
                new CharGlyph('l', new Font("Times New Roman", FontStyle.Normal, 14)),
                new CharGlyph('o', new Font("Times New Roman", FontStyle.Normal, 14)),
                new CharGlyph('r', new Font("Times New Roman", FontStyle.Normal, 14)),
                new CharGlyph('l', new Font("Times New Roman", FontStyle.Normal, 14)),
                new CharGlyph('d', new Font("Times New Roman", FontStyle.Normal, 14))
            ]);
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

            DeleteCommand deleteCommand = new DeleteCommand( comp, startFrom, endAt);


            deleteCommand.Execute();
            List<Glyph> changedChildren = comp.GetChildren();

            for (int i = 0; i < children.Count; i++)
            {
                Assert.Equal(changedChildren[i], children[i]);
            }
        }

        [Fact]
        public void Constructor_OutOfRange_ThrowsException()
        {
            int startFrom = 3;
            int endAt = 12;
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

            var act = () => new DeleteCommand(comp, startFrom, endAt);
            Assert.Throws<ArgumentOutOfRangeException>(act);

        }

        [Fact]
        public void Unexecute_CorrectParameters_ReturnsDeletedGlyphs()
        {
            int startFrom = 3;
            int endAt = 5;
            Composition comp = new Composition();
            List<Glyph> children = new List<Glyph>([
                new CharGlyph('h', new Font("Times New Roman", FontStyle.Normal, 14)),
                new CharGlyph('e', new Font("Times New Roman", FontStyle.Normal, 14)),
                new CharGlyph('l', new Font("Times New Roman", FontStyle.Normal, 14)),
                new CharGlyph('l', new Font("Times New Roman", FontStyle.Normal, 14)),
                new CharGlyph('o', new Font("Times New Roman", FontStyle.Normal, 14)),
                new CharGlyph('w', new Font("Times New Roman", FontStyle.Normal, 14)),
                new CharGlyph('o', new Font("Times New Roman", FontStyle.Normal, 14)),
                new CharGlyph('r', new Font("Times New Roman", FontStyle.Normal, 14)),
                new CharGlyph('l', new Font("Times New Roman", FontStyle.Normal, 14)),
                new CharGlyph('d', new Font("Times New Roman", FontStyle.Normal, 14))
            ]);
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

            DeleteCommand deleteCommand = new DeleteCommand(comp, startFrom, endAt);


            deleteCommand.Execute();
            deleteCommand.UnExecute();
            List<Glyph> changedChildren = comp.GetChildren();

            for (int i = 0; i < children.Count; i++)
            {
                Assert.Equal(changedChildren[i], children[i]);
            }

        }

        [Fact]
        public void CanUndo_Always_ReturnsTrue()
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

            DeleteCommand deleteCommand = new DeleteCommand(comp, startFrom, endAt);


            Assert.True(deleteCommand.CanUndo());

        }
    }
}
