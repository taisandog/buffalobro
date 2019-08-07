using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
using Buffalo.Kernel;
using System.Data;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.DB.CommBase.DataAccessBases.AliasTableMappingManagers
{
    /// <summary>
    /// 表别名管理
    /// </summary>
    public class TableAliasNameManager
    {

        int _tableIndex = 0;

        int _paramIndex = 0;


        AliasTableMapping _primaryTable;//主表

        //List<KeyWordJoinItem> _lstJoin = new List<KeyWordJoinItem>();//关联的表
        private Dictionary<string, AliasTableMapping> _dicKeyTable = new Dictionary<string, AliasTableMapping>();


        //private bool _isInitReader = false;//是否已经初始化过Reader

        /// <summary>
        /// 初始化映射
        /// </summary>
        public void InitMapping(IDataReader reader) 
        {
            _primaryTable.InitReaderMapping(reader);
        }

        /// <summary>
        /// 从Reader读取
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hasValue"></param>
        /// <returns></returns>
        public object LoadFromReader(IDataReader reader) 
        {


            return _primaryTable.LoadFromReader(reader);
        }

        public TableAliasNameManager(BQLEntityTableHandle pEntityinfo) 
        {
            _primaryTable = new AliasTableMapping(pEntityinfo, this,null);
            string key = pEntityinfo.GetEntityKey();
            _dicKeyTable[key] = _primaryTable;
            
        }

        /// <summary>
        /// 添加子表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool AddChildTable(BQLEntityTableHandle table) 
        {
            string key=table.GetEntityKey();
            bool ret=false;
            if (!_dicKeyTable.ContainsKey(key))
            {
                _dicKeyTable[key] = _primaryTable.AddChildTable(table);
                ret = true;
            }
            return ret;
        }

        
        /// <summary>
        /// 获取主表的别名信息
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public BQLAliasHandle GetPrimaryAliasHandle(BQLTableHandle table) 
        {
            BQLEntityTableHandle eHandle = table as BQLEntityTableHandle;
            if (Buffalo.Kernel.CommonMethods.IsNull( eHandle)) 
            {
                return null;
            }
            if (eHandle.GetEntityInfo() == _primaryTable.EntityInfo) 
            {
                return _primaryTable.TableInfo;
            }
            return null;
        }

        /// <summary>
        /// 获取别名字段
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public List<BQLParamHandle> GetPrimaryAliasParamHandle(BQLParamHandle[] handles) 
        {
            List<BQLParamHandle> lst=new List<BQLParamHandle>();
            bool hasOther = false;//是否有不是指定的字段
            foreach (BQLParamHandle handle in handles)
            {
                BQLEntityParamHandle pHandle = handle as BQLEntityParamHandle;
                if (CommonMethods.IsNull(pHandle))
                {
                    lst.Add(handle);
                    hasOther = true;
                    continue;
                }
                string pName = null;
                if (pHandle.PInfo == null)
                {
                    pName = "*";
                }
                else
                {
                    pName = pHandle.PInfo.PropertyName;
                }
                lst.AddRange(_primaryTable.GetParamInfo(pName));
            }
            if (!hasOther)
            {
                LoadChildParams(_primaryTable, lst);
            }
            return lst;
        }

        /// <summary>
        /// 获取子类的字段信息
        /// </summary>
        /// <param name="table"></param>
        private void LoadChildParams(AliasTableMapping table,List<BQLParamHandle> lst) 
        {
            foreach (KeyValuePair<string,AliasTableMapping> cTableMapping in table.ChildTables) 
            {
                AliasTableMapping cTable=cTableMapping.Value;
                lst.AddRange(cTable.GetParamInfo("*"));
                if (cTable.ChildTables.Count > 0) 
                {
                    LoadChildParams(cTable, lst);
                }
            }
        }

        /// <summary>
        /// 给表查询加上关联表
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public KeyWordFromItem ToInnerTable(KeyWordFromItem from, KeyWordInfomation info) 
        {
            KeyWordFromItem inner = InnerTable(from, _primaryTable);
            return inner;
        }

        /// <summary>
        /// 关联表
        /// </summary>
        /// <param name="from"></param>
        /// <param name="sourceTable"></param>
        /// <param name="targetTable"></param>
        /// <returns></returns>
        private KeyWordFromItem InnerTable(KeyWordFromItem from, AliasTableMapping sourceTable) 
        {
            KeyWordFromItem inner = from;
            string sTableName = sourceTable.TableInfo.GetAliasName();
            foreach (KeyValuePair<string, AliasTableMapping> tablePair in sourceTable.ChildTables)
            {
                AliasTableMapping tTable = tablePair.Value;
                string tTableName = tTable.TableInfo.GetAliasName();
                EntityMappingInfo minfo = tTable.MappingInfo;
                BQLCondition fhandle = null;
                //if (minfo.IsPrimary)
                //{
                fhandle = BQL.Tables[sTableName][minfo.SourceProperty.ParamName] == BQL.Tables[tTableName][minfo.TargetProperty.ParamName];
                //}
                //else
                //{
                //    fhandle = BQLDbBase.BQL.Tables[sTableName][minfo.SourceProperty.ParamName] == BQLDbBase.BQL.Tables[tTableName][minfo.PrimaryKey];
                //}
                inner = inner.LeftJoin(tablePair.Value.TableInfo, fhandle);
                if (tTable.ChildTables.Count > 0)
                {
                    inner = InnerTable(inner, tTable);
                }
            }
            return inner;
        }

        /// <summary>
        /// 输出别名表
        /// </summary>
        /// <param name="paramHandle"></param>
        /// <returns></returns>
        public string GetTableAliasName(BQLEntityTableHandle table) 
        {
            AliasTableMapping mapping = FindMapping(table);
            if (mapping != null) 
            {
                return mapping.TableInfo.GetAliasName();
            }
            return null;
        }

        /// <summary>
        /// 查找所属的表映射信息
        /// </summary>
        /// <returns></returns>
        private AliasTableMapping FindMapping(BQLEntityTableHandle table) 
        {
            AliasTableMapping ret = null;
            string key=table.GetEntityKey();
            _dicKeyTable.TryGetValue(key, out ret);
            return ret;
        }

        /// <summary>
        /// 下一个表的别名
        /// </summary>
        /// <param name="tableIndex">表索引</param>
        /// <returns></returns>
        internal string NextTableAliasName()
        {
            _tableIndex++;
            StringBuilder sb = new StringBuilder(5);
            sb.Append("t");
            sb.Append(_tableIndex.ToString("X"));
            sb.Append("_");
            return sb.ToString();
        }

        /// <summary>
        /// 下一个字段的别名
        /// </summary>
        /// <param name="tableIndex">表索引</param>
        /// <returns></returns>
        internal string NextParamAliasName(string tableName)
        {
            _paramIndex++;
            StringBuilder sb = new StringBuilder(5);
            sb.Append(tableName);
            sb.Append("p");
            sb.Append(_paramIndex.ToString("X"));
            sb.Append("_");
            return sb.ToString();
        }
    }
}

