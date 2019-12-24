using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB
{
    /// <summary>
    /// 自增长
    /// </summary>
    public class MongoAutoIncrement:Attribute
    {
        /// <summary>
        /// 自增长名
        /// </summary>
        private string _name;
        /// <summary>
        /// 自增长名
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }
        /// <summary>
        /// 自增长
        /// </summary>
        /// <param name="name">自增长名</param>
        public MongoAutoIncrement(string name)
        {
            _name = name;
        }
        /// <summary>
        /// 自增长
        /// </summary>
        /// <param name="name">自增长名</param>
        public MongoAutoIncrement()
        {
        }
    }
}
