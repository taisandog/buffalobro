using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using Buffalo.DB.MessageOutPuters;

namespace Buffalo.DB.CommBase.BusinessBases
{
    /// <summary>
    /// ���ݿ����������
    /// </summary>
    public class BatchAction : IDisposable
    {

        /// <summary>
        /// ���ͷ�������
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
        /// �Ƿ�ǰ����
        /// </summary>
        public bool Runnow
        {
            get { return _oper!=null; }
        }

       

        #region IDisposable ��Ա

        /// <summary>
        /// �ͷ�����
        /// </summary>
        public void Dispose()
        {
            EndBatch();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// ������������
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
        #endregion
    }
}
