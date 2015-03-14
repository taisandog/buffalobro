using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;

namespace Buffalo.WinFormsControl.DataGridViewEx
{
    public class DataGridViewExButtonColumn : DataGridViewButtonColumn
    {
        public DataGridViewExButtonColumn()
        {
            this.CellTemplate = new DataGridViewExButtonCell();
        }
    }
}
