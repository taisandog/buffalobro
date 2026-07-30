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
        /// <summary>
        /// 清空连接缓存
        /// </summary>
        /// <param name="db"></param>
        public static void ClearCacheOperate(DBInfo db)
        {
            db.SelectedOperate = null;
            db.SelectedOperateAsync = null;
        }

        /// <summary>
        /// 获取当前同步线程的静态连接。
        /// </summary>
        public static DataBaseOperate GetStaticOperate(DBInfo db)
        {
            DataBaseOperate oper = db.SelectedOperate;
            if (oper == null)
            {
                oper = CreateOperate(db.SelectedDBInfoSync);
                db.SelectedOperate = oper;
            }
            return oper;
        }

        /// <summary>
        /// 获取当前异步调用链的静态连接。
        /// </summary>
        public static DataBaseOperate GetStaticOperateAsync(DBInfo db)
        {
            DataBaseOperate oper = db.SelectedOperateAsync;
            if (oper == null)
            {
                oper = CreateOperate(db.SelectedDBInfoAsync);
                db.SelectedOperateAsync = oper;
            }
            return oper;
        }

        private static DataBaseOperate CreateOperate(DBInfo db)
        {
            DataBaseOperate oper = new DataBaseOperate(db, true);
            if (oper.DBInfo.SqlOutputer.HasOutput)
            {
                oper.OutMessage(MessageType.OtherOper, "CreateConnection", null, "NewConnection");
            }
            return oper;
        }

        public static DataBaseOperate GetDefaultOperate<T>()
        {
            return GetStaticOperate(EntityInfoManager.GetEntityHandle(typeof(T)).DBInfo);
        }

        public static DataBaseOperate GetDefaultOperateAsync<T>()
        {
            return GetStaticOperateAsync(EntityInfoManager.GetEntityHandle(typeof(T)).DBInfo);
        }

    }
}
