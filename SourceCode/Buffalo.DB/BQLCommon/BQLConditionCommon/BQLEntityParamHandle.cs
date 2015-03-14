using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class BQLEntityParamHandle:BQLParamHandle
    {
        private EntityInfoHandle entityInfo;
        private EntityPropertyInfo pinfo;
        private BQLEntityTableHandle _belongTable;//父表

        
        /// <summary>
        /// 实体属性信息
        /// </summary>
        /// <param name="entityInfo">实体信息</param>
        /// <param name="propertyName">属性名</param>
        internal BQLEntityParamHandle(EntityInfoHandle entityInfo, string propertyName, BQLEntityTableHandle belongTable)
        {
            this.entityInfo = entityInfo;
            if (propertyName != "*")
            {
                pinfo = entityInfo.PropertyInfo[propertyName];
                if (pinfo == null) 
                {
                    throw new MissingMemberException(entityInfo.EntityType.FullName + "类中不包含属性:" + propertyName);
                }
                this._valueDbType = pinfo.SqlType;
            }
            _belongTable = belongTable;
            
        }
        /// <summary>
        /// 所属的表
        /// </summary>
        public BQLEntityTableHandle BelongEntity
        {
            get { return _belongTable; }
        }
        /// <summary>
        /// 实体属性信息
        /// </summary>
        internal EntityInfoHandle EntityInfo
        {
            get
            {
                return entityInfo;
            }
        }
        /// <summary>
        /// 属性信息
        /// </summary>
        internal EntityPropertyInfo PInfo
        {
            get
            {
                return pinfo;
            }
        }

        /// <summary>
        /// 输出用于填充DataSet的字段信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        internal string DisplayDataSetValue(KeyWordInfomation info)
        {
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            StringBuilder sbRet = new StringBuilder();

            string tableName = null;
            if (!CommonMethods.IsNull(_belongTable))
            {
                if (info.AliasManager != null)
                {
                    string aliasName = info.AliasManager.GetTableAliasName(_belongTable);
                    if (!string.IsNullOrEmpty(aliasName))
                    {
                        tableName = aliasName;
                    }
                }
                else
                {
                    tableName = _belongTable.DisplayValue(info);
                }
            }
            else
            {
                tableName = entityInfo.TableName;
            }

            if (pinfo == null)//查询全部字段时候
            {

                foreach (EntityPropertyInfo eInfo in entityInfo.PropertyInfo)
                {
                    if (info.Infos.IsShowTableName)
                    {

                        sbRet.Append(idba.FormatTableName(tableName));
                    }
                    sbRet.Append(idba.FormatParam(eInfo.ParamName) + " as " + idba.FormatParam(eInfo.PropertyName) + ",");
                }
                if (sbRet.Length > 0)
                {
                    sbRet.Remove(sbRet.Length - 1, 1);
                }
            }
            else
            {
                if (info.Infos.IsShowTableName)
                {
                    sbRet.Append(idba.FormatTableName(tableName));
                    sbRet.Append(".");
                }
                sbRet.Append(idba.FormatParam(pinfo.ParamName) + " as " + idba.FormatParam(pinfo.PropertyName));
            }
            return sbRet.ToString();
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            //if (info.Infos.IsPutPropertyName && !info.IsWhere)
            //{
            //    return DisplayDataSetValue(info);
            //}
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            StringBuilder sbRet = new StringBuilder();
            BQLEntityTableHandle outputTable = _belongTable;
            if (string.IsNullOrEmpty(entityInfo.TableName)) 
            {
                outputTable = info.FromTable;
            }
            if (info.Infos.IsShowTableName)
            {
                if (info.AliasManager != null && !CommonMethods.IsNull(outputTable))
                {
                    
                    string aliasName = info.AliasManager.GetTableAliasName(outputTable);
                    if (!string.IsNullOrEmpty(aliasName))
                    {
                        sbRet.Append(idba.FormatTableName(aliasName));
                        sbRet.Append(".");
                    }
                }
                else if(!string.IsNullOrEmpty(entityInfo.TableName))
                {
                    sbRet.Append(idba.FormatTableName(entityInfo.TableName));
                    sbRet.Append(".");
                }
            }

            if (pinfo == null)//查询全部字段时候
            {
                sbRet.Append("*");
            }
            else
            {
                sbRet.Append(idba.FormatParam(pinfo.ParamName));
            }
            return sbRet.ToString();
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
            if (info.AliasManager != null)
            {
                BQLValueItem.DoFillInfo(_belongTable, info);
            }
        }
    }
}
