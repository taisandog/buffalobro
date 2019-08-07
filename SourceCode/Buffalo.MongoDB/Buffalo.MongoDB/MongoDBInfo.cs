using Buffalo.Kernel;
using Buffalo.Kernel.FastReflection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB
{
    /// <summary>
    /// 数据库连接器
    /// </summary>
    public class MongoDBInfo
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private string _connectionString = null;
        /// <summary>
        ///  连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            
        }
        /// <summary>
        /// 数据库名
        /// </summary>
        private string _dbName = null;

        /// <summary>
        ///  连接字符串
        /// </summary>
        public string DBName
        {
            get
            {
                return _dbName;
            }
            
        }

        private string _dbKey;
        /// <summary>
        ///  本数据库配置标记
        /// </summary>
        public string DBKey
        {
            get
            {
                return _dbKey;
            }

        }
        /// <summary>
        /// mongoDB连接器
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="connName">此链接的标识</param>
        public MongoDBInfo(string dbKey,string connectionString, string dbName, MongoLiquidUnit liquid)
        {
            _dbKey = dbKey;
            _liquid = liquid;
            _connectionString = connectionString;
            _dbName = dbName;
            
        }
        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        public IMongoDatabase CreateConnection()
        {
            MongoClient client = new MongoClient(_connectionString);
            IMongoDatabase db = client.GetDatabase(_dbName);
            return db;
        }

        

        /// <summary>
        /// 自增长ID
        /// </summary>
        private MongoLiquidUnit _liquid;
        /// <summary>
        ///  自增长ID
        /// </summary>
        public MongoLiquidUnit Liquid
        {
            get
            {
                return _liquid;
            }
            set
            {
                _liquid = value;
            }
        }
    }
}
