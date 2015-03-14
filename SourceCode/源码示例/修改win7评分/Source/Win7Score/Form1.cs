using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Win7Score
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

            if (!File.Exists(Path))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();

            doc.Load(Path);
            EditItem("CpuScore", txtCpuScore.Text, doc);
            EditItem("DiskScore", txtDiskScore.Text, doc);
            EditItem("GamingScore", txtGamingScore.Text, doc);
            EditItem("GraphicsScore", txtGraphicsScore.Text, doc);
            EditItem("MemoryScore", txtMemoryScore.Text, doc);
            EditItem("SystemScore", txtSystemScore.Text, doc);
            doc.Save(Path);
            MessageBox.Show("修改完毕");

        }

        private void FillItem(string tagName, TextBox txt, XmlDocument doc) 
        {
            XmlNodeList nodes = doc.GetElementsByTagName(tagName);
            if (nodes.Count > 0)
            {
                txt.Text= nodes[0].InnerText ;
            }
        }

        /// <summary>
        /// 修改项
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="value"></param>
        private void EditItem(string tagName, string value, XmlDocument doc)
        {
            XmlNodeList nodes = doc.GetElementsByTagName(tagName);
            if (nodes.Count > 0) 
            {
                nodes[0].InnerText = value;
            }
        }
        string SatDic = System.Environment.GetEnvironmentVariable("windir") + "\\Performance\\WinSAT\\DataStore\\";
        string Path = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] files = System.IO.Directory.GetFiles(SatDic);
            foreach (string fileName in files) 
            {
                if (fileName.IndexOf("Formal.Assessment") >= 0) 
                {
                    Path = fileName;
                }
            }

            if (!File.Exists(Path))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();

            doc.Load(Path);
            FillItem("CpuScore", txtCpuScore, doc);
            FillItem("DiskScore", txtDiskScore, doc);
            FillItem("GamingScore", txtGamingScore, doc);
            FillItem("GraphicsScore", txtGraphicsScore, doc);
            FillItem("MemoryScore", txtMemoryScore, doc);
            FillItem("SystemScore", txtSystemScore, doc);
            

        }
    }
}