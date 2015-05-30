using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.EntityInfos;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.CommBase.BusinessBases;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.ProxyBuilder;
using Buffalo.DB.CacheManager;
using Buffalo.Kernel;

namespace Buffalo.DB.CommBase.DataAccessBases
{
    public class DataAccessSetBase
    {
        public DataAccessSetBase(EntityInfoHandle entityInfo) 
        {
            _entityInfo = entityInfo;
            
        }


        private EntityInfoHandle _entityInfo;

        /// <summary>
        /// 当前实体信息
        /// </summary>
        public virtual EntityInfoHandle EntityInfo
        {
            get { return _entityInfo; }
        }
        

        protected DataBaseOperate _oper;

        protected BQLDbBase _cdal;//BQL数据层类

        /// <summary>
        /// BQL上下文
        /// </summary>
        protected internal BQLDbBase ContextDAL
        {
            get
            {
                return _cdal;
            }
        }

        /// <summary>
        /// 修改记录
        /// </summary>
        /// <param name="obj">修改的对象</param>
        /// <param name="scopeList">条件列表</param>
        /// <param name="setList">Set值列表</param>
        /// <param name="optimisticConcurrency">是否进行并发控制</param>
        /// <returns></returns>
        public int Update(EntityBase obj, ScopeList scopeList, ValueSetList setList, bool optimisticConcurrency)
        {
            StringBuilder sql = new StringBuilder(500);
            ParamList list = new ParamList();
            StringBuilder where = new StringBuilder(500);
            //where.Append("1=1");
            Type type = EntityInfo.EntityType;
            List<VersionInfo> lstVersionInfo = null;
            int index = 0;

            KeyWordInfomation keyinfo = BQLValueItem.GetKeyInfo().Clone() as KeyWordInfomation;
            keyinfo.ParamList = list;
            keyinfo.OutPutModle = false;
            if (obj != null)
            {
                bool isProxy = true;
                if (!(obj is IEntityProxy))//如果为非代理类则全实体更新
                {
                    //throw new System.InvalidCastException("Update的实体类型必须为代理类，请用CH.Create<T>创建实体或者使用查询出来的实体来更新");
                    isProxy = false;
                }
                ///读取属性别名
                foreach (EntityPropertyInfo info in EntityInfo.PropertyInfo)
                {


                    object curValue = info.GetValue(obj);
                    if (!info.ReadOnly)
                    {
                        if (optimisticConcurrency == true && info.IsVersion) //并发控制
                        {

                            object newValue = FillUpdateConcurrency(sql, info, list, curValue, ref index);
                            FillWhereConcurrency(where, info, list, curValue, ref index);
                            if (lstVersionInfo == null)
                            {
                                lstVersionInfo = new List<VersionInfo>();
                            }
                            lstVersionInfo.Add(new VersionInfo(info, curValue, newValue));//添加信息
                        }
                        else
                        {
                            //string paramVal = CurEntityInfo.DBInfo.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(info.ParamName, index));
                            //string paramKey = CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(info.ParamName, index));
                            if (info.IsNormal)
                            {
                                //if (obj._dicUpdateProperty___ == null || obj._dicUpdateProperty___.Count == 0)
                                //{
                                //    //if (DefaultType.IsDefaultValue(curValue))
                                //    //{
                                //    continue;
                                //    //}
                                //}
                                if (isProxy && !obj.HasPropertyChange(info.PropertyName))
                                {
                                    continue;
                                }
                                if (setList != null)//如果强制赋值集合已经有，则不使用实体值更新
                                {
                                    BQLValueItem bvalue = null;
                                    if (setList.TryGetValue(info.PropertyName, out bvalue))
                                    {
                                        continue;
                                    }
                                }
                                sql.Append(",");
                                sql.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                                sql.Append("=");
                                if (curValue != null)
                                {
                                    DBParameter dbPrm = list.NewParameter(info.SqlType, curValue, EntityInfo.DBInfo);
                                    sql.Append(dbPrm.ValueName);
                                }
                                else
                                {
                                    sql.Append("null");
                                }
                            }
                        }
                    }

                    if (info.IsPrimaryKey && scopeList == null)//当不强制指定条件时候，用主键做更新条件
                    {
                        //if (DefaultType.IsDefaultValue(curValue))
                        //{
                        //    continue;
                        //}
                        //if (obj._dicUpdateProperty___ != null) 
                        //{
                        //    if (!obj._dicUpdateProperty___.ContainsKey(info.PropertyName))
                        //    {
                        //        continue;
                        //    }
                        //}

                        DBParameter dbPrm = list.NewParameter(info.SqlType, curValue, EntityInfo.DBInfo);
                        where.Append(" and ");
                        where.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                        where.Append("=");
                        where.Append(dbPrm.ValueName);
                        //primaryKeyValue=curValue;
                    }
                    index++;



                }

            }
            if (setList != null)
            {
                foreach (KeyValuePair<string, BQLValueItem> kvp in setList)
                {
                    EntityPropertyInfo epinfo = EntityInfo.PropertyInfo[kvp.Key];
                    if (epinfo == null)
                    {
                        throw new Exception("实体:" + EntityInfo.EntityType.FullName + "  找不到属性:" + kvp.Key);
                    }
                    sql.Append(",");
                    sql.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(epinfo.ParamName));
                    sql.Append("=");
                    sql.Append(kvp.Value.DisplayValue(keyinfo));
                }
            }
            where.Append(DataAccessCommon.FillCondition(EntityInfo, list, scopeList));
            if (sql.Length <= 0)
            {
                return 0;
            }
            else
            {
                sql.Remove(0, 1);

            }
            UpdateCondition con = new UpdateCondition(EntityInfo.DBInfo);
            con.Tables.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(EntityInfo.TableName));
            con.UpdateSetValue.Append(sql);
            con.Condition.Append("1=1");

