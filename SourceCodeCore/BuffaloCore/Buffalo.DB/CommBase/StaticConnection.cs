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
using Buffalo.DB.BQLCommon.BQLBaseFunction;

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
        }
        /// <summary>
        /// 获取数据库的静态连接
        /// </summary>
        /// <param name="db">数据库信息</param>
        /// <returns></returns>
        public static DataBaseOperate GetStaticOperate(DBInfo db) 
        {
            DataBaseOperate oper = db.SelectedOperate;
            if (oper==null) 
            {
                oper = new DataBaseOperate(db, true);

                if (oper.DBInfo.SqlOutputer.HasOutput)
                {
                    oper.OutMessage(MessageType.OtherOper, "CreateConnection", null, "NewConnection");
                }

                db.SelectedOperate = oper;
            }
            BQL.HotAsyncContext();
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
