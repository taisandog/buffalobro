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
    public class DataGridViewExCheckBoxCell : DataGridViewCheckBoxCell
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
        // Override the Clone method so that the Enabled property is copied.
        public override object Clone()
        {
            DataGridViewExCheckBoxCell cell =
                (DataGridViewExCheckBoxCell)base.Clone();
            cell.Enabled = this.Enabled;
            return cell;
        }

        // By default, enable the CheckBox cell.
        public DataGridViewExCheckBoxCell()
        {
        }

        

        private DataGridViewExCheckBoxColumn _belongColumn;

        /// <summary>
        /// 所属列
        /// </summary>
        public DataGridViewExCheckBoxColumn BelongColumn
        {
            get 
            {
                if (_belongColumn == null) 
                {
                    _belongColumn = this.DataGridView.Columns[this.ColumnIndex] as DataGridViewExCheckBoxColumn;
                }
                return _belongColumn; 
            }
            
        }

        /// <summary>
        /// 真正的可用性
        /// </summary>
        private bool RealEnable 
        {
            get 
            {
                bool enbaleCell = _enabledValue;

                if (BelongColumn != null && !BelongColumn.Enabled)
                {
                    enbaleCell = false;
                }
                return enbaleCell;
            }
        }

        protected override void OnContentClick(DataGridViewCellEventArgs e)
        {
            if (RealEnable)
            {
                base.OnContentClick(e);
            }
        }

        protected override void OnContentDoubleClick(DataGridViewCellEventArgs e)
        {
            if (RealEnable)
            {
                base.OnContentDoubleClick(e);
            }
        }

        

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value,
            object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // The checkBox cell is disabled, so paint the border,  
            // background, and disabled checkBox for the cell.



            if (!RealEnable)
            {
                // Draw the cell background, if specified.
                if ((paintParts & DataGridViewPaintParts.Background) ==
                    DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground =
                        new SolidBrush(cellStyle.BackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }

                // Draw the cell borders, if specified.
                if ((paintParts & DataGridViewPaintParts.Border) ==
                    DataGridViewPaintParts.Border)
                {
                    PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                        advancedBorderStyle);
                }

                // Calculate the area in which to draw the checkBox.
                CheckBoxState state = value != null && (bool)value ?
                    CheckBoxState.CheckedDisabled : CheckBoxState.UncheckedDisabled;
                Size size = CheckBoxRenderer.GetGlyphSize(graphics, state);
                Point center = new Point(cellBounds.X, cellBounds.Y);
                center.X += (cellBounds.Width - size.Width) / 2;
                center.Y += (cellBounds.Height - size.Height) / 2;

                // Draw the disabled checkBox.
                CheckBoxRenderer.DrawCheckBox(graphics, center, state);
            }
            else
            {
                // The checkBox cell is enabled, so let the base class 
                // handle the painting.
                base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                    elementState, value, formattedValue, errorText,
                    cellStyle, advancedBorderStyle, paintParts);
            }
        }
    }
}
