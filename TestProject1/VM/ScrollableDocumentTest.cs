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
    public class ScrollableDocumentTest
    {
        [Fact]
        public void Draw_WhenNeedScrollingTrue_DrawsPageNumberAndDelegatesToInnerDocument()
        {
            var innerDoc = new Mock<Document>();
            
            var r1 = new Mock<Row>();
            var r2 = new Mock<Row>();
            r1.Setup(r => r.GetHeight()).Returns(60.0);
            r2.Setup(r => r.GetHeight()).Returns(50.0);
            var innerRows = new List<Row> { r1.Object, r2.Object };
            innerDoc.Object.SetRows(innerRows);

            var scroll = new ScrollableDocument(innerDoc.Object);

            var rowsToDraw = new List<Row> { r1.Object, r2.Object };

            var dg = new DrawingGroup();
            ViewEventArgs args;
            using (var dc = dg.Open())
            {
                args = new ViewEventArgs(dc, top: 0.0, left: 0.0, frameWidth: 200.0, frameHeight: 80.0);
                scroll.Draw(rowsToDraw, args, from: 0);
            }

            Assert.Equal(dg.Children.Count, 1);

            innerDoc.Verify(d => d.Draw(It.Is<List<Row>>(lst => ReferenceEquals(lst, rowsToDraw)), args, 0), Times.Once);
        }

        [Fact]
        public void Draw_WhenNoScrollingAndFromZero_DoesNotDrawPageNumberButDelegates()
        {
            var innerDoc = new Mock<Document>();
            var r1 = new Mock<Row>();
            r1.Setup(r => r.GetHeight()).Returns(10.0);
            var innerRows = new List<Row> { r1.Object };
            innerDoc.Object.SetRows(innerRows);

            var scroll = new ScrollableDocument(innerDoc.Object);
            var rowsToDraw = new List<Row> { r1.Object };

            var dg = new DrawingGroup();
            ViewEventArgs args;
            using (var dc = dg.Open())
            {
                args = new ViewEventArgs(dc, top: 0.0, left: 0.0, frameWidth: 200.0, frameHeight: 100.0);
                scroll.Draw(rowsToDraw, args, from: 0);
            }

            Assert.Equal(dg.Children.Count, 0);
            innerDoc.Verify(d => d.Draw(rowsToDraw, args, 0), Times.Once);
        }


        [Fact]
        public void Draw_WhenFromGreaterOrEqualCount_ResetsToZeroBeforeDelegation()
        {
            var innerDoc = new Mock<Document>();
            var r1 = new Mock<Row>();
            r1.Setup(r => r.GetHeight()).Returns(5.0);
            innerDoc.Object.SetRows(new List<Row> { r1.Object });

            var scroll = new ScrollableDocument(innerDoc.Object);
            var rowsToDraw = new List<Row> { r1.Object };

            var dg = new DrawingGroup();
            ViewEventArgs args;
            using (var dc = dg.Open())
            {
                args = new ViewEventArgs(dc, top: 0.0, left: 0.0, frameWidth: 200.0, frameHeight: 100.0);
                scroll.Draw(rowsToDraw, args, from: 5);
            }

            innerDoc.Verify(d => d.Draw(rowsToDraw, args, 0), Times.Once);
        }

        [Fact]
        public void NeedScrolling_ReturnsTrue_WhenTotalHeightExceedsFrameHeight()
        {
            var innerDoc = new Mock<Document>();
            var r1 = new Mock<Row>(); r1.Setup(r => r.GetHeight()).Returns(30.0);
            var r2 = new Mock<Row>(); r2.Setup(r => r.GetHeight()).Returns(40.0);
            innerDoc.Object.SetRows(new List<Row> { r1.Object, r2.Object });

            var scroll = new ScrollableDocument(innerDoc.Object);

            bool need = scroll.NeedScrolling(50.0);

            Assert.True(need);
        }

        [Fact]
        public void NeedScrolling_ReturnsFalse_WhenTotalHeightDoesNotExceedFrameHeight()
        {
            var innerDoc = new Mock<Document>();
            var r1 = new Mock<Row>(); r1.Setup(r => r.GetHeight()).Returns(10.0);
            var r2 = new Mock<Row>(); r2.Setup(r => r.GetHeight()).Returns(12.0);
            innerDoc.Object.SetRows(new List<Row> { r1.Object, r2.Object });

            var scroll = new ScrollableDocument(innerDoc.Object);

            bool need = scroll.NeedScrolling(50.0);

            Assert.False(need);
        }
    }
}
