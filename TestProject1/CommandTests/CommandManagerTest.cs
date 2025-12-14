using Moq;
using NSem3PT34.Classes.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.CommandTests
{
    public class CommandManagerTest
    {
        public CommandManagerTest()
        {
            ResetCommandManagerSingleton();
        }

        private void ResetCommandManagerSingleton()
        {
            var t = typeof(CommandManager);
            var field = t.GetField("instance", BindingFlags.Static | BindingFlags.NonPublic);
            if (field == null) throw new InvalidOperationException("Field 'instance' not found. Prove Namespace/Classname.");
            field.SetValue(null, null);
        }

        [Fact]
        public void Execute_WhenExecuteAndCanUndoTrue_AddsCommandAndReturnsTrue()
        {
            var manager = CommandManager.GetInstance();

            var cmdMock = new Mock<ICommand>();
            cmdMock.Setup(c => c.Execute()).Returns(true);
            cmdMock.Setup(c => c.CanUndo()).Returns(true);

            var result = manager.Execute(cmdMock.Object);

            Assert.True(result);
            Assert.True(manager.CanUndo());
            Assert.False(manager.CanRedo());

            cmdMock.Verify(c => c.Execute(), Times.Once);
            cmdMock.Verify(c => c.CanUndo(), Times.Once);
        }

        [Fact]
        public void Execute_WhenExecuteFalseOrCanUndoFalse_DoesNotAddAndReturnsFalse()
        {
            var manager = CommandManager.GetInstance();

            var failExecute = new Mock<ICommand>();
            failExecute.Setup(c => c.Execute()).Returns(false);
            failExecute.Setup(c => c.CanUndo()).Returns(true);

            var failCanUndo = new Mock<ICommand>();
            failCanUndo.Setup(c => c.Execute()).Returns(true);
            failCanUndo.Setup(c => c.CanUndo()).Returns(false);

            Assert.False(manager.Execute(failExecute.Object));
            Assert.False(manager.CanUndo());

            Assert.False(manager.Execute(failCanUndo.Object));
            Assert.False(manager.CanUndo());

            failExecute.Verify(c => c.Execute(), Times.Once);
            failCanUndo.Verify(c => c.Execute(), Times.Once);
            failCanUndo.Verify(c => c.CanUndo(), Times.Once);
        }

        [Fact]
        public void Undo_WhenCanUndo_CallsUnExecuteAndDecrementsCurrent()
        {
            var manager = CommandManager.GetInstance();

            var cmdMock = new Mock<ICommand>();
            cmdMock.Setup(c => c.Execute()).Returns(true);
            cmdMock.Setup(c => c.CanUndo()).Returns(true);
            
            manager.Execute(cmdMock.Object);

            Assert.True(manager.CanUndo());

            manager.Undo();

            cmdMock.Verify(c => c.UnExecute(), Times.Once);
            Assert.False(manager.CanUndo());
        }

        [Fact]
        public void Redo_WhenCanRedo_CallsExecuteAgainOnCommand()
        {
            var manager = CommandManager.GetInstance();

            var cmd1 = new Mock<ICommand>();
            cmd1.Setup(c => c.Execute()).Returns(true);
            cmd1.Setup(c => c.CanUndo()).Returns(true);

            var cmd2 = new Mock<ICommand>();
            cmd2.Setup(c => c.Execute()).Returns(true);
            cmd2.Setup(c => c.CanUndo()).Returns(true);

            manager.Execute(cmd1.Object);
            manager.Execute(cmd2.Object);

            manager.Undo();
            Assert.True(manager.CanRedo());

            manager.Redo();

            cmd2.Verify(c => c.Execute(), Times.Exactly(2));
        }

        [Fact]
        public void Execute_AfterUndo_ClearsRedoStack()
        {
            var manager = CommandManager.GetInstance();

            var cmd1 = new Mock<ICommand>();
            cmd1.Setup(c => c.Execute()).Returns(true);
            cmd1.Setup(c => c.CanUndo()).Returns(true);

            var cmd2 = new Mock<ICommand>();
            cmd2.Setup(c => c.Execute()).Returns(true);
            cmd2.Setup(c => c.CanUndo()).Returns(true);

            var cmd3 = new Mock<ICommand>();
            cmd3.Setup(c => c.Execute()).Returns(true);
            cmd3.Setup(c => c.CanUndo()).Returns(true);

            manager.Execute(cmd1.Object);
            manager.Execute(cmd2.Object);

            manager.Undo();
            Assert.True(manager.CanRedo());

            manager.Execute(cmd3.Object);

            Assert.False(manager.CanRedo());

            cmd2.Verify(c => c.Execute(), Times.Once);
        }

        [Fact]
        public void GetInstance_ReturnsSameInstance()
        {
            ResetCommandManagerSingleton();
            var a = CommandManager.GetInstance();
            var b = CommandManager.GetInstance();
            Assert.Same(a, b);
        }
    }
}
