using Buffalo.Kernel;
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
    public class MongoDBConnect
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
        /// <summary>
        /// mongoDB连接器
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="connName">此链接的标识</param>
        public MongoDBConnect(string connectionString, string dbName)
        {
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
    }
}
