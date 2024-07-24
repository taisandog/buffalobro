using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Buffalo.DB.DbCommon;
using Buffalo.DB.MessageOutPuters;

namespace Buffalo.DB.CommBase.BusinessBases
{
    /// <summary>
    /// 数据库的批量动作
    /// </summary>
    public class BatchAction : IDisposable,IAsyncDisposable
    {

        /// <summary>
        /// 自释放事务类
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="runnow"></param>
        internal BatchAction(DataBaseOperate oper) 
        {
            if (oper.CommitState == CommitState.AutoCommit)
            {
                if (oper.DBInfo.SqlOutputer.HasOutput)
                {
                    oper.OutMessage(MessageType.OtherOper, "StarBatchAction", null, "");
                }
                _state = oper.CommitState;
                oper.CommitState = CommitState.UserCommit;
                _oper = oper;
            }
        }


        private DataBaseOperate _oper;
        CommitState _state;

        /// <summary>
        /// 是否当前运行
        /// </summary>
        public bool Runnow
        {
            get { return _oper!=null; }
        }

       

        #region IDisposable 成员

       
        /// <summary>
        /// 释放事务
        /// </summary>
        public void Dispose()
        {
            EndBatch();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 结束批量操作
        /// </summary>
        private void EndBatch() 
        {
            if (_oper != null)
            {
                if (_oper.DBInfo.SqlOutputer.HasOutput)
                {
                    _oper.OutMessage(MessageType.OtherOper, "EndBatchAction", null, "");
                }
                _oper.CommitState = _state;
                _oper.AutoClose();
            }
        }
        /// <summary>
        /// 结束批量操作
        /// </summary>
        private async Task EndBatchAsync()
        {
            if (_oper != null)
            {
                if (_oper.DBInfo.SqlOutputer.HasOutput)
                {
                    _oper.OutMessage(MessageType.OtherOper, "EndBatchAction", null, "");
                }
                _oper.CommitState = _state;
                await _oper.AutoCloseAsync();
            }
        }

        public async ValueTask DisposeAsync()
        {
            await EndBatchAsync();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
