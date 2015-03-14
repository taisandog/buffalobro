using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;

namespace Buffalo.WinFormsControl.DataGridViewEx
{
    public class DataGridViewExCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        private bool _enabledValue=true;
        /// <summary>
        /// 可用
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _enabledValue;
            }
            set
            {
                _enabledValue = value;
            }
        }

        public DataGridViewExCheckBoxColumn()
        {
            this.CellTemplate = new DataGridViewExCheckBoxCell();
        }
    }
}
