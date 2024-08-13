using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.Kernel;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using System.Collections;
using Buffalo.DB.BQLCommon;
using System.Data.Common;
using System.Threading.Tasks;

namespace Buffalo.DB.CommBase.DataAccessBases.AliasTableMappingManagers
{
    public class AliasTableMapping:IDisposable
    {
        

        private BQLAliasHandle _table;

        private Dictionary<string,BQLAliasParamHandle> _dicParams;

        private Dictionary<string, AliasTableMapping> _dicChildTables = new Dictionary<string, AliasTableMapping>();

        TableAliasNameManager _belongManager;//所属的管理器

        EntityMappingInfo _mappingInfo;//所属的关联

        private EntityInfoHandle _entityInfo;

        private Dictionary<string, EntityPropertyInfo> _dicPropertyInfo = new Dictionary<string, EntityPropertyInfo>();

        private List<AliasReaderMapping> _lstReaderMapping =null;

        private AliasReaderMapping _primaryMapping = null;

        //private Dictionary<string, EntityBase> _dicInstance = new Dictionary<string, EntityBase>();//已经实例化的实体

        //private IList _baseList;

        /// <summary>
        /// 别名映射
        /// </summary>
        /// <param name="table"></param>
        /// <param name="aliasName"></param>
        public AliasTableMapping(BQLEntityTableHandle table, TableAliasNameManager belongManager, EntityMappingInfo mappingInfo) 
        {
            _belongManager = belongManager;
            _entityInfo = table.GetEntityInfo();
            _table = new BQLAliasHandle(table, _belongManager.NextTableAliasName());
            _mappingInfo = mappingInfo;
            InitParam(table);
        }

        /// <summary>
        /// 初始化跟Reader的映射信息
        /// </summary>
        /// <param name="reader"></param>
        public void InitReaderMapping(IDataReader reader) 
        {
            _lstReaderMapping = new List<AliasReaderMapping>(_dicPropertyInfo.Count);
            int fCount=reader.FieldCount;
            EntityPropertyInfo info=null;
            for (int i = 0; i < fCount; i++) 
            {
                string colName = reader.GetName(i);

                if (_dicPropertyInfo.TryGetValue(colName, out info)) 
                {
                    AliasReaderMapping aliasMapping = new AliasReaderMapping(i, info, !info.TypeEqual(reader,i));
                    if (aliasMapping != null)
                    {
                        _lstReaderMapping.Add(aliasMapping);
                        if (info.IsPrimaryKey)
                        {
                            _primaryMapping = aliasMapping;
                        }
                    }
                }
            }

            foreach (KeyValuePair<string, AliasTableMapping> keyPair in _dicChildTables)
            {
                keyPair.Value.InitReaderMapping(reader);
            }

            //_baseList = new ArrayList();
        }

        /// <summary>
        /// 读取信息
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public EntityBase LoadFromReader(IDataReader reader)
        {

            IDBAdapter dbAdapter = _entityInfo.DBInfo.CurrentDbAdapter;

            EntityBase objRet = null;
            
            objRet = _entityInfo.CreateSelectProxyInstance() as EntityBase;

            foreach (AliasReaderMapping readMapping in _lstReaderMapping)
            {
                int index = readMapping.ReaderIndex;
                EntityPropertyInfo info = readMapping.PropertyInfo;
                if (!reader.IsDBNull(index))
                {
                    if (info.IsPrimaryKey) 
                    {
                        objRet.GetEntityBaseInfo().HasPKValue = true;
                    }
                    dbAdapter.SetObjectValueFromReader(reader, index, objRet, info, readMapping.NeedChangeType);
                }
            }
            foreach (KeyValuePair<string, AliasTableMapping> keyPair in _dicChildTables)//写入父表属性
            {
                AliasTableMapping childMapping = keyPair.Value;
                EntityBase child = childMapping.LoadFromReader(reader);
                if (childMapping.MappingInfo.IsParent)
                {
                    if (child.GetEntityBaseInfo().HasPKValue)
                    {
                        childMapping.MappingInfo.SetValue(objRet, child);
                    }
                    objRet.GetEntityBaseInfo()._dicFilledParent___[childMapping.MappingInfo.PropertyName] = true;
                }
               

            }
            return objRet;
        }

        /// <summary>
        /// 读取信息
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public async Task<EntityBase> LoadFromReaderAsync(DbDataReader reader)
        {

            IDBAdapter dbAdapter = _entityInfo.DBInfo.CurrentDbAdapter;

            EntityBase objRet = null;

            objRet = _entityInfo.CreateSelectProxyInstance() as EntityBase;

            foreach (AliasReaderMapping readMapping in _lstReaderMapping)
            {
                int index = readMapping.ReaderIndex;
                EntityPropertyInfo info = readMapping.PropertyInfo;
                if (!(await reader.IsDBNullAsync(index)))
                {
                    if (info.IsPrimaryKey)
                    {
                        objRet.GetEntityBaseInfo().HasPKValue = true;
                    }
                    await dbAdapter.SetObjectValueFromReaderAsync(reader, index, objRet, info, readMapping.NeedChangeType);
                }
            }
            foreach (KeyValuePair<string, AliasTableMapping> keyPair in _dicChildTables)//写入父表属性
            {
                AliasTableMapping childMapping = keyPair.Value;
                EntityBase child =await childMapping.LoadFromReaderAsync(reader);
                if (childMapping.MappingInfo.IsParent)
                {
                    if (child.GetEntityBaseInfo().HasPKValue)
                    {
                        childMapping.MappingInfo.SetValue(objRet, child);
                    }
                    objRet.GetEntityBaseInfo()._dicFilledParent___[childMapping.MappingInfo.PropertyName] = true;
                }


            }
            return objRet;
        }


