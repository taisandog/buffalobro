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
        ///  ��ʾ��
        /// </summary>
        /// <param name="text">��ʾ�ı�</param>
        public TipItem(string text) 
        {
            this.text = text;
        }

        /// <summary>
        /// ��ʾ��
        /// </summary>
        /// <param name="text">��ʾ�ı�</param>
        /// <param name="value">��ʾֵ</param>
        public TipItem(string text,string value)
        {
            this.text = text;
            this.value = value;
        }

        /// <summary>
        /// �ı�
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
        /// ֵ
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
