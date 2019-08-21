using Buffalo.DB.EntityInfos;
using Buffalo.Kernel;
using System;
using TestApp.BQLEntity;
using TestApp.Business;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestDB.InitDB();

            TEUserBusiness bo = new TEUserBusiness();
            TEUser user = CH.Create<TEUser>();
            user.Name = "taisandog";
            bo.Insert(user, true);
            Console.WriteLine(user.Id);
        }
    }
}
