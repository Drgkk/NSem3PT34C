using Moq;
using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Util;
using NSem3PT34.Classes.Visitor;
using NSem3PT34.Classes.VM;
using NSem3PT34C.Classes.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestProject1.Visitor
{
    public class SpellingCheckingVisitorTest
    {
        private Font DefaultFont() =>
            new Font("Times New Roman", NSem3PT34.Classes.Util.FontStyle.Normal, 14);

        private void ResetSpellCheckerSingleton()
        {
            var field = typeof(SpellChecker)
                .GetField("instance", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, null);
        }

        private void LoadDictionary(params string[] words)
        {
            ResetSpellCheckerSingleton();
            var checker = SpellChecker.GetInstance();

            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, string.Join("\n", words), Encoding.UTF8);
            checker.LoadDictionary(tempFile);
            File.Delete(tempFile);
        }

        [Fact]
        public void VisitRow_WithCorrectWord_DoesNotInvokeErrorHandler()
        {
            LoadDictionary("hello");

            var handlerMock = new Mock<ISpellingErrorHandler>();
            var visitor = new SpellingCheckingVisitor(handlerMock.Object);

            var row = new Row();
            row.GetUiGlyphs().Add(new UiGlyph(new CharGlyph('h', DefaultFont()), new Point(0, 0), 0));
            row.GetUiGlyphs().Add(new UiGlyph(new CharGlyph('e', DefaultFont()), new Point(10, 0), 1));
            row.GetUiGlyphs().Add(new UiGlyph(new CharGlyph('l', DefaultFont()), new Point(20, 0), 2));
            row.GetUiGlyphs().Add(new UiGlyph(new CharGlyph('l', DefaultFont()), new Point(30, 0), 3));
            row.GetUiGlyphs().Add(new UiGlyph(new CharGlyph('o', DefaultFont()), new Point(40, 0), 4));

            visitor.Visit(row);
            visitor.Visit(new BreakGlyph(DefaultFont()));

            handlerMock.Verify(
                h => h.HandleSpellingError(It.IsAny<Dictionary<UiGlyph, Row>>()),
                Times.Never);
        }

        [Fact]
        public void VisitRow_WithMisspelledWord_InvokesErrorHandlerOnce()
        {
            LoadDictionary("hello");

            var handlerMock = new Mock<ISpellingErrorHandler>();
            var visitor = new SpellingCheckingVisitor(handlerMock.Object);

            var row = new Row();
            var uiGlyphs = new List<UiGlyph>
            {
                new UiGlyph(new CharGlyph('h', DefaultFont()), new Point(0, 0), 0),
                new UiGlyph(new CharGlyph('x', DefaultFont()), new Point(10, 0), 1),
                new UiGlyph(new CharGlyph('l', DefaultFont()), new Point(20, 0), 2),
                new UiGlyph(new CharGlyph('l', DefaultFont()), new Point(30, 0), 3),
                new UiGlyph(new CharGlyph('o', DefaultFont()), new Point(40, 0), 4)
            };

            foreach (var ui in uiGlyphs)
                row.GetUiGlyphs().Add(ui);

            visitor.Visit(row);
            visitor.Visit(new BreakGlyph(DefaultFont()));

            handlerMock.Verify(
                h => h.HandleSpellingError(It.IsAny<Dictionary<UiGlyph, Row>>()),
                Times.Once);
        }

        [Fact]
        public void ErrorHandler_ReceivesCorrectUiGlyphsForMisspelledWord()
        {
            LoadDictionary("cat");

            var handlerMock = new Mock<ISpellingErrorHandler>();
            Dictionary<UiGlyph, Row> received = null;

            handlerMock
                .Setup(h => h.HandleSpellingError(It.IsAny<Dictionary<UiGlyph, Row>>()))
                .Callback<Dictionary<UiGlyph, Row>>(d => received = new Dictionary<UiGlyph, Row>(d));

            var visitor = new SpellingCheckingVisitor(handlerMock.Object);
            var row = new Row();

            var ui1 = new UiGlyph(new CharGlyph('d', DefaultFont()), new Point(0, 0), 0);
            var ui2 = new UiGlyph(new CharGlyph('o', DefaultFont()), new Point(10, 0), 1);
            var ui3 = new UiGlyph(new CharGlyph('g', DefaultFont()), new Point(20, 0), 2);

            row.GetUiGlyphs().Add(ui1);
            row.GetUiGlyphs().Add(ui2);
            row.GetUiGlyphs().Add(ui3);
            row.GetUiGlyphs().Add(new UiGlyph(new BreakGlyph(DefaultFont()), new Point(30, 0), 3));

            visitor.Visit(row);

            Assert.NotNull(received);
            Assert.Equal(3, received.Count);
            Assert.All(received.Values, r => Assert.Same(row, r));
            Assert.Contains(ui1, received.Keys);
            Assert.Contains(ui2, received.Keys);
            Assert.Contains(ui3, received.Keys);
        }

        [Fact]
        public void NonLetterCharacter_TriggersSpellCheckAndClearsState()
        {
            LoadDictionary("test");

            var handlerMock = new Mock<ISpellingErrorHandler>();
            var visitor = new SpellingCheckingVisitor(handlerMock.Object);

            var row = new Row();
            row.GetUiGlyphs().Add(new UiGlyph(new CharGlyph('t', DefaultFont()), new Point(0, 0), 0));
            row.GetUiGlyphs().Add(new UiGlyph(new CharGlyph('e', DefaultFont()), new Point(0, 0), 1));
            row.GetUiGlyphs().Add(new UiGlyph(new CharGlyph('x', DefaultFont()), new Point(0, 0), 2));
            row.GetUiGlyphs().Add(new UiGlyph(new CharGlyph(' ', DefaultFont()), new Point(0, 0), 3));

            visitor.Visit(row);

            handlerMock.Verify(
                h => h.HandleSpellingError(It.IsAny<Dictionary<UiGlyph, Row>>()),
                Times.Once);
        }

        [Fact]
        public void Visit_BreakGlyph_TriggersSpellCheck()
        {
            LoadDictionary("ok");

            var handlerMock = new Mock<ISpellingErrorHandler>();
            var visitor = new SpellingCheckingVisitor(handlerMock.Object);

            var row = new Row();
            row.GetUiGlyphs().Add(new UiGlyph(new CharGlyph('n', DefaultFont()), new Point(0, 0), 0));
            row.GetUiGlyphs().Add(new UiGlyph(new CharGlyph('o', DefaultFont()), new Point(0, 0), 1));
            row.GetUiGlyphs().Add(new UiGlyph(new BreakGlyph(DefaultFont()), new Point(0, 0), 2));

            visitor.Visit(row);

            handlerMock.Verify(
                h => h.HandleSpellingError(It.IsAny<Dictionary<UiGlyph, Row>>()),
                Times.Once);
        }

        [Fact]
        public void NoHandler_DoesNotThrowOnMisspelledWord()
        {
            LoadDictionary("yes");

            var visitor = new SpellingCheckingVisitor();

            var row = new Row();
            row.GetUiGlyphs().Add(new UiGlyph(new CharGlyph('n', DefaultFont()), new Point(0, 0), 0));
            row.GetUiGlyphs().Add(new UiGlyph(new CharGlyph('o', DefaultFont()), new Point(0, 0), 1));
            row.GetUiGlyphs().Add(new UiGlyph(new BreakGlyph(DefaultFont()), new Point(0, 0), 2));

            var exception = Record.Exception(() =>
            {
                visitor.Visit(row);
            });

            Assert.Null(exception);
        }
    }
}
