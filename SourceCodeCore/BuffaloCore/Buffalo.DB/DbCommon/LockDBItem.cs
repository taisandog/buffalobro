using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Buffalo.Kernel;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.DbCommon
{
    /// <summary>
    /// 锁定的连接项
    /// </summary>
    public class LockDBItem:IDisposable
    {
        /// <summary>
        /// 锁定的连接项
        /// </summary>
        /// <param name="db">要锁定的数据库</param>
        /// <param name="timeout">超时时间</param>
        public LockDBItem(DBInfo db, int timeout) 
        {
            _lock = new Lock(db,timeout);
            _db = db;
        }

        /// <summary>
        /// 连接
        /// </summary>
        private DBInfo _db;
        /// <summary>
        /// 连接
        /// </summary>
        public DBInfo DB
        {
            get { return _db; }
        }
        /// <summary>
        /// 锁
        /// </summary>
        private Lock _lock;
        /// <summary>
        /// 锁
        /// </summary>
        public Lock Lock
        {
            get { return _lock; }
        }

        #region IDisposable 成员
        /// <summary>
        /// 释放锁
        /// </summary>
        public void Dispose()
        {
            _lock.Dispose();
        }

        #endregion
    }
}
