using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace MyTipText
{
    /// <summary>
    /// 子项的属性信息
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)), Serializable]
    public class TipItemInfo
    {
        private Color backColor;
        private Color fontColor;
        private Color shadowColor;
        private Color shadowFontColor;
        private string cssItem = "";
        private string cssSelectedItem = "";
        /// <summary>
        /// 子项(未选中)的背景颜色
        /// </summary>
        [Description("子项(未选中)的背景颜色"),
       Category("外观"),
        NotifyParentProperty(true)]
        public Color BackColor
        {
            get
            {
                return backColor;
            }
            set
            {
                backColor = value;
            }
        }

        /// <summary>
        /// 子项(未选中)的字体颜色
        /// </summary>
        [Description("子项(未选中)的字体颜色"),
       Category("外观"),
        NotifyParentProperty(true)]
        public Color FontColor
        {
            get
            {
                return fontColor;
            }
            set
            {
                fontColor = value;
            }
        }

        /// <summary>
        /// 子项(已选中)的背景颜色
        /// </summary>
        [Description("子项(已选中)的背景颜色"),
       Category("外观"),
        NotifyParentProperty(true)]
        public Color ShadowColor
        {
            get
            {
                return shadowColor;
            }
            set
            {
                shadowColor = value;
            }
        }

        /// <summary>
        /// 子项(已选中)的字体颜色
        /// </summary>
        [Description("子项(已选中)的字体颜色"),
       Category("外观"),
        NotifyParentProperty(true)]
        public Color ShadowFontColor
        {
            get
            {
                return shadowFontColor;
            }
            set
            {
                shadowFontColor = value;
            }
        }

        /// <summary>
        /// 子项(未选中)的CSS名
        /// </summary>
        [Description("子项(未选中)的CSS名"),
       Category("外观"),
        NotifyParentProperty(true)]
        public string ItemClassName
        {
            get
            {
                return cssItem;
            }
            set
            {
                cssItem = value;
            }
        }
        /// <summary>
        /// 子项(已选中)的CSS名
        /// </summary>
        [Description("子项(已选中)的CSS名"),
       Category("外观"),
        NotifyParentProperty(true)]
        public string SelectedClassName
        {
            get
            {
                return cssSelectedItem;
            }
            set
            {
                cssSelectedItem = value;
            }
        }
    }
}
