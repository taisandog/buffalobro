using System;
using System.Collections.Generic;
using System.Text;

namespace MyTipText
{
    [Serializable]
    public class TipItem
    {
        private string text;
        private string value;

        /// <summary>
        ///  提示项
        /// </summary>
        /// <param name="text">提示文本</param>
        public TipItem(string text) 
        {
            this.text = text;
        }

        /// <summary>
        /// 提示项
        /// </summary>
        /// <param name="text">提示文本</param>
        /// <param name="value">提示值</param>
        public TipItem(string text,string value)
        {
            this.text = text;
            this.value = value;
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text 
        {
            get 
            {
                return text;
            }
            set 
            {
                text = value;
            }
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Value 
        {
            get 
            {
                return value;
            }
            set 
            {
                this.value = value;
            }
        }
    }
}
