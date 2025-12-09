using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Util;

namespace NSem3PT34C.Classes.VM
{
    public class ConcreteDocument : Document
    {
        public override void Draw(List<Row> rows, ViewEventArgs args, int from)
        {
            this.SetRows(rows);
            if (this.GetRows() != null && this.GetRows().Count > 0)
            {
                double currentTop = args.GetTop();
                for (int i = from; i < this.GetRows().Count; i++)
                {
                    Row row = this.GetRows()[i];
                    row.Draw(args.GetGraphics(), args.GetLeft(), currentTop);
                    currentTop += row.GetHeight();
                }
            }
        }

        public override bool NeedScrolling(double args)
        {
            return false;
        }
    }
}
