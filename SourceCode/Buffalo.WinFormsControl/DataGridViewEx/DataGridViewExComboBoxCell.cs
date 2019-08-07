using System;
using System.Collections.Generic;

using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Buffalo.WinFormsControl.DataGridViewEx
{
    /// <summary>
    /// 扩展的Checkbox单元格
    /// </summary>
    public class DataGridViewExComboBoxCell : DataGridViewTextBoxCell
    {
        
        // Override the Clone method so that the Enabled property is copied.
        public override object Clone()
        {
            DataGridViewExComboBoxCell cell =
                (DataGridViewExComboBoxCell)base.Clone();
            return cell;
        }

        private object _selectValue;

        /// <summary>
        /// 选中的值
        /// </summary>
        public object SelectValue
        {
            get { return _selectValue; }
            internal set 
            {
                _selectValue = value;
            }
        }

        // By default, enable the CheckBox cell.
        public DataGridViewExComboBoxCell()
        {
        }

        protected override void OnClick(DataGridViewCellEventArgs e)
        {
            BelongColumn.ShowComboBox(this);
            base.OnClick(e);
        }

        protected override void OnLeave(int rowIndex, bool throughMouseClick)
        {
            BelongColumn.HideComboBox();
            base.OnLeave(rowIndex, throughMouseClick);
        }
        private DataGridViewExComboBoxColumn _belongColumn;

        /// <summary>
        /// 所属列
        /// </summary>
        public DataGridViewExComboBoxColumn BelongColumn
        {
            get 
            {
                if (_belongColumn == null) 
                {
                    _belongColumn = this.DataGridView.Columns[this.ColumnIndex] as DataGridViewExComboBoxColumn;
                }
                return _belongColumn; 
            }
            
        }
    }
}
