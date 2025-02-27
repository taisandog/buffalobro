using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MongoDB
{
    /// <summary>
    /// 事务
    /// </summary>
    public class MongoTransaction:IDisposable
    {
        private MongoConnection _connection = null;

        /// <summary>
        /// 已完成
        /// </summary>
        private bool _isFinish = false;
        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="conn"></param>
        public MongoTransaction(MongoConnection conn)
        {
            _connection = conn;
            _isFinish = false;
        }

        /// <summary>
        /// 回滚
        /// </summary>
        /// <returns></returns>
        public bool Rollback()
        {
            if (_connection == null)
            {
                return true;
            }
            bool ret=_connection.RollbackTransaction();
            _isFinish = true;
            return ret;
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            if (_connection == null)
            {
                return true;
            }
            bool ret = _connection.CommitTransaction();
            _isFinish = true;
            return ret;
        }
        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            if(_connection==null || _isFinish)
            {
                return;
            }
            bool ret = _connection.RollbackTransaction();
            _isFinish = true;
        }
    }
}
