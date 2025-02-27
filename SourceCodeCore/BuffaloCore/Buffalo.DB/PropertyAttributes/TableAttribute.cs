using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace Buffalo.DB.PropertyAttributes
{
    public class TableAttribute : System.Attribute
    {
        private string _tableName;
        private string _belongDB;



        /// <summary>
        /// 类标示
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="connectionKey">连接字符串的键</param>
        public TableAttribute(string belongDB,string tableName)
        {
            this._tableName = tableName;
            this._belongDB = belongDB;
        }
        /// <summary>
        /// 类标示
        /// </summary>
        public TableAttribute():this("","")
        {

        }

        private bool _allowLazy;

        /// <summary>
        /// 是否允许延迟加载
        /// </summary>
        public bool AllowLazy
        {
            get { return _allowLazy; }
            set { _allowLazy = value; }
        }

        private string _description;

        /// <summary>
        /// 字段注释
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// 所属库
        /// </summary>
        public string BelongDB
        {
            get { return _belongDB; }
            set { _belongDB = value; }
        }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName 
        {
            get 
            {
                return _tableName;
            }
            set 
            {
                _tableName = value;
            }
        }
        

    }
}