            con.Condition.Append(where);

            int ret = -1;
            Dictionary<string, bool> cacheTables = null;

            cacheTables = _oper.DBInfo.QueryCache.CreateMap(EntityInfo.TableName);

            ret = ExecuteCommand(con.GetSql(true), list, CommandType.Text, cacheTables);
            if (obj != null && obj._dicUpdateProperty___ != null)
            {
                obj._dicUpdateProperty___.Clear();
            }
            if (lstVersionInfo != null && lstVersionInfo.Count > 0)
            {
                foreach (VersionInfo info in lstVersionInfo)
                {
                    info.Info.SetValue(obj, info.NewValue);
                }
            }

            return ret;
        }

        /// <summary>
        /// 填充版本控制的信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="where"></param>
        /// <param name="info"></param>
        /// <param name="list"></param>
        /// <param name="curValue"></param>
        protected internal void FillWhereConcurrency(StringBuilder where,
            EntityPropertyInfo info, ParamList list, object curValue, ref int index)
        {


            string paramValW = EntityInfo.DBInfo.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(info.ParamName, index));
            string paramKeyW = EntityInfo.DBInfo.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(info.ParamName, index));
            index++;
            if (DefaultType.IsDefaultValue(curValue))
            {
                throw new Exception("版本控制字段:" + info.PropertyName + " 必须有当前版本值");
            }
            where.Append(" and ");
            where.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
            where.Append("=");
            where.Append(paramValW);
            list.AddNew(paramKeyW, info.SqlType, curValue);
        }

        #region 版本控制

        private object FillUpdateConcurrency(StringBuilder sql,
            EntityPropertyInfo info, ParamList list, object curValue, ref int index)
        {
            object newValue = NewConcurrencyValue(curValue);
            if (newValue != null)
            {
                string paramValV = EntityInfo.DBInfo.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(info.ParamName, index));
                string paramKeyV = EntityInfo.DBInfo.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(info.ParamName, index));

                sql.Append(",");
                sql.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                sql.Append("=");
                sql.Append(paramValV);
                list.AddNew(paramKeyV, info.SqlType, newValue);
                index++;
            }
            return newValue;
        }

        private object GetDefaultConcurrency(
            EntityPropertyInfo info)
        {
            DbType type = info.SqlType;
            if (type == DbType.DateTime || type == DbType.Time || type == DbType.Date)
            {
                return DateTime.Now;
            }
            else if (type == DbType.Int64 ||
               type == DbType.Decimal || type == DbType.Double || type == DbType.Int32 ||
           type == DbType.Int16 || type == DbType.Double ||
           type == DbType.SByte || type == DbType.Byte || type == DbType.Currency || type == DbType.UInt16
           || type == DbType.UInt32 || type == DbType.UInt64 || type == DbType.VarNumeric
               )
            {
                return 1;
            }
            else if (type == DbType.String) 
            {
                return CommonMethods.GuidToString(Guid.NewGuid());
            }
            else if (type == DbType.Guid)
            {
                return Guid.NewGuid();
            }
            return null;
        }



        /// <summary>
        /// 新的版本值
        /// </summary>
        /// <param name="val">当前值</param>
        /// <returns></returns>
        private object NewConcurrencyValue(object val)
        {
            Type objType = val.GetType();
            if (DefaultType.EqualType(objType, DefaultType.IntType))
            {
                return (int)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.DoubleType))
            {
                return (double)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.FloatType))
            {
                return (float)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.DateTimeType))
            {
                return DateTime.Now;
            }
            if (DefaultType.EqualType(objType, DefaultType.DecimalType))
            {
                return (decimal)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.ByteType))
            {
                return (byte)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.SbyteType))
            {
                return (sbyte)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.ShortType))
            {
                return (short)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.LongType))
            {
                return (long)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.ULongType))
            {
                return (ulong)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.UShortType))
            {
                return (ushort)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.UIntType))
            {
                return (uint)val + 1;
            }

            if (DefaultType.EqualType(objType, DefaultType.StringType))
            {
                return CommonMethods.GuidToString(Guid.NewGuid());
            }
            if (DefaultType.EqualType(objType, DefaultType.GUIDType))
            {
                return Guid.NewGuid();
            }

            return null;
        }

        #endregion



        /// <summary>
        /// 数据库链接对象
        /// </summary>
        protected internal DataBaseOperate Oper
        {
            get
            {
                return _oper;
            }
            set
            {
                _oper = value;
                _cdal = new BQLDbBase(_oper);
            }
        }

        /// <summary>
        /// 进行插入操作
        /// </summary>
        /// <param name="obj">要插入的对象</param>
        /// <param name="fillIdentity">是否要填充刚插入的实体的ID</param>
        /// <returns></returns>
        protected internal int DoInsert(EntityBase obj,ValueSetList setList, bool fillIdentity)
        {
            StringBuilder sqlParams = new StringBuilder(1000);
            StringBuilder sqlValues = new StringBuilder(1000);
            ParamList list = new ParamList();
            string param = null;
            string svalue = null;
            
            List<EntityPropertyInfo> identityInfo = new List<EntityPropertyInfo>();

            KeyWordInfomation keyinfo = BQLValueItem.GetKeyInfo().Clone() as KeyWordInfomation;
            keyinfo.ParamList = list;

            foreach (EntityPropertyInfo info in EntityInfo.PropertyInfo)
            {
                //EntityPropertyInfo info = enums.Current.Value;
                object curValue = info.GetValue(obj);
                
                if (info.Identity)
                {
                    if (info.SqlType == DbType.Guid && info.FieldType==DefaultType.GUIDType)
                    {
                        curValue = Guid.NewGuid();
                        info.SetValue(obj, curValue);

                        DBParameter prm = list.NewParameter(info.SqlType, curValue, EntityInfo.DBInfo);

                        sqlParams.Append(",");
                        sqlParams.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                        sqlValues.Append(",");
                        sqlValues.Append(prm.ValueName);
                        continue;
                    }
                    else
                    {
                        if (fillIdentity)
                        {
                            identityInfo.Add(info);
                        }
                        param = EntityInfo.DBInfo.CurrentDbAdapter.GetIdentityParamName(info);
                        if (!string.IsNullOrEmpty(param))
                        {
                            sqlParams.Append(",");
                            sqlParams.Append(param);
                        }
                        svalue = EntityInfo.DBInfo.CurrentDbAdapter.GetIdentityParamValue(EntityInfo, info);
                        if (!string.IsNullOrEmpty(svalue))
                        {
                            sqlValues.Append(",");
                            sqlValues.Append(svalue);
                        }
                        continue;
                    }


                }
                else if (info.IsVersion) //版本初始值
                {
                    object conValue = curValue;
                    if (conValue == null) 
                    {
                        conValue=GetDefaultConcurrency(info);
                    }
                    if (conValue != null)
                    {
                        DBParameter prm = list.NewParameter(info.SqlType, conValue, EntityInfo.DBInfo);

                        sqlParams.Append(",");
                        sqlParams.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                        sqlValues.Append(",");
                        sqlValues.Append(prm.ValueName);

                        continue;
                    }
                }
                else
                {
                    BQLValueItem bvalue = null;
                    if (setList != null && setList.TryGetValue(info.PropertyName, out bvalue))
                    {
                        sqlParams.Append(",");
                        sqlParams.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                        sqlValues.Append(",");
                        sqlValues.Append(bvalue.DisplayValue(keyinfo));
                        continue;
                    }
                    else if (curValue == null)
                    {
                        continue;
                    }
                    else
                    {
                        DBParameter prmValue = list.NewParameter(info.SqlType, curValue, EntityInfo.DBInfo);
                        sqlParams.Append(",");
                        sqlParams.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                        sqlValues.Append(",");
                        sqlValues.Append(prmValue.ValueName);
                    }
                }
            }
            if (sqlParams.Length > 0)
            {
                sqlParams.Remove(0, 1);
            }
            else 
            {
                return 0;
            }
            if (sqlValues.Length > 0)
            {
                sqlValues.Remove(0, 1);
            }
            else
            {
                return 0;
            }

            InsertCondition con = new InsertCondition(EntityInfo.DBInfo);
            con.Tables.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(EntityInfo.TableName));
            con.SqlParams.Append(sqlParams.ToString());
            con.SqlValues.Append(sqlValues.ToString());
            int ret = -1;
            con.DbParamList = list;

            
            using (BatchAction ba = _oper.StarBatchAction())
            {
                string sql = con.GetSql(true);
                Dictionary<string, bool> cacheTables = null;

                    cacheTables = _oper.DBInfo.QueryCache.CreateMap(EntityInfo.TableName);
                
                ret = ExecuteCommand(sql, list, CommandType.Text,cacheTables);
                if (identityInfo.Count>0 && fillIdentity)
                {
                    foreach (EntityPropertyInfo pkInfo in identityInfo)
                    {
                        sql = EntityInfo.DBInfo.CurrentDbAdapter.GetIdentitySQL(pkInfo);
                        using (IDataReader reader = _oper.Query(sql, new ParamList(),null))
                        {

                            if (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {

                                    EntityInfo.DBInfo.CurrentDbAdapter.SetObjectValueFromReader(reader, 0, obj, pkInfo, !pkInfo.TypeEqual(reader, 0));
                                    //obj.PrimaryKeyChange();
                                    ret = 1;
                                }
                            }
                        }
                    }
                }
            }
            
            return ret;
        }

        /// <summary>
        /// 删除指定数据
        /// </summary>
        /// <param name="obj">要删除的实体</param>
        /// <param name="scopeList">要删除的条件</param>
        /// <param name="isConcurrency">是否版本并发删除</param>
        /// <returns></returns>
        public int Delete(EntityBase obj, ScopeList scopeList, bool isConcurrency)
        {
            DeleteCondition con = new DeleteCondition(EntityInfo.DBInfo);
            con.Tables.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(EntityInfo.TableName));
            ParamList list = new ParamList();
            Type type = EntityInfo.EntityType;
            con.Condition.Append("1=1");

            if (obj != null)
            {
                if (scopeList == null)//通过ID删除
                {
                    scopeList = new ScopeList();
                    foreach (EntityPropertyInfo info in EntityInfo.PrimaryProperty)
                    {
                        scopeList.AddEqual(info.PropertyName, info.GetValue(obj));
                    }
                }
            }
            con.Condition.Append(DataAccessCommon.FillCondition(EntityInfo, list, scopeList));


            if (isConcurrency)
            {
                int index = 0;
                foreach (EntityPropertyInfo pInfo in EntityInfo.PropertyInfo)
                {
                    if (pInfo.IsVersion)
                    {
                        FillWhereConcurrency(con.Condition, pInfo, list, pInfo.GetValue(obj), ref index);
                    }
                }
            }
            Dictionary<string, bool> cacheTables = null;

                cacheTables = _oper.DBInfo.QueryCache.CreateMap(EntityInfo.TableName);
            
            int ret = -1;
            ret = ExecuteCommand(con.GetSql(true), list, CommandType.Text, cacheTables);

            return ret;
        }

        /// <summary>
        /// 根据ID删除记录
        /// </summary>
        /// <param name="id">要删除的记录ID</param>
        /// <returns></returns>
        public int DeleteById(object id)
        {
            int ret = -1;

            DeleteCondition con = new DeleteCondition(EntityInfo.DBInfo);
            con.Tables.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(EntityInfo.TableName));
            ParamList list = new ParamList();

            ScopeList lstScope = new ScopeList();
            PrimaryKeyInfo pkInfo = id as PrimaryKeyInfo;
            if (pkInfo == null)
            {
                lstScope.AddEqual(EntityInfo.PrimaryProperty[0].PropertyName, id);
            }
            else
            {

                pkInfo.FillScope(EntityInfo.PrimaryProperty, lstScope, true);

            }
            con.Condition.Append("1=1");
            con.Condition.Append(DataAccessCommon.FillCondition(EntityInfo, list, lstScope));
            Dictionary<string, bool> cacheTables = null;

            cacheTables = _oper.DBInfo.QueryCache.CreateMap(EntityInfo.TableName);

            ret = ExecuteCommand(con.GetSql(true), list, CommandType.Text, cacheTables);
            return ret;

        }
        #region 执行操作
        /// <summary>
        /// 执行Sql命令
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="list">参数列表</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="tables">缓存关联的表</param>
        public int ExecuteCommand(string sql, ParamList list, CommandType commandType, 
            Dictionary<string, bool> cachetables)
        {
            int ret = -1;
            ret = _oper.Execute(sql, list, commandType,cachetables);
            return ret;
        }

        /// <summary>
        /// 执行sql语句，返回DataSet
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="list">参数列表</param>
        /// <param name="commandType">语句类型</param>
        public DataSet QueryDataSet(string sql, ParamList list, CommandType commandType, Dictionary<string, bool> cachetables)
        {
            DataSet ds = null;
            ds = _oper.QueryDataSet(sql, list, commandType,cachetables);
            return ds;
        }

 
        /// <summary>
        /// 执行sql语句，分页返回DataSet(游标分页)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="objPage">分页对象</param>
        public DataSet QueryDataSet(string sql, PageContent objPage)
        {
            DataSet ds = new DataSet();
            
            DataTable retDt = EntityInfo.DBInfo.CurrentDbAdapter.QueryDataTable(sql, objPage, _oper, null);
            ds.Tables.Add(retDt);
            return ds;
        }
        /// <summary>
        /// 执行sql语句，分页返回列名用类属性映射的DataSet(游标分页)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="objPage">分页对象</param>
        protected DataSet QueryMappingDataSet(string sql, PageContent objPage)
        {
            DataSet ds = new DataSet();

            DataTable retDt = EntityInfo.DBInfo.CurrentDbAdapter.QueryDataTable(sql, objPage, _oper, EntityInfo.EntityType);
            ds.Tables.Add(retDt);
            return ds;
        }


        /// <summary>
        /// 执行sql语句，分页返回DataSet(游标分页)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="lstParam">参数集合</param>
        /// <param name="objPage">分页对象</param>
        public DataSet QueryDataSet(string sql, ParamList lstParam, PageContent objPage)
        {
            DataSet ds = new DataSet();
            DataTable retDt = EntityInfo.DBInfo.CurrentDbAdapter.QueryDataTable(sql, lstParam, objPage, _oper, null);
            ds.Tables.Add(retDt);
            return ds;
        }
        /// <summary>
        /// 执行sql语句，分页返回列名用类属性映射的DataSet(游标分页)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="lstParam">参数集合</param>
        /// <param name="objPage">分页对象</param>
        protected DataSet QueryMappingDataSet(string sql, ParamList lstParam, PageContent objPage)
        {
            DataSet ds = new DataSet();
            DataTable retDt = EntityInfo.DBInfo.CurrentDbAdapter.QueryDataTable(sql, lstParam, objPage, _oper, EntityInfo.EntityType);
            ds.Tables.Add(retDt);
            return ds;
        }
        #endregion

    }
}
