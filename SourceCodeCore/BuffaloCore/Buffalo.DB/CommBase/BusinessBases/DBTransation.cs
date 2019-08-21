using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.CommBase.BusinessBases
{
    /// <summary>
    /// ���ݿ�����ͷ�������
    /// </summary>
    public class DBTransaction:IDisposable
    {
        /// <summary>
        /// ���ͷ�������
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="runnow"></param>
        public DBTransaction(DataBaseOperate oper) 
        {
            _oper = oper;
            _isCommit = false;
        }


        private DataBaseOperate _oper;
        private bool _isCommit;

        /// <summary>
        /// �Ƿ�ǰ����
        /// </summary>
        public bool Runnow
        {
            get { return _oper != null; }
        }

        /// <summary>
        /// �Ƿ��Ѿ��ύ
        /// </summary>
        public bool IsCommit
        {
            get { return _isCommit; }
        }

        /// <summary>
        /// �ύ����
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
        /// �ع�����
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

        #region IDisposable ��Ա

        /// <summary>
        /// �ͷ�����
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
