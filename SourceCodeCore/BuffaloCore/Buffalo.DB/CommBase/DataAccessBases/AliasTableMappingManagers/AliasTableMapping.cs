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

        TableAliasNameManager _belongManager;//�����Ĺ�����

        EntityMappingInfo _mappingInfo;//�����Ĺ���

        private EntityInfoHandle _entityInfo;

        private Dictionary<string, EntityPropertyInfo> _dicPropertyInfo = new Dictionary<string, EntityPropertyInfo>();

        private List<AliasReaderMapping> _lstReaderMapping =null;

        private AliasReaderMapping _primaryMapping = null;

        //private Dictionary<string, EntityBase> _dicInstance = new Dictionary<string, EntityBase>();//�Ѿ�ʵ������ʵ��

        //private IList _baseList;

        /// <summary>
        /// ����ӳ��
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
        /// ��ʼ����Reader��ӳ����Ϣ
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
        /// ��ȡ��Ϣ
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
            foreach (KeyValuePair<string, AliasTableMapping> keyPair in _dicChildTables)//д�븸������
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
        /// ��ȡ��Ϣ
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
            foreach (KeyValuePair<string, AliasTableMapping> keyPair in _dicChildTables)//д�븸������
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
        /// ʵ����Ϣ
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
        /// �����ֶ�
        /// </summary>
        public Dictionary<string, AliasTableMapping> ChildTables
        {
            get 
            {

                return _dicChildTables;
            }
        }
        /// <summary>
        /// ӳ����Ϣ
        /// </summary>
        public EntityMappingInfo MappingInfo 
        {
            get 
            {
                return _mappingInfo;
            }
        }
        /// <summary>
        /// ��ȡ�ֶα�����Ϣ
        /// </summary>
        /// <param name="propertyName">������������</param>
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
        /// ����Ϣ
        /// </summary>
        public BQLAliasHandle TableInfo
        {
            get 
            {
                return _table;
            }
        }

        /// <summary>
        /// ����ӱ�
        /// </summary>
        /// <param name="table">�ӱ�</param>
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

            AliasTableMapping lastTable = null;//��һ����
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
                            throw new MissingMemberException("ʵ��:" + entityInfo.EntityType.FullName + "���Ҳ�������:" + pName + "");
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
        /// ��ȡ�����ֶ�
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
        /// ��ʼ���ֶ�
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
