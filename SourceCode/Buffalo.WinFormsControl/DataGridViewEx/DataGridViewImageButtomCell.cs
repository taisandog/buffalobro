using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Buffalo.WinFormsControl.DataGridViewEx
{
    public class DataGridViewImageButtomCell:DataGridViewButtonCell
    {

        protected override bool SetValue(int rowIndex, object value)
        {
            return base.SetValue(rowIndex, value);
        }
        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {

            base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            DataGridViewImageButtomColumn col = this.OwningColumn as DataGridViewImageButtomColumn;
            if (col == null)
            {
                return;
            }
            Image img = col.BackgroundImage;
            if (img == null)
            {
                return;
            }
            graphics.DrawImage(img, cellBounds.X + ((cellBounds.Width - img.Size.Width) / 2), cellBounds.Y + ((cellBounds.Height - img.Size.Height) / 2));

            //graphics.FillRectangle(Brushes.Black, cellBounds);

        }
    }
}
