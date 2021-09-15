using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DBTools.HelperKernel;
using System.ComponentModel;
using Buffalo.WinFormsControl.Editors;
using Buffalo.Win32Kernel;
using EnvDTE;



namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// ��ѡ��
    /// </summary>
    public class ConfigItem
    {
        private string _name;
        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return _name;}
            set { _name = value; }
        }

        private string _summary;
        /// <summary>
        /// ˵��
        /// </summary>
        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }

        private List<ComboBoxItem> _lstItems = null;

        public List<ComboBoxItem> Items
        {
            get { return _lstItems; }
            set { _lstItems = value; }
        }
        private ConfigItemType _type;
        /// <summary>
        /// ������
        /// </summary>
        public ConfigItemType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _defaultValue;

        /// <summary>
        /// Ĭ��ֵ
        /// </summary>
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        /// <summary>
        /// ��ȡ�༭��
        /// </summary>
        /// <returns></returns>
        public EditorBase GetEditor() 
        {
            switch (Type)
            {
                case ConfigItemType.Check:
                    CheckBoxEditor cbe = new CheckBoxEditor();
                    
                    cbe.OnOffType = OnOffButtonType.Quadrate;
                    return cbe;
                case ConfigItemType.Combo:
                    
                    ComboBoxEditor cmb = new ComboBoxEditor();
                    cmb.BindValue(Items);
                    return cmb;
                case ConfigItemType.MText:
                    TextBoxEditor mtxt = new TextBoxEditor();
                    mtxt.Multiline = true;
                    mtxt.Height = 80;
                    return mtxt;
                case ConfigItemType.Number:
                    NumbericEditor ntxt = new NumbericEditor();
                    return ntxt;
                default:
                    TextBoxEditor txt = new TextBoxEditor();

                    return txt;
            }
        }

        /// <summary>
        /// ��ʽ��Ĭ�����ֵ
        /// </summary>
        /// <param name="item">����Ϣ</param>
        /// <param name="entityInfo">ʵ����Ϣ</param>
        /// <param name="selectedProject">ѡ�е���Ŀ</param>
        /// <param name="itemInfo">���������Ϣ</param>
        /// <returns></returns>
        public static string FormatDefaultValue(ConfigItem item,
            EntityInfo entityInfo, Project selectedProject, UIModelItem itemInfo)
        {
            string value = item.DefaultValue;
            value = UIConfigItem.FormatParameter(value, entityInfo, selectedProject);
            value = value.Replace("{ItemSummary}", item.Summary);
            value = value.Replace("{ItemName}", item.Name);
            if (itemInfo != null) 
            {
                value = value.Replace("{PropertyName}", itemInfo.PropertyName);
                value = value.Replace("{PropertySummary}", itemInfo.Summary);
                value = value.Replace("{PropertyTypeName}", itemInfo.TypeName);
                value = value.Replace("{PropertyTypeFullName}", itemInfo.TypeFullName);
            }
            return value;

        }

    }

    /// <summary>
    /// ������
    /// </summary>
    public enum ConfigItemType 
    {
        /// <summary>
        /// ѡ���
        /// </summary>
        [Description("ѡ���")]
        Check,
        /// <summary>
        /// ������
        /// </summary>
        [Description("������")]
        Combo,
        /// <summary>
        /// �ı���
        /// </summary>
        [Description("�ı���")]
        Text,
        /// <summary>
        /// �����ı���
        /// </summary>
        [Description("�����ı���")]
        MText,
        /// <summary>
        /// ���ֿ�
        /// </summary>
        [Description("���ֿ�")]
        Number
    }
}
