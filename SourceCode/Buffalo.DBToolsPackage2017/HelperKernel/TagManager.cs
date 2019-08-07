using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// 标记标签管理
    /// </summary>
    public class TagManager
    {
        private Stack<string> _stk = new Stack<string>();

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="tabName">标记名</param>
        public void AddTag(string tabName) 
        {
            _stk.Push(tabName);
        }

        /// <summary>
        /// 弹出标签
        /// </summary>
        /// <returns></returns>
        public string PopTag() 
        {
            if (_stk.Count > 0) 
            {
                return _stk.Pop();
            }
            return null;
        }

        /// <summary>
        /// 当前标签
        /// </summary>
        /// <returns></returns>
        public string CurrentTag
        {
            get
            {
                if (_stk.Count > 0)
                {
                    return _stk.Peek();
                }
                return null ;
            }
        }

    }
}
