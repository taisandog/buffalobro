using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Buffalo.WinFormsControl.DataGridViewEx
{
    public class DataGridViewImageButtomColumn:DataGridViewButtonColumn
    {
        private Image _backgroundImage;

        public Image BackgroundImage
        {
            get { return _backgroundImage; }
            set { _backgroundImage = value; }
        }


        public DataGridViewImageButtomColumn()
        {
            base.CellTemplate = new DataGridViewImageButtomCell();
        }
        
    }
}
