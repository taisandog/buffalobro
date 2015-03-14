using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel;
namespace MyTipText
{
    [Serializable]
    public class TipItemCollection:List<TipItem>
    {
        /// <summary>
        /// 添加一个列表对象
        /// </summary>
        /// <param name="text"></param>
        public void Add(string text) 
        {
            TipItem item = new TipItem(text,text);
            this.Add(item);
        }
        /// <summary>
        /// 添加一个列表对象
        /// </summary>
        /// <param name="text"></param>
        public void Add(string text,string value)
        {
            TipItem item = new TipItem(text, value);
            this.Add(item);
        }
    }
}
