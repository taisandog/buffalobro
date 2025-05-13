using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using Buffalo.DBTools.HelperKernel;
using Buffalo.Win32Kernel;

namespace Buffalo.DBTools
{
    public class GridViewComboBoxCell
    {
        ComboBox _curComboBox = new ComboBox();
        public GridViewComboBoxCell(Control parent) 
        {
            parent.Controls.Add(_curComboBox);
            _curComboBox.Visible = false;
            _curComboBox.Leave += new EventHandler(_curComboBox_Leave);
        }

        void _curComboBox_Leave(object sender, EventArgs e)
        {
            if (_cell != null) 
            {
                _cell.Value = _curComboBox.Text;

                if (_curComboBox.SelectedItem != null)
                {
                     _cell.Tag = _curComboBox.SelectedItem;
                }
                
                HideComboBox();
            }
        }

        DataGridViewCell _cell;
        /// <summary>
        /// 显示ComboBox
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="dataSource"></param>
        public void ShowComboBox(DataGridViewCell cell) 
        {
            
            
            Rectangle rect = cell.DataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, false);
            _curComboBox.Left = rect.Left;
            _curComboBox.Top = rect.Top ;
            _curComboBox.Width = rect.Width;


            if (_curComboBox.DropDownStyle == ComboBoxStyle.DropDown)
            {
                if (cell.Value != null)
                {
                    _curComboBox.Text = cell.Value.ToString();
                }
            }
            else
            {
                for (int i = 0; i < _curComboBox.Items.Count; i++)
                {
                    if (_curComboBox.Items[i]!=null && _curComboBox.Items[i].ToString().Equals(cell.Value.ToString()))
                    {
                        _curComboBox.SelectedIndex = i;
                        break;
                    }
                }

            }
            //_curComboBox.SelectedText = cell.Value.ToString();
            
            
            _curComboBox.Show();

            _cell = cell;
            _curComboBox.BringToFront();
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="dataSource"></param>
        public void SetDataSource(IEnumerable dataSource)
        {
            _curComboBox.Items.Clear();
            if (dataSource == null) 
            {
                return;
            }
            
            foreach (object value in dataSource) 
            {
                _curComboBox.Items.Add(value);
            }
            if (dataSource is IEnumerable<ComboBoxItem>)
            {
                _curComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            else
            {
                _curComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            }
            _curComboBox.Text = "";
        }

        /// <summary>
        /// 隐藏ComboBox
        /// </summary>
        public void HideComboBox() 
        {
            _curComboBox.Hide();
            _cell = null;
        }
    }


    

}
