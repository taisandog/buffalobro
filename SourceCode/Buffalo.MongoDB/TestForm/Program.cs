using Buffalo.DB.CacheManager;
using Buffalo.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm
{
    static class Program
    {
        
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            string connString = System.Configuration.ConfigurationManager.AppSettings["MongoDB.YMRDB.ConnString"];
            string dbName = System.Configuration.ConfigurationManager.AppSettings["MongoDB.YMRDB.DBName"];
            QueryCache cache = CacheUnit.CreateCache("redis", "server=119.23.144.219:6379;throw=0;");
            MongoDBManager.AddConfig("YMRDB", connString, dbName,typeof(Program).Assembly);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
