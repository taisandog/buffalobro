using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DirectShowLib;

namespace CameraTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        CameraCapture cap;
        FrmCamera frm;
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            frm = new FrmCamera();
            cap = CameraCapture.CreateCapture(frm, 640, 480);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Image img = cap.GetCaptureImage();
            cap.FreeCaptureImage();
            pictureBox1.Image = img;
            
        }
    }
}