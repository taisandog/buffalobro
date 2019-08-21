using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Buffalo.Kernel;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.DbCommon
{
    /// <summary>
    /// ������������
    /// </summary>
    public class LockDBItem:IDisposable
    {
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="db">Ҫ���������ݿ�</param>
        /// <param name="timeout">��ʱʱ��</param>
        public LockDBItem(DBInfo db, int timeout) 
        {
            _lock = new Lock(db,timeout);
            _db = db;
        }

        /// <summary>
        /// ����
        /// </summary>
        private DBInfo _db;
        /// <summary>
        /// ����
        /// </summary>
        public DBInfo DB
        {
            get { return _db; }
        }
        /// <summary>
        /// ��
        /// </summary>
        private Lock _lock;
        /// <summary>
        /// ��
        /// </summary>
        public Lock Lock
        {
            get { return _lock; }
        }

        #region IDisposable ��Ա
        /// <summary>
        /// �ͷ���
        /// </summary>
        public void Dispose()
        {
            _lock.Dispose();
        }

        #endregion
    }
}
