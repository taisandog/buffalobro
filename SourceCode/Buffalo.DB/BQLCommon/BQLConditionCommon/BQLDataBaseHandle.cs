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
        /// 获取数据库信息
        /// </summary>
        /// <returns></returns>
        public static DBInfo GetDBinfo()
        {
            return _db;
        }

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private static bool _isInit=false;

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public static void InitDB() 
        {
            //if (_isInit)
            //{
            //    return;
            //}
            Type type = typeof(T);
            DataAccessLoader.AppendModelAssembly(type.Assembly);
            DataAccessLoader.InitConfig();
            _db = GetDB();
            
            Type baseType=typeof(BQLEntityTableHandle);
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
#if DEBUG
            _db.SqlOutputer.OnOutputerCreate += new Buffalo.DB.MessageOutPuters.CreateOutputerHandle(SqlOutputer_OnOutputerCreate);
#endif
        }


#if DEBUG
        static DebugOutputer _outputer = new DebugOutputer();
        static Buffalo.DB.MessageOutPuters.MessageOutputBase SqlOutputer_OnOutputerCreate(DataBaseOperate oper)
        {
            return _outputer;
        }

#endif
        /// <summary>
        /// 设置生成的SQL语句进行运算符优先级优化可读性
        /// </summary>
        /// <param name="enable"></param>
        public static void SetOperatorPrecedenceEnable(bool enable) 
        {
            _db.OperatorPrecedence = enable;
        }

        /// <summary>
        /// 获取默认连接
        /// </summary>
        public static DataBaseOperate GetDefaultOperate()
        {
            return _db.DefaultOperate;
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public static DBTransaction StartTransaction()
        {

            return GetDefaultOperate().StartTransaction() ;
        }
        /// <summary>
        /// 开始非事务的批量动作
        /// </summary>
        /// <returns></returns>
        public static BatchAction StartBatchAction()
        {

            return GetDefaultOperate().StarBatchAction();
        }

        /// <summary>
        /// 添加到库信息
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        protected static BQLEntityTableHandle AddToDB(BQLEntityTableHandle table) 
        {
            _db.AddToDB(table);
            return table;
        }

        /// <summary>
        /// 创新数据库连接
        /// </summary>
        /// <returns></returns>
        public static DataBaseOperate CreateOperate() 
        {
            DataBaseOperate oper = _db.CreateOperate();
            return oper;
        }

        /// <summary>
        /// 通过实体类型查找对应的BQL表信息
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static BQLEntityTableHandle FindTable(Type entityType)
        {

            return _db.FindTable(entityType);
        }

        /// <summary>
        /// 通过实体类型查找对应的BQL表信息
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static BQLEntityTableHandle FindTable(string fullName)
        {

            return _db.FindTable(fullName);
        }

        /// <summary>
        /// 获取当前类关联的DB信息
        /// </summary>
        /// <returns></returns>
        private static DBInfo GetDB() 
        {
            Type cType=typeof(T);
            DataBaseAttribute att=FastInvoke.GetClassAttribute<DataBaseAttribute>(cType);
            if(att==null)
            {
                throw new Exception(cType.FullName+"类还没配置DataBaseAttribute标签");
            }
            string dbName=att.DataBaseName;
            DataAccessLoader.InitConfig();
            return DataAccessLoader.GetDBInfo(dbName);
        }
    }
}
