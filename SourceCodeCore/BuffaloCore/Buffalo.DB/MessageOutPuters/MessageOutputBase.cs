using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.DB.MessageOutPuters
{
    /// <summary>
    /// ���������
    /// </summary>
    public abstract class MessageOutputBase
    {
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="messType">��Ϣ����</param>
        /// <param name="mess">��Ϣ</param>
        public abstract void OutPut(MessageType messType, MessageInfo mess);
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="messType">��Ϣ����</param>
        /// <param name="mess">��Ϣ</param>
        public abstract Task OutPutAsync(MessageType messType, MessageInfo mess);

        private bool _showBinary;
        /// <summary>
        /// ���SQLʱ���Ƿ���������Ʊ���ֵ��Hex
        /// </summary>
        public virtual bool ShowBinary
        {
            get { return _showBinary; }
            set { _showBinary = value; }
        }
        private int _hideTextLength;
        /// <summary>
        /// ���SQLʱ������һ��ֵ�����ַ��������������ʱ��������ֵ
        /// </summary>
        public virtual int HideTextLength
        {
            get { return _hideTextLength; }
            set { _hideTextLength = value; }
            
        }

        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="messType">��Ϣ����</param>
        /// <param name="type">����</param>
        /// <param name="extendType">��չ����</param>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public void OutPut(MessageType messType, string type, string extendType, string value)
        {
            MessageInfo mess = new MessageInfo();
            mess.Type = type;
            if (extendType != null)
            {
                mess.ExtendType = extendType;
            }
            mess.Value = value;
            OutPut(messType, mess);
        }
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="messType">��Ϣ����</param>
        /// <param name="type">����</param>
        /// <param name="extendType">��չ����</param>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public async Task OutPutAsync(MessageType messType, string type, string extendType, string value)
        {
            MessageInfo mess = new MessageInfo();
            mess.Type = type;
            if (extendType != null)
            {
                mess.ExtendType = extendType;
            }
            mess.Value = value;
            await OutPutAsync(messType, mess);
        }
    }
}
