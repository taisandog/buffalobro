using Buffalo.DB.CacheManager;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel;
using System;
using System.Collections.Generic;
using TestApp.BQLEntity;
using TestApp.Business;
using TestLib;
using TestLib.BQLEntity;
using TestLib.Business;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //DataAccessLoader.AppendModelAssembly(typeof(YMRDB).Assembly);
            //DataAccessLoader.AppendModelAssembly(typeof(TestDB).Assembly);
            //TestDB.InitDB();
            //YMRDB.InitDB();
            //YmrrankinglistBusiness bo = new YmrrankinglistBusiness();
            //ScopeList lstScope = new ScopeList();
            //lstScope.OrderBy.Add(YMRDB.Ymrrankinglist.Id.DESC);
            //lstScope.PageContent.PageSize = 20;
            //lstScope.PageContent.CurrentPage = 0;
            //List<Ymrrankinglist> lst = bo.SelectList(lstScope);
            //foreach (Ymrrankinglist obj in lst)
            //{
            //    Console.WriteLine(obj.Id+","+obj.FishName);
            //}
            string connString = "server=192.168.1.26:11211;throw=0;";
            QueryCache cache = CacheUnit.CreateCache(BuffaloCacheTypes.Memcached, connString);
            Console.WriteLine(cache.SetValue("Key4", "shit",SetValueType.AddNew,30));
            Console.WriteLine(cache.SetValue("Key4", "shit1", SetValueType.AddNew, 30));
            Console.WriteLine(cache.GetValue<string>("Key4"));
        }
    }
}
