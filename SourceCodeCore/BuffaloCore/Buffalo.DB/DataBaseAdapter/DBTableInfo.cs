using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.PropertyAttributes;
using Buffalo.Kernel;

namespace Buffalo.DB.DataBaseAdapter
{
    /// <summary>
    /// 数据库的表信息
    /// </summary>
    public class DBTableInfo
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


        private string _name;

        /// <summary>
        /// 对象名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
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
        private string _description;

        /// <summary>
        /// 表注释
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
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
        protected List<EntityParam> _tparams;
        /// <summary>
        /// 字段
        /// </summary>
        public List<EntityParam> Params
        {
            get { return _tparams; }
            set { _tparams = value; }
        }
        private bool _isGenerate;
        /// <summary>
        /// 是否生成
        /// </summary>
        public bool IsGenerate
        {
            get { return _isGenerate; }
            set { _isGenerate = value; }
        }

        /// <summary>
        /// 对象类型
        /// </summary>
        public string ObjectTypeName 
        {
            get 
            {
                return _isView ? "视图" : "表";
            }
        }
    }
}
