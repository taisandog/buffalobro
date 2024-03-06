using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;


namespace Library
{
    public partial class MessageBox : UserControl
    {
        int MaxLength = 0;
        public MessageBox()
        {
            InitializeComponent();
            //string path = CommonMethods.GetBaseRoot() + "\\Log\\";
            //ErrorLog.SetPathAndFileName("Log", . "ErrorAndWarning", true);
        }

        private static readonly Color LogColor = Color.Black;
        private static readonly Color WarningColor = Color.FromArgb(218,128,0);
        private static readonly Color ErrorColor = Color.FromArgb(255, 0, 0);
        public bool ShowLog {
            get
            {
                return Cbx_Normal.Checked;
            }
            set
            {
                Cbx_Normal.Checked = value;
            }
        }
        public bool ShowError
        {
            get
            {
                return Cbx_Error.Checked;
            }
            set
            {
                Cbx_Error.Checked = value;
            }
        }
        public bool ShowWarning
        {
            get
            {
                return Cbx_Warning.Checked;
            }
            set
            {
                Cbx_Warning.Checked = value;
            }
        }


        public void Log(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || !ShowLog)
            {
                return;
            }
            this.Invoke((EventHandler)delegate
            {
                dgDisplay.Rows.Add("消息",message, DateTime.Now);
                dgDisplay.Rows[dgDisplay.Rows.Count - 1].DefaultCellStyle.ForeColor = LogColor;
                CheckRow();
            });
        }
        public void LogError(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || !ShowError)
            {
                return;
            }
            this.Invoke((EventHandler)delegate
            {
                dgDisplay.Rows.Add("错误",message, DateTime.Now);
                dgDisplay.Rows[dgDisplay.Rows.Count - 1].DefaultCellStyle.ForeColor = ErrorColor;
                CheckRow();
            });

            //ApplicationLog.LogError(message);
        }
        
        private void CheckRow()
        {
            int max = (int)nupMax.Value;
            while (dgDisplay.Rows.Count > max)
            {
                dgDisplay.Rows.RemoveAt(0);
            }
            dgDisplay.CurrentCell = dgDisplay.Rows[dgDisplay.Rows.Count - 1].Cells[0];
            dgDisplay.CurrentCell = null;
        }
       
        public void LogWarning(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || !ShowWarning)
            {
                return;
            }
            this.Invoke((EventHandler)delegate
            {
                dgDisplay.Rows.Add("警告",message, DateTime.Now);
                dgDisplay.Rows[dgDisplay.Rows.Count - 1].DefaultCellStyle.ForeColor = WarningColor;
                CheckRow();
            });
            //ApplicationLog.LogWarning(message);
        }
        public void LogFailChangeValue(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || !ShowWarning)
            {
                return;
            }
            this.Invoke((EventHandler)delegate
            {
                dgDisplay.Rows.Add(message, DateTime.Now);
                dgDisplay.Rows[dgDisplay.Rows.Count - 1].Cells[0].Style.ForeColor = Color.Gold;
                CheckRow();
            });
            //ApplicationLog.LogError(message);
        }

        private void tsCopy_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgDisplay.CurrentRow;
            if (row == null)
            {
                return;
            }
            //sb.AppendLine("===="+row.Cells[0].Value + ":" + row.Cells[2].Value+ "====");

            StringBuilder sb = new StringBuilder();
            sb.Append("====");
            sb.Append(row.Cells[0].Value.ToString());
            sb.Append(":");
            sb.Append(row.Cells[2].Value.ToString());
            sb.AppendLine("====");
            sb.AppendLine( row.Cells[1].Value.ToString());
            Clipboard.SetDataObject(sb.ToString());
        }

        private void dgDisplay_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgDisplay.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dgDisplay.Rows.Clear();
        }

        //private void MessageBox_SizeChanged(object sender, EventArgs e)
        //{
        //    Console.WriteLine("尺寸------"+richTextBox1.Height);            
        //    MaxLength = (richTextBox1.Height / (richTextBox1.Font.Height + 2) - 1);
        //    Console.WriteLine("数量" + MaxLength);
        //    if (MaxLength < 10)
        //    {
        //        MaxLength = 10;
        //    }
        //}

    }

}
