using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace MoveDataLink
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            string path = FrmDefault.ShowPath();
            if (!string.IsNullOrEmpty(path)) 
            {
                txtSource.Text = path;
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtSource.Text)) 
            {
                MessageBox.Show("找不到源文件夹");
            }
            if (string.IsNullOrEmpty(txtTarget.Text)) 
            {
                MessageBox.Show("请选择目标文件夹");
            }
            try
            {
                CopyFolder(txtSource.Text, txtTarget.Text);

                Directory.Delete(txtSource.Text,true);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            string res = MKLink(txtSource.Text, txtTarget.Text);
            MessageBox.Show("成功移动:" + res);
        }

        /// <summary>
        /// 建立关联
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private string MKLink(string source,string target) 
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.AutoFlush = true;
            p.StandardInput.WriteLine("mklink /D \"" +source+ "\" \"" + target + "\"");
            p.StandardInput.WriteLine("exit");
            string strRst = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();
            return strRst;
        }

        /// <summary>
        /// Copy文件夹
        /// </summary>
        /// <param name="sPath">源文件夹路径</param>
        /// <param name="dPath">目的文件夹路径</param>
        /// <returns>完成状态：success-完成；其他-报错</returns>
        public void CopyFolder(string sPath, string dPath)
        {
            // 创建目的文件夹
            if (!Directory.Exists(dPath))
            {
                Directory.CreateDirectory(dPath);
            }

            // 拷贝文件
            DirectoryInfo sDir = new DirectoryInfo(sPath);
            FileInfo[] fileArray = sDir.GetFiles();
            foreach (FileInfo file in fileArray)
            {
                file.CopyTo(dPath + "\\" + file.Name, true);
            }

            // 循环子文件夹
            DirectoryInfo dDir = new DirectoryInfo(dPath);
            DirectoryInfo[] subDirArray = sDir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirArray)
            {
                CopyFolder(subDir.FullName, dPath + "//" + subDir.Name);
            }
        }
    }
}