using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.MessageOutPuters
{
    public delegate MessageOutputBase CreateOutputerHandle(DataBaseOperate oper);

    /// <summary>
    /// ��Ϣ�����
    /// </summary>
    public class MessageOutput
    {
        public event CreateOutputerHandle OnOutputerCreate;

        /// <summary>
        /// ��Ϣ�����
        /// </summary>
        public MessageOutput() 
        {

        }

        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        internal MessageOutputBase CreateOutput(DataBaseOperate oper) 
        {
            if (OnOutputerCreate != null) 
            {
                return OnOutputerCreate(oper);
            }
            return null;
        }

        /// <summary>
        /// �ж��Ƿ��������
        /// </summary>
        public bool HasOutput 
        {
            get 
            {
                return OnOutputerCreate != null;
            }
        }
        /// <summary>
        /// �������¼�
        /// </summary>
        public void ClearOutpuHandle()
        {
            OnOutputerCreate = null;
        }


    }
}