//NHibernate: SELECT top 1 this_.ID as ID54_3_, this_.DataStatus as DataStatus54_3_,
//this_.LastUpdate as LastUpdate54_3_, this_.Auth as Auth54_3_, this_.Duty as Duty54_3_, 
//this_.ForClass as ForClass54_3_, this_.WorkPlace as WorkPlace54_3_, this_.Employee as Employee54_3_,
//this_.LogTime as LogTime54_3_, forclass2_.ID as ID61_0_, forclass2_.DataStatus as DataStatus61_0_, 
//forclass2_.LastUpdate as LastUpdate61_0_, forclass2_.UpdateEmp as UpdateEmp61_0_, forclass2_.IsSys as IsSys61_0_,
//forclass2_.DataVersion as DataVers6_61_0_, forclass2_.IsEnabled as IsEnabled61_0_,
//forclass2_.ForDate as ForDate61_0_, forclass2_.IsFinished as IsFinished61_0_, 
//forclass2_.FinishedEmployee as Finishe10_61_0_, forclass2_.ClassIndex as ClassIndex61_0_, 
//forclass2_.StartTime as StartTime61_0_, forclass2_.EndTime as EndTime61_0_, forclass2_.CanUse as CanUse61_0_,
//workplace3_.ID as ID56_1_, workplace3_.DataStatus as DataStatus56_1_, workplace3_.LastUpdate as LastUpdate56_1_, 
//workplace3_.UpdateEmp as UpdateEmp56_1_, workplace3_.IsSys as IsSys56_1_, workplace3_.DataVersion as DataVers6_56_1_,
//workplace3_.IsEnabled as IsEnabled56_1_, workplace3_.PlaceName as PlaceName56_1_, 
//workplace3_.BindMachine as BindMach9_56_1_, workplace3_.BindStock as BindStock56_1_, 
//workplace3_.IsNeedCheck as IsNeedC11_56_1_, workplace3_.ClientGUID as ClientGUID56_1_,
//workplace3_.PlaceSetting as PlaceSe13_56_1_, employee1_.ID as ID20_2_, employee1_.DataStatus as DataStatus20_2_, 
//employee1_.LastUpdate as LastUpdate20_2_, employee1_.UpdateEmp as UpdateEmp20_2_, employee1_.IsSys as IsSys20_2_, 
//employee1_.DataVersion as DataVers6_20_2_, employee1_.IsEnabled as IsEnabled20_2_, employee1_.LogName as LogName20_2_, employee1_.Password as Password20_2_, 
//employee1_.RealName as RealName20_2_, employee1_.IDCard as IDCard20_2_, employee1_.RegTime as RegTime20_2_, 
//employee1_.Email as Email20_2_, employee1_.Phone as Phone20_2_, employee1_.Summary as Summary20_2_, 
//employee1_.CardID as CardID20_2_ 

//FROM 
//GP_Duty this_ 
//inner join GP_Class forclass2_ on this_.ForClass=forclass2_.ID 
//inner join GP_WorkPlace workplace3_ on this_.WorkPlace=workplace3_.ID 
//inner join GP_Employee employee1_ on this_.Employee=employee1_.ID 
//WHERE employee1_.ID = @p0 and forclass2_.ID = @p1 and 
//workplace3_.ID = @p2 and this_.DataStatus = @p3

//;@p0 = 2, @p1 = 1, @p2 = 2, @p3 = 0
