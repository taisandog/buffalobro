using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB
{
    /// <summary>
    /// 更新
    /// </summary>
    public class UpdateList<TDocument> : List<UpdateDefinition<TDocument>>
    {
        /// <summary>
        /// 条件生成器
        /// </summary>
        public static UpdateDefinitionBuilder<TDocument> Update
        {
            get
            {
                return Builders<TDocument>.Update;
            }
        }
    }
}
