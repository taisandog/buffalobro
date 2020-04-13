using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Web;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.EntityInfos;
using Buffalo.Kernel;
using System.Diagnostics;
using Buffalo.DB.MessageOutPuters;

namespace Buffalo.DB.CommBase
{
    /// <summary>
    /// 静态连接管理类
    /// </summary>
    public class StaticConnection
    {
        private const string SessionName = "_$*&Buff.StConn.";

        /// <summary>
        /// 获取数据连接
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static DataBaseOperate GetOperate(string name)
        {
            return ContextValue.Current[name] as DataBaseOperate;
            //if (Buffalo.Kernel.CommonMethods.IsWebContext)
            //{
            //    return (DataBaseOperate)System.Web.HttpContext.Current.Items[name];
            //}
            //else
            //{
            //    return (DataBaseOperate)System.Runtime.Remoting.Messaging.CallContext.GetData(name);
            //}
        }

        /// <summary>
        /// 设置数据连接
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static void SetOperate(DataBaseOperate value,string name)
        {

            ContextValue.Current[name] = value;

            //if (CommonMethods.IsWebContext)
            //{
            //    System.Web.HttpContext.Current.Items[name] = value;
            //}
            //else
            //{
            //    System.Runtime.Remoting.Messaging.CallContext.SetData(name, value);
            //}
            
        }

        /// <summary>
        /// 清空连接缓存
        /// </summary>
        /// <param name="DbName"></param>
        public static void ClearCacheOperate(string dbName) 
        {
            string key = SessionName + dbName;
            SetOperate(null, key);
        }
        /// <summary>
        /// 清空连接缓存
        /// </summary>
        /// <param name="DbName"></param>
        public static void ClearCacheOperate(DBInfo db)
        {
            ClearCacheOperate(db.Name);
        }
        /// <summary>
        /// 获取数据库的静态连接
        /// </summary>
        /// <param name="db">数据库信息</param>
        /// <returns></returns>
        public static DataBaseOperate GetStaticOperate(DBInfo db) 
        {
            string key = SessionName+db.Name;
            DataBaseOperate oper = GetOperate(key) as DataBaseOperate;
            if (oper==null) 
            {
                oper = new DataBaseOperate(db, true);

                if (oper.DBInfo.SqlOutputer.HasOutput)
                {
                    oper.OutMessage(MessageType.OtherOper, "CreateConnection", null, "NewConnection");
                }
                
                SetOperate(oper,key);
            }
            return oper;
        }

        /// <summary>
        /// 获取此实体的默认连接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataBaseOperate GetDefaultOperate<T>() 
        {
            return StaticConnection.GetStaticOperate(EntityInfoManager.GetEntityHandle(typeof(T)).DBInfo);
        }

    }
}
