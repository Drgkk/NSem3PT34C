using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using NSem3PT34;

namespace TestProject1
{
    public class MyCanvasTest
    {
        private static void RunInSta(Action action)
        {
            Exception? ex = null;
            var thread = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    ex = e;
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            if (ex != null)
            {
                throw new Exception("Error in STA-Thread", ex);
            }
        }

        [Fact]
        public void RenderStart_AddsDrawingVisualAndReturnsDrawingContext()
        {
            RunInSta(() =>
            {
                var canvas = new MyCanvas();

                DrawingContext dc = canvas.RenderStart();

                Assert.NotNull(dc);
                Assert.Equal(1, canvas._children.Count);

                canvas.RenderStop(dc);
            });
        }

        [Fact]
        public void RenderStart_ClearsPreviousChildrenBeforeAddingNewOne()
        {
            RunInSta(() =>
            {
                var canvas = new MyCanvas();

                var dummy = new DrawingVisual();
                canvas._children.Add(dummy);
                Assert.Equal(1, canvas._children.Count);

                var dc = canvas.RenderStart();
                Assert.Equal(1, canvas._children.Count);
                Assert.IsType<DrawingVisual>(canvas._children[0]);
                Assert.NotSame(dummy, canvas._children[0]);

                canvas.RenderStop(dc);
            });
        }

        [Fact]
        public void RenderStop_ClosesDrawingContext_NoException()
        {
            RunInSta(() =>
            {
                var canvas = new MyCanvas();
                var dc = canvas.RenderStart();

                var ex = Record.Exception(() => canvas.RenderStop(dc));
                Assert.Null(ex);
            });
        }

    }
}
