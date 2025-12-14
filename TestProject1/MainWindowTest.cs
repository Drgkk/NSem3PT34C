using Moq;
using NSem3PT34;
using NSem3PT34.Classes;
using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Util;
using NSem3PT34.Classes.VM;
using NSem3PT34C.Classes.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace TestProject1
{
    public class MainWindowTest
    {
        private void SetPrivateField(object target, string fieldName, object value)
        {
            var fi = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi == null) throw new InvalidOperationException($"Field '{fieldName}' not found.");
            fi.SetValue(target, value);
        }

        [Fact]
        public void HandleDrawing_CallsDocumentDraw()
        {
            var mw = (MainWindow)FormatterServices.GetUninitializedObject(typeof(MainWindow));

            var docMock = new Mock<ConcreteDocument>();
            var glyphMock1 = new Mock<Glyph>();
            var glyphMock2 = new Mock<Glyph>();
            glyphMock1.Setup(g => g.GetWidth()).Returns(10.0);
            glyphMock2.Setup(g => g.GetWidth()).Returns(8.0);
            var row = new Row();
            row.GetUiGlyphs().Add(new UiGlyph(glyphMock1.Object, new Point(0, 0), 0));
            row.GetUiGlyphs().Add(new UiGlyph(glyphMock2.Object, new Point(0, 0), 1));

            var rows = new List<Row>() { row };
            docMock.Object.SetRows(rows);
            docMock.Setup(d => d.Draw(It.IsAny<List<Row>>(), It.IsAny<ViewEventArgs>(), It.IsAny<int>()));

            SetPrivateField(mw, "document", docMock.Object);
            SetPrivateField(mw, "index", 0); 

            var dg = new DrawingGroup();
            var dc = dg.Open();
            var args = new ViewEventArgs(dc, 4, 4, 100, 200);

            typeof(MainWindow).GetMethod("HandleDrawing", BindingFlags.Instance | BindingFlags.Public)
                ?.Invoke(mw, new object[] { rows, args });

            docMock.Verify(d => d.Draw(rows, args, 0), Times.Once);

            dc.Close();
        }

        [Fact]
        public void HandleDrawing_WithSelectionRange_CallsRowSelects()
        {
            var mw = (MainWindow)FormatterServices.GetUninitializedObject(typeof(MainWindow));

            var docMock = new Mock<ConcreteDocument>();
            var glyphMock1 = new Mock<Glyph>();
            var glyphMock2 = new Mock<Glyph>();
            glyphMock1.Setup(g => g.GetWidth()).Returns(10.0);
            glyphMock2.Setup(g => g.GetWidth()).Returns(8.0);
            glyphMock1.Setup(g => g.Select(It.IsAny<DrawingContext>(), It.IsAny<SolidColorBrush>(), It.IsAny<SolidColorBrush>(), It.IsAny<double>(), It.IsAny<double>()));
            glyphMock2.Setup(g => g.Select(It.IsAny<DrawingContext>(), It.IsAny<SolidColorBrush>(), It.IsAny<SolidColorBrush>(), It.IsAny<double>(), It.IsAny<double>()));

            var row = new Row();
            row.GetUiGlyphs().Add(new UiGlyph(glyphMock1.Object, new Point(0, 0), 0));
            row.GetUiGlyphs().Add(new UiGlyph(glyphMock2.Object, new Point(0, 0), 1));

            var rows = new List<Row>() { row };
            docMock.Setup(d => d.Draw(It.IsAny<List<Row>>(), It.IsAny<ViewEventArgs>(), It.IsAny<int>()));
            docMock.Object.SetRows(rows);

            SetPrivateField(mw, "document", docMock.Object);
            SetPrivateField(mw, "index", 0);

            var sel = new SelectionRange();
            sel.SetStartRow(0);
            sel.SetEndRow(0);
            sel.SetStartCol(0);
            sel.SetEndCol(1);
            SetPrivateField(mw, "selectionRange", sel);

            var dg = new DrawingGroup();
            var dc = dg.Open();
            SetPrivateField(mw, "graphics", dc);

            var args = new ViewEventArgs(dc, 4, 4, 100, 200);

            typeof(MainWindow).GetMethod("HandleDrawing", BindingFlags.Instance | BindingFlags.Public)
                ?.Invoke(mw, new object[] { rows, args });

            glyphMock1.Verify(g => g.Select(It.IsAny<DrawingContext>(), It.IsAny<SolidColorBrush>(), It.IsAny<SolidColorBrush>(), It.IsAny<double>(), It.IsAny<double>()), Times.AtLeastOnce);
            glyphMock2.Verify(g => g.Select(It.IsAny<DrawingContext>(), It.IsAny<SolidColorBrush>(), It.IsAny<SolidColorBrush>(), It.IsAny<double>(), It.IsAny<double>()), Times.AtLeastOnce);

            dc.Close();
        }

        

        [Fact]
        public void HandleSpellingError_InvokesGlyphSelectUsingInternalGraphics()
        {
            var mw = (MainWindow)FormatterServices.GetUninitializedObject(typeof(MainWindow));

            var glyphMock = new Mock<Glyph>();
            glyphMock.Setup(g => g.Bounds()).Returns(new Rect(0, 0, 5.0, 3.0));
            glyphMock.Setup(g => g.Select(It.IsAny<DrawingContext>(), It.IsAny<SolidColorBrush>(), It.IsAny<SolidColorBrush>(), It.IsAny<double>(), It.IsAny<double>()));

            var ui = new UiGlyph(glyphMock.Object, new Point(10, 20), 42);
            var row = new Row();
            var rowMock = new Mock<Row>();
            rowMock.Setup(r => r.Bounds()).Returns(new Rect(0, 5, 100, 30));

            var dict = new Dictionary<UiGlyph, Row>();
            dict.Add(ui, rowMock.Object);

            var dg = new DrawingGroup();
            var dc = dg.Open();
            SetPrivateField(mw, "graphics", dc);

            typeof(MainWindow).GetMethod("HandleSpellingError", BindingFlags.Instance | BindingFlags.Public)
                ?.Invoke(mw, new object[] { dict });

            glyphMock.Verify(g => g.Select(
                It.Is<DrawingContext>(d => d == dc),
                It.IsAny<SolidColorBrush>(),
                It.IsAny<SolidColorBrush>(),
                It.Is<double>(x => Math.Abs(x - ui.GetPosition().X) < 1e-6),
                It.IsAny<double>()),
                Times.Once);

            dc.Close();
        }

        [Fact]
        public void GetStartFromAndGetEndAt_ReturnPhysicalIndices()
        {
            var mw = (MainWindow)FormatterServices.GetUninitializedObject(typeof(MainWindow));
            var docMock = new Mock<ConcreteDocument>();

            var row = new Row();
            var g1 = new UiGlyph(Mock.Of<Glyph>(), new Point(0, 0), 11);
            var g2 = new UiGlyph(Mock.Of<Glyph>(), new Point(0, 0), 22);
            row.GetUiGlyphs().Add(g1);
            row.GetUiGlyphs().Add(g2);

            docMock.Object.SetRows(new List<Row> { row });
            SetPrivateField(mw, "document", docMock.Object);

            var sel = new SelectionRange();
            sel.SetStartRow(0); sel.SetStartCol(0);
            sel.SetEndRow(0); sel.SetEndCol(1);
            SetPrivateField(mw, "selectionRange", sel);

            var start = (int)typeof(MainWindow).GetMethod("GetStartFrom", BindingFlags.Instance | BindingFlags.Public)
                ?.Invoke(mw, null);
            var end = (int)typeof(MainWindow).GetMethod("GetEndAt", BindingFlags.Instance | BindingFlags.Public)
                ?.Invoke(mw, null);

            Assert.Equal(11, start);
            Assert.Equal(22, end);
        }

        [Fact]
        public void IsPointGreater_WorksForVariousPairs()
        {
            Assert.Equal(-1, MainWindow.IsPointGreater(new Point(0, 0), new Point(1, 0)));
            Assert.Equal(1, MainWindow.IsPointGreater(new Point(2, 1), new Point(1, 9)));
            Assert.Equal(-1, MainWindow.IsPointGreater(new Point(1, 1), new Point(1, 2)));
            Assert.Equal(0, MainWindow.IsPointGreater(new Point(1, 1), new Point(1, 1)));
        }

        [Fact]
        public void GetCharFromKey_ReturnsSpaceForSpaceKey()
        {
            char c = MainWindow.GetCharFromKey(Key.Space);
            Assert.Equal(' ', c);
        }
    }
}
