using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ
{
    /// <summary>
    /// 数据库的自释放事务类
    /// </summary>
    public class MQTransaction : IDisposable
    {
        /// <summary>
        /// 自释放事务类
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="runnow"></param>
        public MQTransaction(MQConnection oper)
        {
            _oper = oper;
            _isCommit = false;
        }


        private MQConnection _oper;
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
            if (_oper == null)
            {
                return false;
            }
            if (_isCommit)
            {
                return false;
            }
            _oper.CommitTransaction();
            _oper.AutoClose();

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
            _oper.RoolbackTransaction();
            _oper.AutoClose();
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

        ~MQTransaction()
        {
            Rollback();
        }
        #endregion
    }
}
