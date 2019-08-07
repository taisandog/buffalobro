using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace MyTipText
{
    /// <summary>
    /// ��������Ϣ��
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)),Serializable]
    public class ContainerInfo
    {
        private string cssContainer = "";
        private int containerHeight =0;

        /// <summary>
        /// ����DIV��CSS��
        /// </summary>
        [Description("����DIV��CSS��"),
       Category("���"),
        NotifyParentProperty(true)]
        public string ClassName
        {
            get
            {
                return cssContainer;
            }
            set
            {
                cssContainer = value;
            }
        }

        /// <summary>
        /// ����DIV��CSS
        /// </summary>
        [Description("�����ĸ�"),
       Category("���"),
        NotifyParentProperty(true)]
        public int ContainerHeight
        {
            get
            {
                return containerHeight;
            }
            set
            {
                containerHeight = value;
            }
        }
    }
}
