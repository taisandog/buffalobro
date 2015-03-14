using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.PropertyAttributes
{
    /// <summary>
    /// 数据库标识
    /// </summary>
    public class DataBaseAttribute : System.Attribute
    {
        private string _dbName;
        public string DataBaseName 
        {
            get 
            {
                return _dbName;
            }
        }

        public DataBaseAttribute(string dbName) 
        {
            _dbName = dbName;
        }
    }
}
