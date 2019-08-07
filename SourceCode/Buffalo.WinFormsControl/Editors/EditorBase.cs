using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Buffalo.Kernel.Defaults;
using System.ComponentModel;
using System.Drawing;


namespace Buffalo.WinFormsControl.Editors
{
    public delegate void ValueChangeHandle(object sender,object oldValue,object newValue);
    [ToolboxItem(false)]
    public class EditorBase:UserControl
    {
        public event ValueChangeHandle OnValueChange;

        private string _bindPropertyName;
        /// <summary>
        /// �󶨵�������
        /// </summary>
        public string BindPropertyName
        {
            get { return _bindPropertyName; }
            set { _bindPropertyName = value; }
        }
        /// <summary>
        /// ��ǩ����
        /// </summary>
        public virtual string LableText
        {
            get
            {
                if (Lable == null) 
                {
                    return "";
                }
                return Lable.Text;
            }
            set
            {
                if (Lable == null)
                {
                    return;
                }
                Lable.Text = value;
            }
        }
        public virtual Font LableFont
        {
            get
            {
                if (Lable == null)
                {
                    return this.Font;
                }
                return Lable.Font;
            }
            set
            {
                if (Lable == null)
                {
                    return;
                }
                Lable.Font = value;
            }
        }
        /// <summary>
        /// ����ֵ
        /// </summary>
        public virtual void Reset()
        {

        }
        /// <summary>
        /// ��ǩ���
        /// </summary>
        public virtual int LableWidth
        {
            get { return 0; }
            set { }
        }
        public virtual Color LableForeColor
        {
            get
            {
                if (Lable == null)
                {
                    return this.ForeColor;
                }
                return Lable.ForeColor;
            }
            set
            {
                if (Lable == null)
                {
                    return ;
                }
                Lable.ForeColor = value;
            }
        }
        
        

        public virtual Label Lable
        {
            get 
            { 
                return null; 
            }
            
        }

        /// <summary>
        /// ֵ
        /// </summary>
        public virtual object Value 
        {
            get { return null; }
            set { }
        }
        private object _oldvalue;
        /// <summary>
        /// ����ֵ�ı��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected void DoValueChange(object sender, object newValue)
        {
            if (OnValueChange != null) 
            {
                object ovalue = _oldvalue;
                if (ovalue==null && newValue != null) 
                {
                    ovalue = DefaultValue.DefaultForType(newValue.GetType());
                }
                OnValueChange(sender, ovalue, newValue);
                _oldvalue = newValue;
            }
        }
        /// <summary>
        /// ����ֵ�ı��¼�
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected void DoValueChange(object newValue)
        {
            DoValueChange(this, newValue);
        }
    }
}
