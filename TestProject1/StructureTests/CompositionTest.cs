using Moq;
using NSem3PT34.Classes;
using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.StructureTests
{
    public class CompositionTest
    {
        private Font MakeFont(string name = "Times New Roman", FontStyle style = FontStyle.Normal, int size = 14)
            => new Font(name, style, size);

        [Fact]
        public void Insert_InsertsAtIndex_And_GetChildrenReflectsOrder()
        {
            var comp = new Composition();
            var g1 = new Mock<Glyph>().Object;
            var g2 = new Mock<Glyph>().Object;
            var g3 = new Mock<Glyph>().Object;

            comp.Insert(g1, 0);
            comp.Insert(g2, 1);
            comp.Insert(g3, 1);

            var kids = comp.GetChildren();
            Assert.Equal(3, kids.Count);
            Assert.Same(g1, kids[0]);
            Assert.Same(g3, kids[1]);
            Assert.Same(g2, kids[2]);
        }

        [Fact]
        public void Remove_RemovesByIndex()
        {
            var comp = new Composition();
            var m1 = new Mock<Glyph>().Object;
            var m2 = new Mock<Glyph>().Object;
            var m3 = new Mock<Glyph>().Object;

            comp.Insert(m1, 0);
            comp.Insert(m2, 1);
            comp.Insert(m3, 2);

            comp.Remove(1);

            var kids = comp.GetChildren();
            Assert.Equal(2, kids.Count);
            Assert.Same(m1, kids[0]);
            Assert.Same(m3, kids[1]);
        }

        [Fact]
        public void UpdateFont_SetsFontsOnRange_And_NotifiesObservers()
        {
            var comp = new Composition();

            var mg0 = new Mock<Glyph>();
            var mg1 = new Mock<Glyph>();
            var mg2 = new Mock<Glyph>();

            comp.Insert(mg0.Object, 0);
            comp.Insert(mg1.Object, 1);
            comp.Insert(mg2.Object, 2);

            var fonts = new List<Font> {
                MakeFont(size:10),
                MakeFont(size:12),
                MakeFont(size:14)
            };

            var observerMock = new Mock<IObserver>();
            comp.RegisterObserver(observerMock.Object);

            comp.UpdateFont(fonts, 0, 2);

            mg0.Verify(g => g.SetFont(It.Is<Font>(f => f.Equals(fonts[0]))), Times.Once);
            mg1.Verify(g => g.SetFont(It.Is<Font>(f => f.Equals(fonts[1]))), Times.Once);
            mg2.Verify(g => g.SetFont(It.Is<Font>(f => f.Equals(fonts[2]))), Times.Once);

            observerMock.Verify(o => o.UpdateObserver(), Times.Once);
        }

        [Fact]
        public void RegisterAndNotifyObservers_CallUpdateObserver_OnAll()
        {
            var comp = new Composition();
            var o1 = new Mock<IObserver>();
            var o2 = new Mock<IObserver>();

            comp.RegisterObserver(o1.Object);
            comp.RegisterObserver(o2.Object);

            comp.NotifyObservers();

            o1.Verify(o => o.UpdateObserver(), Times.Once);
            o2.Verify(o => o.UpdateObserver(), Times.Once);
        }

        [Fact]
        public void RemoveObserver_RemovesOnlyThatObserver()
        {
            var comp = new Composition();
            var o1 = new Mock<IObserver>();
            var o2 = new Mock<IObserver>();

            comp.RegisterObserver(o1.Object);
            comp.RegisterObserver(o2.Object);

            comp.RemoveObserver(o1.Object);

            comp.ModelChanged();

            o1.Verify(o => o.UpdateObserver(), Times.Never);
            o2.Verify(o => o.UpdateObserver(), Times.Once);
        }

        [Fact]
        public void RemoveObserver_DoesNotThrow_WhenObserverNotRegistered()
        {
            var comp = new Composition();
            var o1 = new Mock<IObserver>();
            comp.RemoveObserver(o1.Object);
            comp.NotifyObservers();
            o1.Verify(o => o.UpdateObserver(), Times.Never);
        }
    }
}
