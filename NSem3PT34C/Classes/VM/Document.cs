using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSem3PT34C.Classes.VM
{
    public abstract class Document
    {
        private List<Row> rows;

        public Document()
        {
            this.rows = new List<Row>();
        }

        public Document(List<Row> rows)
        {
            this.rows = rows;
        }

        public List<Row> GetRows()
        {
            return this.rows;
        }

        public void SetRows(List<Row> rows)
        {
            this.rows = rows;
        }

        public abstract void Draw(List<Row> rows, ViewEventArgs args, int from);

        public abstract bool NeedScrolling(double frameHeight);
    }
}
