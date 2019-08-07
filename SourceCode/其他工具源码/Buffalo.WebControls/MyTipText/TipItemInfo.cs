using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace MyTipText
{
    /// <summary>
    /// �����������Ϣ
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
        /// ����(δѡ��)�ı�����ɫ
        /// </summary>
        [Description("����(δѡ��)�ı�����ɫ"),
       Category("���"),
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
        /// ����(δѡ��)��������ɫ
        /// </summary>
        [Description("����(δѡ��)��������ɫ"),
       Category("���"),
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
        /// ����(��ѡ��)�ı�����ɫ
        /// </summary>
        [Description("����(��ѡ��)�ı�����ɫ"),
       Category("���"),
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
        /// ����(��ѡ��)��������ɫ
        /// </summary>
        [Description("����(��ѡ��)��������ɫ"),
       Category("���"),
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
        /// ����(δѡ��)��CSS��
        /// </summary>
        [Description("����(δѡ��)��CSS��"),
       Category("���"),
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
        /// ����(��ѡ��)��CSS��
        /// </summary>
        [Description("����(��ѡ��)��CSS��"),
       Category("���"),
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
