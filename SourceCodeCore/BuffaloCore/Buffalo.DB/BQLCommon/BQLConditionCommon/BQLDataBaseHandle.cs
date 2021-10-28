using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.DbCommon;
using Buffalo.DB.CommBase;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.Kernel.FastReflection;
using System.Reflection;
using Buffalo.DB.MessageOutPuters;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{

    public class BQLDataBaseHandle<T>
    {
        private static DBInfo _db = null;

        /// <summary>
        /// ��ȡ���ݿ���Ϣ
        /// </summary>
        /// <returns></returns>
        public static DBInfo GetDBinfo()
        {
            return _db;
        }
        /// <summary>
        /// ���߳�ʹ�õ�������Դ��-1��Ϊ�ָ�������Դ��
        /// </summary>
        public static int SelectedDataSource
        {
            get
            {
                return _db.SelectedDataSource;
            }
            set
            {
                _db.SelectedDataSource = value;
            }
        }
        /// <summary>
        /// �Ƿ��Ѿ���ʼ��
        /// </summary>
        private static bool _isInit = false;

        /// <summary>
        /// ��ʼ�����ݿ�
        /// </summary>
        public static void InitDB()
        {
            InitDB(null);
        }
        /// <summary>
        /// ��ʼ�����ݿ�
        /// </summary>
        /// <param name="configPath">�Զ��������ļ�·��</param>
        /// <param name="lstChildDBInfo">������Դ</param>
        public static void InitDB(IEnumerable<string> configPath=null,IEnumerable<DBInfo> lstChildDBInfo=null)
        {
            //if (_isInit)
            //{
            //    return;
            //}
            Type type = typeof(T);
            DataAccessLoader.AppendModelAssembly(type.Assembly);
            DataAccessLoader.InitConfig(configPath);
            _db = GetDB(configPath);
            foreach(DBInfo info in lstChildDBInfo) 
            {
                _db.SetChildDataSource(info);
            }
            Type baseType = typeof(BQLEntityTableHandle);
            PropertyInfo[] infos = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (PropertyInfo info in infos)
            {
                Type objType = info.PropertyType;
                if (!objType.IsSubclassOf(baseType))
                {
                    continue;
                }
                BQLEntityTableHandle handle = FastValueGetSet.GetGetMethodInfo(info.Name, type).Invoke(null, new object[] { }) as BQLEntityTableHandle;
                AddToDB(handle);
            }
            StaticConnection.ClearCacheOperate(_db);

        }
        /// <summary>
        /// �������ɵ�SQL��������������ȼ��Ż��ɶ���
        /// </summary>
        /// <param name="enable"></param>
        public static void SetOperatorPrecedenceEnable(bool enable)
        {
            _db.OperatorPrecedence = enable;
        }

        /// <summary>
        /// ��ȡĬ������
        /// </summary>
        public static DataBaseOperate GetDefaultOperate()
        {
            return _db.DefaultOperate;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public static DBTransaction StartTransaction()
        {

            return GetDefaultOperate().StartTransaction();
        }
        /// <summary>
        /// ��ʼ���������������
        /// </summary>
        /// <returns></returns>
        public static BatchAction StartBatchAction()
        {

            return GetDefaultOperate().StarBatchAction();
        }

        /// <summary>
        /// ��ӵ�����Ϣ
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        protected static BQLEntityTableHandle AddToDB(BQLEntityTableHandle table)
        {
            _db.AddToDB(table);
            return table;
        }

        /// <summary>
        /// �������ݿ�����
        /// </summary>
        /// <returns></returns>
        public static DataBaseOperate CreateOperate()
        {
            DataBaseOperate oper = _db.CreateOperate();
            return oper;
        }

        /// <summary>
        /// ͨ��ʵ�����Ͳ��Ҷ�Ӧ��BQL����Ϣ
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static BQLEntityTableHandle FindTable(Type entityType)
        {

            return _db.FindTable(entityType);
        }

        /// <summary>
        /// ͨ��ʵ�����Ͳ��Ҷ�Ӧ��BQL����Ϣ
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static BQLEntityTableHandle FindTable(string fullName)
        {

            return _db.FindTable(fullName);
        }

        /// <summary>
        /// ��ȡ��ǰ�������DB��Ϣ
        /// </summary>
        /// <returns></returns>
        private static DBInfo GetDB(IEnumerable<string> configPath)
        {
            Type cType = typeof(T);
            DataBaseAttribute att = FastInvoke.GetClassAttribute<DataBaseAttribute>(cType);
            if (att == null)
            {
                throw new Exception(cType.FullName + "�໹û����DataBaseAttribute��ǩ");
            }
            string dbName = att.DataBaseName;
            DataAccessLoader.InitConfig(configPath);
            return DataAccessLoader.GetDBInfo(dbName);
        }
    }
}
