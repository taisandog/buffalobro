using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.BQLCommon.IdentityInfos;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.PropertyAttributes;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    /// <summary>
    /// 建表的字段集合
    /// </summary>
    public class KeyWordTableParamItem : BQLQuery
    {
        private List<EntityParam> _primaryParam;

        /// <summary>
        /// 主键
        /// </summary>
        public List<EntityParam> PrimaryParam
        {
            get
            {
                if (_primaryParam == null)
                {
                    _primaryParam = new List<EntityParam>();
                    int pkValue = (int)EntityPropertyType.PrimaryKey;
                    foreach (EntityParam prm in _tparams)
                    {
                        if (EnumUnit.ContainerValue((int)prm.PropertyType, pkValue))
                        {
                            _primaryParam.Add(prm);

                        }
                    }
                }
                return _primaryParam;
            }

        }


        protected List<EntityParam> _tparams;
        /// <summary>
        /// 字段
        /// </summary>
        public List<EntityParam> Params
        {
            get { return _tparams; }
            set { _tparams = value; }
        }
        protected string _tableName;
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
        }

        private string _description;

        /// <summary>
        /// 表注释
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private bool _isView;

        /// <summary>
        /// 是否视图
        /// </summary>
        public bool IsView
        {
            get { return _isView; }
            set { _isView = value; }
        }

        private List<TableRelationAttribute> _relationItems;

        /// <summary>
        /// 关系集合
        /// </summary>
        public List<TableRelationAttribute> RelationItems
        {
            get { return _relationItems; }
            set { _relationItems = value; }
        }


        /// <summary>
        /// Insert的字段关键字项
        /// </summary>
        /// <param name="paramHandles">字段集合</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordTableParamItem(string tableName,BQLQuery previous)
            : base(previous) 
        {
            _tableName = tableName;
            this._tparams = new List<EntityParam>();
        }
        /// <summary>
        /// 表信息
        /// </summary>
        /// <param name="paramHandles">字段集合</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordTableParamItem(List<EntityParam> lstParams,List<TableRelationAttribute> relationItems, string tableName, BQLQuery previous)
            : base(previous)
        {
            _tableName = tableName;
            this._relationItems = relationItems;
            this._tparams = lstParams;
        }


        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="paramName">字段名</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="allowNull">允许空</param>
        /// <param name="type">类型</param>
        /// <param name="length">长度</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public KeyWordTableParamItem _(string paramName, DbType dbType, bool allowNull, 
            EntityPropertyType type,int length,string defaultValue)
        {
            EntityParam info = new EntityParam("",paramName,"",
                dbType, type,"", length, allowNull, false, defaultValue);
            _tparams.Add(info);
            return this;
        }

        /// <summary>
        /// 添加一个set项
        /// </summary>
        /// <param name="parameter">字段</param>
        /// <param name="valueItem">值</param>
        /// <returns></returns>
        public KeyWordTableParamItem _(List<EntityParam> lstParam)
        {
            _tparams.AddRange(lstParam);
            return this;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {

        }

        internal override void Tran(KeyWordInfomation info)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbPkstr = new StringBuilder();


            foreach (EntityParam item in _tparams)
            {
                if (item.IsPrimaryKey) 
                {
                    info.PrimaryKeys++;
                }
            }
            foreach (EntityParam item in _tparams)
            {
                sb.Append(item.DisplayInfo(info, TableName) + ",");

                if (item.IsPrimaryKey && info.PrimaryKeys > 1)
                {
                    sbPkstr.Append(info.DBInfo.CurrentDbAdapter.FormatParam(item.ParamName) + ",");
                }
            }

            
            if (sbPkstr.Length > 0)
            {
                sb.Append("PRIMARY KEY");
                sbPkstr.Remove(sbPkstr.Length - 1, 1);
                sb.Append("(" + sbPkstr.ToString() + "),");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            info.Condition.SqlParams.Append(sb.ToString());
        }
    }

    
}
