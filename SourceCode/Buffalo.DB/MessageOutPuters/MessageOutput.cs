using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.MessageOutPuters
{
    public delegate MessageOutputBase CreateOutputerHandle(DataBaseOperate oper);

    /// <summary>
    /// 信息输出类
    /// </summary>
    public class MessageOutput
    {
        public event CreateOutputerHandle OnOutputerCreate;

        /// <summary>
        /// 信息输出类
        /// </summary>
        public MessageOutput() 
        {

        }

        /// <summary>
        /// 创建输出器
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
        /// 判断是否有输出器
        /// </summary>
        public bool HasOutput 
        {
            get 
            {
                return OnOutputerCreate != null;
            }
        }
        /// <summary>
        /// 清除输出事件
        /// </summary>
        public void ClearOutpuHandle()
        {
            OnOutputerCreate = null;
        }


    }
}
