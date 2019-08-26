using Buffalo.Kernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ContextValue.Current["aa"] = 10;
            Thread thd = new Thread(new ThreadStart(Anyc));
            thd.Start();
            int val = ContextValue.Current["aa"].ConvertTo<int>();
            MessageBox.Show(val.ToString());
        }

        private void Anyc()
        {
            int val = ContextValue.Current["aa"].ConvertTo<int>();
            MessageBox.Show(val.ToString());

        }
    }
}
