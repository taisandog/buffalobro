using Buffalo.Kernel;
using Buffalo.MongoDB;
using Buffalo.MongoDB.ProxyBase;
using Buffalo.MongoDB.QueryCondition;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                UserInfo user = new UserInfo();
                user._id = ObjectId.GenerateNewId();
                user.Name = "taisandog" + i;
                user.CreateTime = (long)CommonMethods.ConvertDateTimeInt(DateTime.Now);
                user.UserId = i + 1;
                UserInfo.Context.Insert(user);
            }
            sw.Stop();
            MessageBox.Show(sw.ElapsedMilliseconds.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ConditionList<UserInfo> list = new ConditionList<UserInfo>();
            //list.AddMoreThen<int>("UserId", 50);
            //list.OrderBy.Add("CreateTime", MGSortType.DESC);
            //list.OrderBy.Add("_id", MGSortType.DESC);
            //list.PageContext.PageSize = 100;
            //list.PageContext.CurrentPage = 0;

            List<UserInfo> lst = UserInfo.Context.SelectList(list);
            dataGridView1.DataSource = lst;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UserInfo info = MCH.Create<UserInfo>();
            info._id = ObjectId.GenerateNewId();
            info.UserId = 1020;
            info.CreateDate = DateTime.Now;
            info.Name = "taisandogX2";
            UserInfo.Context.Insert(info);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DateTime dt = CommonMethods.ConvertIntDateTime(int.MaxValue);
            MessageBox.Show(dt.ToString("yyyy-MM-dd HH:mm:ss"));

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ConditionList<UserInfo> list = new ConditionList<UserInfo>();
            list.AddShowProperty("UserId", MGAggregateType.Sum, "UserId");
            List<UserInfo> lst = UserInfo.Context.SelectList(list);
            dataGridView1.DataSource = lst;
        }
    }
}
