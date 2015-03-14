using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.Replacer
{
    public class ReplaceCollection:List<ReplaceItem>
    {
        /// <summary>
        /// 添加一个新的替换项
        /// </summary>
        /// <param name="oldString">旧字符串</param>
        /// <param name="newString">新字符串</param>
        public void Add(string oldString,string newString) 
        {
            ReplaceItem item = new ReplaceItem();
            item.OldString = oldString;
            item.NewString = newString;
            this.Add(item);
        }
    }
}
