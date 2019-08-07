using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.WinFormsControl.DataGridViewEx
{
    public class DataGridViewPercentColumn:System.Windows.Forms.DataGridViewColumn
    {

        public DataGridViewPercentColumn()
        {
            base.CellTemplate = new DataGridViewPercentCell();
        }

        private bool _showTotal = false;

        /// <summary>
        /// 是否在值后边显示总数
        /// </summary>
        public bool ShowTotal
        {
            get
            {
                return _showTotal;
            }
            set
            {
                _showTotal = value;
            }
        }

        private decimal _TotalCount;
        /// <summary>
        /// 列总数
        /// </summary>
        public decimal TotalCount
        {
            get { return _TotalCount; }
            set { _TotalCount = value; }
        }

        private string _valueForamt;

        /// <summary>
        /// 值的格式化
        /// </summary>
        public string ValueForamt
        {
            get { return _valueForamt; }
            set { _valueForamt = value; }
        }
    }
}
