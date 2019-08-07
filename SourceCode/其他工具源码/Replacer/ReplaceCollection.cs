using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.Replacer
{
    public class ReplaceCollection:List<ReplaceItem>
    {
        /// <summary>
        /// ���һ���µ��滻��
        /// </summary>
        /// <param name="oldString">���ַ���</param>
        /// <param name="newString">���ַ���</param>
        public void Add(string oldString,string newString) 
        {
            ReplaceItem item = new ReplaceItem();
            item.OldString = oldString;
            item.NewString = newString;
            this.Add(item);
        }
    }
}
