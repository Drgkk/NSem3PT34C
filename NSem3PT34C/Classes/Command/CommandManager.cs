using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSem3PT34.Classes.Command
{
    public class CommandManager
    {
        private static CommandManager instance;
        private List<ICommand> commands = new List<ICommand>();
        private int current = -1;

        private CommandManager() { }

        private static readonly object _lock = new object();
        public static CommandManager GetInstance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new CommandManager();
                    }
                }
            }
		
            return instance;
        }
	
        public bool Execute(ICommand cmd)
        {
            bool val = cmd.Execute() && cmd.CanUndo();
            if (val)
            {
                int size = this.commands.Count;
                for (int i = size - 1; i >= current + 1; i--)
                {
                    this.commands.RemoveAt(i);
                }

                this.commands.Add(cmd);
                this.current++;
            }

            return val;
        }

        public void Undo()
        {
            if (this.CanUndo())
            {
                this.commands[current].UnExecute();
                current--;
            }
        }

        public void Redo()
        {
            if (this.CanRedo())
            {
                current++;
                this.commands[current].Execute();
            }
        }

        public bool CanUndo()
        {
            return this.current > -1;
        }

        public bool CanRedo()
        {
            return this.current < (this.commands.Count - 1);
        }
    }
}