        /// <summary>
        /// 实体信息
        /// </summary>
        /// <returns></returns>
        public EntityInfoHandle EntityInfo 
        {
            get 
            {
                return _entityInfo;
            }
        }

        /// <summary>
        /// 所有字段
        /// </summary>
        public Dictionary<string, AliasTableMapping> ChildTables
        {
            get 
            {

                return _dicChildTables;
            }
        }
        /// <summary>
        /// 映射信息
        /// </summary>
        public EntityMappingInfo MappingInfo 
        {
            get 
            {
                return _mappingInfo;
            }
        }
        /// <summary>
        /// 获取字段别名信息
        /// </summary>
        /// <param name="propertyName">所属的属性名</param>
        /// <returns></returns>
        public List<BQLParamHandle> GetParamInfo(string propertyName) 
        {
            List<BQLParamHandle> lstRet = new List<BQLParamHandle>();

            if (propertyName != "*")
            {
                BQLAliasParamHandle handle = null;
                if (_dicParams.TryGetValue(propertyName, out handle))
                {
                    lstRet.Add(handle);
                }
            }
            else 
            {
                foreach (KeyValuePair<string, BQLAliasParamHandle> keyPair in _dicParams) 
                {
                    lstRet.Add(keyPair.Value);
                }
            }
            return lstRet;
        }

        /// <summary>
        /// 表信息
        /// </summary>
        public BQLAliasHandle TableInfo
        {
            get 
            {
                return _table;
            }
        }

        /// <summary>
        /// 添加子表
        /// </summary>
        /// <param name="table">子表</param>
        public AliasTableMapping AddChildTable(BQLEntityTableHandle table) 
        {
            AliasTableMapping retTable = null;
            Stack<BQLEntityTableHandle> stkTables = new Stack<BQLEntityTableHandle>();
            BQLEntityTableHandle curTable=table;
            do
            {
                stkTables.Push(curTable);
                curTable = curTable.GetParentTable();
            } while (!CommonMethods.IsNull(curTable));

            AliasTableMapping lastTable = null;//上一个表
            while (stkTables.Count > 0) 
            {
                BQLEntityTableHandle cTable=stkTables.Pop();
                
                string pName = cTable.GetPropertyName();
                if (string.IsNullOrEmpty(pName))
                {

                    lastTable = this;
                    retTable = this;
                }
                else
                {
                    if (!lastTable._dicChildTables.ContainsKey(pName))
                    {
                        EntityInfoHandle entityInfo = retTable.EntityInfo;
                        EntityMappingInfo mapInfo = entityInfo.MappingInfo[pName];
                        if (mapInfo != null)
                        {
                            retTable = new AliasTableMapping(cTable, _belongManager, mapInfo);
                            lastTable._dicChildTables[pName] = retTable;
                            lastTable = retTable;
                        }
                        else
                        {
                            throw new MissingMemberException("实体:" + entityInfo.EntityType.FullName + "中找不到属性:" + pName + "");
                        }

                    }
                    else 
                    {
                        retTable = lastTable._dicChildTables[pName];
                        lastTable = retTable;
                    }

                }
                
            }
            return retTable;
        }

       
        /// <summary>
        /// 获取别名字段
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public BQLParamHandle GetAliasParam(string propertyName) 
        {
            BQLAliasParamHandle prm = null;
            BQLParamHandle ret = null;
            if (_dicParams.TryGetValue(propertyName, out prm))
            {
                ret = BQL.ToParam(prm.AliasName);
                ret.ValueDbType = prm.ValueDbType;
            }
            return ret;
        }

        /// <summary>
        /// 初始化字段
        /// </summary>
        /// <param name="table"></param>
        /// <param name="paramIndex"></param>
        private void InitParam(BQLEntityTableHandle table) 
        {
            _dicParams = new Dictionary<string, BQLAliasParamHandle>();

            foreach (EntityPropertyInfo info in table.GetEntityInfo().PropertyInfo) 
            {
                string prmAliasName=_belongManager.NextParamAliasName(_table.GetAliasName());
                BQLAliasParamHandle prm = BQL.Tables[_table.GetAliasName()][info.ParamName].As(prmAliasName);

                
                _dicPropertyInfo[prmAliasName] = info;
                prm.ValueDbType = info.SqlType;
                _dicParams[info.PropertyName]=prm;
            }
        }

        public void Dispose()
        {
            _belongManager = null;
            _entityInfo = null;
            _dicChildTables = null;
            _dicParams = null;
            _dicPropertyInfo = null;
            _entityInfo = null;
            _lstReaderMapping = null;
            _mappingInfo = null;
            _primaryMapping = null;
            _table = null;
        }
    }
}
