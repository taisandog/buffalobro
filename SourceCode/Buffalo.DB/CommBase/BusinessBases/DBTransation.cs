using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.CommBase.BusinessBases
{
    /// <summary>
    /// 数据库的自释放事务类
    /// </summary>
    public class DBTransaction:IDisposable
    {
        /// <summary>
        /// 自释放事务类
        /// </summary>
        /// <param name="oper"></param>
        public DBTransaction(DataBaseOperate oper) 
        {
            _oper = oper;
            _isCommit = false;
        }


        private DataBaseOperate _oper;
        private bool _isCommit;

        /// <summary>
        /// 是否当前运行
        /// </summary>
        public bool Runnow
        {
            get { return _oper != null; }
        }

        /// <summary>
        /// 是否已经提交
        /// </summary>
        public bool IsCommit
        {
            get { return _isCommit; }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        public bool Commit() 
        {
            if (_oper==null)
            {
                return false;
            }
            if (_isCommit) 
            {
                return false;
            }
            _oper.Commit();
            _oper = null;
            _isCommit = true;
            return true;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <returns></returns>
        public bool Rollback()
        {
            if (_oper == null)
            {
                return false;
            }
            if (_isCommit)
            {
                return false;
            }
            _oper.RoolBack();
            _oper = null;
            _isCommit = true;
            return true;
        }

        #region IDisposable 成员

        /// <summary>
        /// 释放事务
        /// </summary>
        public void Dispose()
        {
            Rollback();
            GC.SuppressFinalize(this);
        }

        ~DBTransaction() 
        {
            Rollback();
        }
        #endregion
    }
}
