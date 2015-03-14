using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace MyTipText
{
    /// <summary>
    /// 容器的信息类
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)),Serializable]
    public class ContainerInfo
    {
        private string cssContainer = "";
        private int containerHeight =0;

        /// <summary>
        /// 容器DIV的CSS名
        /// </summary>
        [Description("容器DIV的CSS名"),
       Category("外观"),
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
        /// 容器DIV的CSS
        /// </summary>
        [Description("容器的高"),
       Category("外观"),
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
