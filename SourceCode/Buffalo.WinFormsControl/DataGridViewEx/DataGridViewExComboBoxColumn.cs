using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using Buffalo.Win32Kernel;

namespace Buffalo.WinFormsControl.DataGridViewEx
{
    public class DataGridViewExComboBoxColumn : DataGridViewTextBoxColumn
    {
        ComboBox _columnComboBox;

        /// <summary>
        /// 列所属的ComboBox
        /// </summary>
        public ComboBox ColumnComboBox
        {
            get { return _columnComboBox; }
        }
        
        /// <summary>
        /// 隐藏ComboBox
        /// </summary>
        public void HideComboBox()
        {
            _columnComboBox.Hide();
        }
        /// <summary>
        /// 显示ComboBox
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="dataSource"></param>
        public void ShowComboBox(DataGridViewCell cell)
        {
            ComboBox cmb = ColumnComboBox;

            Rectangle rect = this.DataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, false);
            cmb.Left = rect.Left;
            cmb.Top = rect.Top;
            cmb.Width = rect.Width;


            if (cmb.DropDownStyle == ComboBoxStyle.DropDown)
            {
                if (cell.Value != null)
                {
                    cmb.Text = cell.Value.ToString();
                }
            }
            else
            {
                for (int i = 0; i < cmb.Items.Count; i++)
                {

                    if (cmb.Items[i] != null && cmb.Items[i].ToString().Equals(cell.Value == null ? "" : cell.Value.ToString()))
                    {
                        cmb.SelectedIndex = i;
                        break;
                    }
                }

            }
            cmb.Show();
            cmb.BringToFront();
        }
        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="dataSource"></param>
        public void SetComboBoxDataSource(IEnumerable dataSource)
        {
            _columnComboBox.Items.Clear();
            if (dataSource == null)
            {
                return;
            }

            foreach (object value in dataSource)
            {
                _columnComboBox.Items.Add(value);
            }
            if (dataSource is IEnumerable<ComboBoxItem>)
            {
                _columnComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            else
            {
                _columnComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            }
            _columnComboBox.Text = "";
        }

        public DataGridViewExComboBoxColumn()
        {
            
            
            this.CellTemplate = new DataGridViewExComboBoxCell();
           
        }
        protected override void OnDataGridViewChanged()
        {
            if (this.DataGridView == null) 
            {
                return;
            }
            _columnComboBox = new ComboBox();
            this.ReadOnly = true;

            this.DataGridView.Controls.Add(_columnComboBox);
            _columnComboBox.Visible = false;
            _columnComboBox.Leave += new EventHandler(_columnComboBox_Leave);
            base.OnDataGridViewChanged();
        }

        void _columnComboBox_Leave(object sender, EventArgs e)
        {
            DataGridViewExComboBoxCell cell = this.DataGridView.CurrentCell as DataGridViewExComboBoxCell;
            if (cell != null)
            {
                cell.Value = _columnComboBox.Text;

                if (_columnComboBox.SelectedItem != null)
                {
                    cell.SelectValue = _columnComboBox.SelectedItem;
                }
                else 
                {
                    cell.SelectValue = cell.Value;
                }

                HideComboBox();
            }
        }
        public override bool ReadOnly
        {
            get
            {
                return base.ReadOnly;
            }
            set
            {
                
            }
        }

    }
}
