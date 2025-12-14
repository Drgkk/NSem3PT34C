using Moq;
using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Util;
using NSem3PT34C.Classes.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TestProject1.VM
{
    public class ConcreteDocumentTest
    {
        [Fact]
        public void Draw_StartsFromZero_SetsRowsAndCallsDrawOnAllRowsFromZero()
        {
            var doc = new ConcreteDocument();

            var row1 = new Mock<Row>();
            var row2 = new Mock<Row>();

            row1.Setup(r => r.GetHeight()).Returns(10.0);
            row2.Setup(r => r.GetHeight()).Returns(15.0);

            var rows = new List<Row> { row1.Object, row2.Object };

            var dg = new DrawingGroup();
            ViewEventArgs args;
            using (var dc = dg.Open())
            {
                args = new ViewEventArgs(dc, 100.0, 20.0, 400.0, 100.0);
                doc.Draw(rows, args, 0);
            }

            Assert.Same(rows, doc.GetRows());

            row1.Verify(r => r.Draw(It.IsAny<DrawingContext>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
            row2.Verify(r => r.Draw(It.IsAny<DrawingContext>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
        }

        [Fact]
        public void Draw_StartsFromGivenIndex_OnlyCallsLaterRows()
        {
            var doc = new ConcreteDocument();

            var row0 = new Mock<Row>();
            var row1 = new Mock<Row>();
            var row2 = new Mock<Row>();

            row0.Setup(r => r.GetHeight()).Returns(5.0);
            row1.Setup(r => r.GetHeight()).Returns(7.0);
            row2.Setup(r => r.GetHeight()).Returns(9.0);

            var rows = new List<Row> { row0.Object, row1.Object, row2.Object };

            var dg = new DrawingGroup();
            ViewEventArgs args;

            using (var dc = dg.Open())
            {
                args = new ViewEventArgs(dc, 100.0, 20.0, 400.0, 100.0);
                doc.Draw(rows, args, 1);
            }

            row0.Verify(r => r.Draw(It.IsAny<DrawingContext>(), It.IsAny<double>(), It.IsAny<double>()), Times.Never);
            row1.Verify(r => r.Draw(It.IsAny<DrawingContext>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
            row2.Verify(r => r.Draw(It.IsAny<DrawingContext>(), It.IsAny<double>(), It.IsAny<double>()), Times.Once);
        }

        [Fact]
        public void Draw_WithEmptyRows_DoesNotCallAnyRowDraw()
        {
            var doc = new ConcreteDocument();
            var rows = new List<Row>(); 

            var dg = new DrawingGroup();
            ViewEventArgs args;

            using (var dc = dg.Open())
            {
                args = new ViewEventArgs(dc, 100.0, 20.0, 400.0, 100.0);
                doc.Draw(rows, args, 0);
            }

            Assert.Same(rows, doc.GetRows());
        }

        [Fact]
        public void Draw_WithFromEqualToCount_DoesNotCallAnyRowDraw()
        {
            var doc = new ConcreteDocument();

            var row0 = new Mock<Row>();
            var rows = new List<Row> { row0.Object };

            var dg = new DrawingGroup();
            ViewEventArgs args;

            using (var dc = dg.Open())
            {
                args = new ViewEventArgs(dc, 100.0, 20.0, 400.0, 100.0);
                doc.Draw(rows, args, rows.Count);
            }

            row0.Verify(r => r.Draw(It.IsAny<DrawingContext>(), It.IsAny<double>(), It.IsAny<double>()), Times.Never);
        }

        [Fact]
        public void NeedScrolling_Always_ReturnsFalse()
        {
            var doc = new ConcreteDocument();
            Assert.False(doc.NeedScrolling(100.0));
            Assert.False(doc.NeedScrolling(0.0));
        }
    }
}
