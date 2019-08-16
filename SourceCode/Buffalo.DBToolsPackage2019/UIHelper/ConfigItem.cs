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
    /// 可选项
    /// </summary>
    public class ConfigItem
    {
        private string _name;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get { return _name;}
            set { _name = value; }
        }

        private string _summary;
        /// <summary>
        /// 说明
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
        /// 项类型
        /// </summary>
        public ConfigItemType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _defaultValue;

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        /// <summary>
        /// 获取编辑框
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
        /// 格式化默认项的值
        /// </summary>
        /// <param name="item">项信息</param>
        /// <param name="entityInfo">实体信息</param>
        /// <param name="selectedProject">选中的项目</param>
        /// <param name="itemInfo">项的属性信息</param>
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
    /// 项类型
    /// </summary>
    public enum ConfigItemType 
    {
        /// <summary>
        /// 选择框
        /// </summary>
        [Description("选择框")]
        Check,
        /// <summary>
        /// 下拉框
        /// </summary>
        [Description("下拉框")]
        Combo,
        /// <summary>
        /// 文本框
        /// </summary>
        [Description("文本框")]
        Text,
        /// <summary>
        /// 多行文本框
        /// </summary>
        [Description("多行文本框")]
        MText,
        /// <summary>
        /// 数字框
        /// </summary>
        [Description("数字框")]
        Number
    }
}
