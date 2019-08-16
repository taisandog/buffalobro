using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// ��Ǳ�ǩ����
    /// </summary>
    public class TagManager
    {
        private Stack<string> _stk = new Stack<string>();

        /// <summary>
        /// ��ӱ�ǩ
        /// </summary>
        /// <param name="tabName">�����</param>
        public void AddTag(string tabName) 
        {
            _stk.Push(tabName);
        }

        /// <summary>
        /// ������ǩ
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
        /// ��ǰ��ǩ
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
