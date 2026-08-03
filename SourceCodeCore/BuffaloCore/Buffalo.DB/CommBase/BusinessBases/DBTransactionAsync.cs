using System;
using System.Threading.Tasks;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.CommBase.BusinessBases
{
    /// <summary>
    /// 数据库的异步自释放事务类
    /// </summary>
    public class DBTransactionAsync : IAsyncDisposable
    {
        private DataBaseOperate _oper;
        private bool _isCommit;

        /// <summary>
        /// 异步自释放事务类
        /// </summary>
        /// <param name="oper">数据库操作对象</param>
        public DBTransactionAsync(DataBaseOperate oper)
        {
            _oper = oper;
            _isCommit = false;
        }

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
        /// 异步提交事务
        /// </summary>
        /// <returns>是否提交成功</returns>
        public async Task<bool> CommitAsync()
        {
            if (_oper == null || _isCommit)
            {
                return false;
            }
            bool ret = await _oper.CommitAsync();
            _oper = null;
            _isCommit = true;
            return ret;
        }

        /// <summary>
        /// 异步回滚事务
        /// </summary>
        /// <returns>是否回滚成功</returns>
        public async Task<bool> RollbackAsync()
        {
            if (_oper == null || _isCommit)
            {
                return false;
            }
            await _oper.RoolBackAsync();
            _oper = null;
            _isCommit = true;
            return true;
        }

        /// <summary>
        /// 异步释放事务，未提交时自动回滚
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            await RollbackAsync();
            GC.SuppressFinalize(this);
        }
    }
}
