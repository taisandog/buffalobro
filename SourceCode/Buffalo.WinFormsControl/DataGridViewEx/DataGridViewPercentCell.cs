using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Buffalo.WinFormsControl.DataGridViewEx
{
    public class DataGridViewPercentCell : System.Windows.Forms.DataGridViewCell
    {
        protected override object GetFormattedValue(object value, int rowIndex, ref System.Windows.Forms.DataGridViewCellStyle cellStyle, System.ComponentModel.TypeConverter valueTypeConverter, System.ComponentModel.TypeConverter formattedValueTypeConverter, System.Windows.Forms.DataGridViewDataErrorContexts context)
        {
            if (value == null)
            {
                return "未绑定数据";
            }
            else
            {
                return value;
            }
        }
        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, System.Windows.Forms.DataGridViewElementStates cellState, object value, object formattedValue, string errorText, System.Windows.Forms.DataGridViewCellStyle cellStyle, System.Windows.Forms.DataGridViewAdvancedBorderStyle advancedBorderStyle, System.Windows.Forms.DataGridViewPaintParts paintParts)
        {
            bool canParse = true;
            decimal val = 0m;
            try
            {
                val = Convert.ToDecimal(value);
            }
            catch
            {
                canParse = false;
            }

            DataGridViewPercentColumn col = this.OwningColumn as DataGridViewPercentColumn;
            if (canParse && col.TotalCount > 0)
            {
                decimal decv = val / col.TotalCount;
                if (decv > 1) decv = 1;
                if (decv < 0) decv = 0;
                Brush bs = new SolidBrush(cellStyle.BackColor);
                //Brush bs2 = new SolidBrush(Color.YellowGreen);
                Brush bs2 = new SolidBrush(Color.Red);
                Brush bsstring = new SolidBrush(cellStyle.ForeColor);
                if ((cellState & System.Windows.Forms.DataGridViewElementStates.Selected) == System.Windows.Forms.DataGridViewElementStates.Selected)
                {
                    bs = new SolidBrush(cellStyle.SelectionBackColor);
                    bsstring = new SolidBrush(cellStyle.SelectionForeColor);
                }
                //if (decv < 0.75m)
                //{
                //    if (decv < 0.2m)
                //    {
                //        bs2 = new SolidBrush(Color.Red);
                //    }
                //    else
                //    {
                //        bs2 = new SolidBrush(Color.Orange);
                //    }
                //}
                Pen p = new Pen(DataGridView.GridColor, 1);
                graphics.FillRectangle(bs, cellBounds.X, cellBounds.Y, cellBounds.Width, cellBounds.Height);
                graphics.FillRectangle(bs2, cellBounds.X + 1, cellBounds.Y + 1, (float)((cellBounds.Width - 3) * decv), cellBounds.Height - 3);
                graphics.DrawLine(p, cellBounds.X, cellBounds.Y + cellBounds.Height - 1, cellBounds.X + cellBounds.Width - 1, cellBounds.Y + cellBounds.Height - 1);
                graphics.DrawLine(p, cellBounds.X + cellBounds.Width - 1, cellBounds.Y, cellBounds.X + cellBounds.Width - 1, cellBounds.Y + cellBounds.Height - 1);
                if (cellBounds.Width > cellStyle.Font.Size * 4)
                {
                    string showString = null;
                    if (col.ShowTotal)
                    {

                        showString = GetNumberString(val) + "/" + GetNumberString(col.TotalCount);
                    }
                    else
                    {
                        showString = GetNumberString(val);
                    }
                    graphics.DrawString(showString, cellStyle.Font, bsstring, cellBounds.X + 4, cellBounds.Y + 4);
                }
            }
            else
            {
                Brush bs = new SolidBrush(cellStyle.BackColor);
                if ((cellState & System.Windows.Forms.DataGridViewElementStates.Selected) == System.Windows.Forms.DataGridViewElementStates.Selected)
                {
                    bs = new SolidBrush(cellStyle.SelectionBackColor);
                }
                graphics.FillRectangle(bs, cellBounds.X, cellBounds.Y, cellBounds.Width, cellBounds.Height);
            }
        }


        /// <summary>
        /// 获取数字的格式化字符串
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private string GetNumberString(decimal num)
        {
            DataGridViewPercentColumn col = this.OwningColumn as DataGridViewPercentColumn;
            string format = col.ValueForamt;
            if (string.IsNullOrEmpty(format))
            {
                return num.ToString();
            }
            return num.ToString(format);
        }
    }
}
