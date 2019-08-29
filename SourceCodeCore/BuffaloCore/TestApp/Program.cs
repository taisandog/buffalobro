using Buffalo.DB.CacheManager;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel;
using Buffalo.Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using TestApp.BQLEntity;
using TestApp.Business;
using TestLib;
using TestLib.BQLEntity;
using TestLib.Business;

namespace TestApp
{
    class Program
    {
        static string strOSS = "";
        static void Main(string[] args)
        {
            InitDB();
            TestStorage();


        }

        private static void InitDB()
        {
            DataAccessLoader.AppendModelAssembly(typeof(YMRDB).Assembly);
            DataAccessLoader.AppendModelAssembly(typeof(TestDB).Assembly);
            TestDB.InitDB();
            YMRDB.InitDB();
        }
        private static void TestStorage()
        {
            
            IFileStorage storage = FSCreater.Create("OSS", strOSS);
            storage.Open();
            storage.SaveFile("D:\\HQGJ1.0.6.zip", "ttaqytest/HQGJ1.0.6.zip");
            storage.Close();
        }

        private static void TestSelectTE()
        {

            TEUserBusiness bo = new TEUserBusiness();
            List<TEUser> lst = bo.SelectList(new ScopeList());
            foreach(TEUser user in lst)
            {
                Console.WriteLine(user.Id + "," + user.Name+","+ user.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }
        private static void TestInsertTE()
        {

            TEUserBusiness bo = new TEUserBusiness();
            for(int i = 0; i < 10; i++)
            {
                TEUser obj = CH.Create<TEUser>();
                obj.Name = "user" + i;
                bo.Insert(obj, true);
                Console.WriteLine(obj.Id + "," + obj.Name);
            }
        }

        private static void TestYMR()
        {
            
            YmrrankinglistBusiness bo = new YmrrankinglistBusiness();
            ScopeList lstScope = new ScopeList();
            lstScope.OrderBy.Add(YMRDB.Ymrrankinglist.Id.DESC);
            lstScope.PageContent.PageSize = 20;
            lstScope.PageContent.CurrentPage = 0;
            List<Ymrrankinglist> lst = bo.SelectList(lstScope);
            foreach (Ymrrankinglist obj in lst)
            {
                Console.WriteLine(obj.Id + "," + obj.FishName);
            }
        }

        private static void TestMemcached()
        {
            string connString = "server=192.168.1.26:11211;throw=0;";
            QueryCache cache = CacheUnit.CreateCache(BuffaloCacheTypes.Memcached, connString);
            Console.WriteLine(cache.SetValue("Key4", "shit", SetValueType.AddNew, 30));
            Console.WriteLine(cache.SetValue("Key4", "shit1", SetValueType.AddNew, 30));
            Console.WriteLine(cache.GetValue<string>("Key4"));
        }
        private static void TestWebCache()
        {
            string connString = "expir=1";
            QueryCache cache = CacheUnit.CreateCache(BuffaloCacheTypes.Web, connString);
            Console.WriteLine(cache.SetValue("Key4", "shit", SetValueType.AddNew, 30));
            Console.WriteLine(cache.SetValue("Key4", "shit1", SetValueType.AddNew, 30));
            Thread.Sleep(60 * 1000);
            Console.WriteLine(cache.SetValue("Key4", "shit1", SetValueType.AddNew, 30));
            Console.WriteLine(cache.GetValue<string>("Key4"));
        }
    }
}
