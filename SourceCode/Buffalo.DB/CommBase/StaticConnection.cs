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
    /// ��̬���ӹ�����
    /// </summary>
    public class StaticConnection
    {
        /// <summary>
        /// ������ӻ���
        /// </summary>
        /// <param name="db"></param>
        public static void ClearCacheOperate(DBInfo db)
        {
            db.SelectedOperate = null;
        }
        /// <summary>
        /// ��ȡ���ݿ�ľ�̬����
        /// </summary>
        /// <param name="db">���ݿ���Ϣ</param>
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
            return oper;
        }

        /// <summary>
        /// ��ȡ��ʵ���Ĭ������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataBaseOperate GetDefaultOperate<T>() 
        {
            return StaticConnection.GetStaticOperate(EntityInfoManager.GetEntityHandle(typeof(T)).DBInfo);
        }

    }
}
