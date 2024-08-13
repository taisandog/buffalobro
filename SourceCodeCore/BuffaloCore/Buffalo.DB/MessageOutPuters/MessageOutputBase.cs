using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.DB.MessageOutPuters
{
    /// <summary>
    /// 输出器基类
    /// </summary>
    public abstract class MessageOutputBase
    {
        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="messType">信息类型</param>
        /// <param name="mess">信息</param>
        public abstract void OutPut(MessageType messType, MessageInfo mess);
        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="messType">信息类型</param>
        /// <param name="mess">信息</param>
        public abstract Task OutPutAsync(MessageType messType, MessageInfo mess);

        private bool _showBinary;
        /// <summary>
        /// 输出SQL时候是否输出二进制变量值的Hex
        /// </summary>
        public virtual bool ShowBinary
        {
            get { return _showBinary; }
            set { _showBinary = value; }
        }
        private int _hideTextLength;
        /// <summary>
        /// 输出SQL时候设置一个值，当字符串大于这个长度时候则隐藏值
        /// </summary>
        public virtual int HideTextLength
        {
            get { return _hideTextLength; }
            set { _hideTextLength = value; }
            
        }

        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="messType">消息类型</param>
        /// <param name="type">类型</param>
        /// <param name="extendType">扩展类型</param>
        /// <param name="value">值</param>
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
        /// 输出信息
        /// </summary>
        /// <param name="messType">消息类型</param>
        /// <param name="type">类型</param>
        /// <param name="extendType">扩展类型</param>
        /// <param name="value">值</param>
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
