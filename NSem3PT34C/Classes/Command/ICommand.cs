using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSem3PT34.Classes.Command
{
    public interface ICommand
    {
        bool Execute();

        void UnExecute();

        bool CanUndo();

        String ToString();
    }
}
