using System;
using System.Threading.Tasks;
using Buffalo.DB.DbCommon;
using Buffalo.DB.MessageOutPuters;

namespace Buffalo.DB.CommBase.BusinessBases
{
    /// <summary>
    /// 数据库的异步批量动作
    /// </summary>
    public class BatchActionAsync : IAsyncDisposable
    {
        private DataBaseOperate _oper;
        private CommitState _state;

        private BatchActionAsync()
        {
        }

        /// <summary>
        /// 创建异步批量动作
        /// </summary>
        /// <param name="oper">数据库操作对象</param>
        internal static async Task<BatchActionAsync> CreateAsync(DataBaseOperate oper)
        {
            BatchActionAsync action = new BatchActionAsync();
            if (oper.CommitState == CommitState.AutoCommit)
            {
                if (oper.DBInfo.SqlOutputer.HasOutput)
                {
                    await oper.OutMessageAsync(MessageType.OtherOper, "StarBatchAction", null, "");
                }
                action._state = oper.CommitState;
                oper.CommitState = CommitState.UserCommit;
                action._oper = oper;
            }
            return action;
        }

        /// <summary>
        /// 是否当前运行
        /// </summary>
        public bool Runnow
        {
            get { return _oper != null; }
        }

        /// <summary>
        /// 异步结束批量操作
        /// </summary>
        private async Task EndBatchAsync()
        {
            if (_oper != null)
            {
                if (_oper.DBInfo.SqlOutputer.HasOutput)
                {
                    await _oper.OutMessageAsync(MessageType.OtherOper, "EndBatchAction", null, "");
                }
                _oper.CommitState = _state;
                await _oper.AutoCloseAsync();
            }
        }

        /// <summary>
        /// 异步释放批量动作
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            await EndBatchAsync();
            GC.SuppressFinalize(this);
        }
    }
}
