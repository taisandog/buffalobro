using Buffalo.Storage.LocalFileManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LocalFileAdapter fa = new LocalFileAdapter(@"\\192.168.1.50\Movies\", "taisandog", "vsdognet2010--");
            
            //fa.Open();

            List<string> files = fa.GetFiles("\\",SearchOption.AllDirectories);
            List<string> dics = fa.GetDirectories("\\来生不做香港人\\", SearchOption.AllDirectories);
            //fa.Close();
        }
    }
}
