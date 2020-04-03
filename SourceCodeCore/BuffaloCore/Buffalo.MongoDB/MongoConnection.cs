using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB
{
    /// <summary>
    /// Mongo连接
    /// </summary>
    public class MongoConnection
    {
        private IClientSessionHandle _session =null;

        private IMongoDatabase _db = null;

        /// <summary>
        /// 是否支持事务
        /// </summary>
        private bool _hasTransaction;

        /// <summary>
        /// 是否支持事务
        /// </summary>
        public bool HasTransaction
        {
            get
            {
                return _hasTransaction;
            }
        }
        /// <summary>
        /// Mongo连接
        /// </summary>
        /// <param name="db"></param>
        public MongoConnection(IMongoDatabase db,bool hasTransaction)
        {
            _db = db;
            _hasTransaction = hasTransaction;
        }
        /// <summary>
        /// 连接对象
        /// </summary>
        public IMongoDatabase DB
        {
            get
            {
                return _db;
            }
        }
        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        public bool StartTransaction()
        {
            if (!_hasTransaction)
            {
                return false;
            }
            if (_session!=null)//已经有事务
            {
                return false;
            }
            
            try
            {
                _session = _db.Client.StartSession();
                _session.StartTransaction();
            }catch(Exception ex)
            {
                _session = null;
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        public bool CommitTransaction()
        {
            if (_session==null)
            {
                return false;
            }
            try
            {
                _session.CommitTransaction();
            }
            finally
            {
                _session = null;
            }
            return true;
        }
        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        public bool RollbackTransaction()
        {
            if (_session == null)
            {
                return false;
            }
            try
            {
                _session.AbortTransaction();
            }
            finally
            {
                _session = null;
            }
            return true;
        }
    }
}
