using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DBTools.HelperKernel;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using System.Xml;
using Buffalo.GeneratorInfo;
using Buffalo.WinFormsControl.Editors;
using EnvDTE;
using Buffalo.Kernel;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// UI需要的信息
    /// </summary>
    public class UIModelItem
    {
        private ClrProperty _belongProperty;

        /// <summary>
        /// UI信息
        /// </summary>
        /// <param name="dicCheckItem">选中项</param>
        /// <param name="belongProperty">所属属性</param>
        public UIModelItem(ClrProperty belongProperty) 
        {
            _belongProperty = belongProperty;
        }
        /// <summary>
        /// UI信息
        /// </summary>
        public UIModelItem()
        {
        }
        private RelationInfo _relInfo;
        /// <summary>
        /// 关联信息
        /// </summary>
        public RelationInfo RelInfo
        {
            get { return _relInfo; }
            set { _relInfo = value; }
        }

        private TableInfo _tabInfo;
        /// <summary>
        /// 表信息
        /// </summary>
        public TableInfo TabInfo
        {
            get { return _tabInfo; }
            set { _tabInfo = value; }
        }

        /// <summary>
        /// 初始化默认值
        /// </summary>
        /// <param name="collection">配置项</param>
        /// <param name="entityInfo">实体信息</param>
        /// <param name="selectedProject">选择项</param>
        /// <param name="itemInfo">所属的属性信息</param>
        public void InitDefaultValue(IEnumerable<ConfigItem> collection,
            EntityInfo entityInfo, Project selectedProject,UIModelItem itemInfo) 
        {
            foreach (ConfigItem citem in collection) 
            {
                string dvalue = citem.DefaultValue;
                if (!string.IsNullOrEmpty(dvalue))
                {
                    dvalue = ConfigItem.FormatDefaultValue(citem, entityInfo, selectedProject,itemInfo);
                    object value=ConvertValue(dvalue,citem);
                    _dicCheckItem[citem.Name] = value;
                }
            }
        }

        private Dictionary<string, object> _dicCheckItem=new Dictionary<string,object>();

        /// <summary>
        /// 选中的项
        /// </summary>
        internal Dictionary<string, object> CheckItem 
        {
            get 
            {
                return _dicCheckItem;
            }
        }

        /// <summary>
        /// 获取是否选中项
        /// </summary>
        /// <param name="itemName">项名称</param>
        /// <returns></returns>
        public bool HasItem(string itemName) 
        {
            object ret = false;
            if (_dicCheckItem.TryGetValue(itemName, out ret)) 
            {
                if(ret is bool)
                {
                    return (bool)ret;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public string GetValue(string itemName) 
        {
            object ret = false;
            if (_dicCheckItem.TryGetValue(itemName, out ret))
            {
                return ret.ToString();
            }
            return null;
        }
        private bool _isGenerate;
        /// <summary>
        /// 是否生成
        /// </summary>
        public bool IsGenerate
        {
            get { return _isGenerate; }
            set { _isGenerate = value; }
        }

        /// <summary>
        /// 写到XML节点
        /// </summary>
        /// <param name="node"></param>
        public void WriteNode(XmlNode node) 
        {
            XmlDocument doc=node.OwnerDocument;
            XmlAttribute att = doc.CreateAttribute("name");
            if (_belongProperty != null)
            {
                att.InnerText = PropertyName;
                node.Attributes.Append(att);
            }
            att = doc.CreateAttribute("isgen");

            att.InnerText = IsGenerate?"1":"0";
            node.Attributes.Append(att);
            

            foreach (KeyValuePair<string, object> kvp in _dicCheckItem) 
            {
                XmlNode inode = doc.CreateElement("item");
                att = doc.CreateAttribute("name");
                att.InnerText = kvp.Key;
                inode.Attributes.Append(att);

                string value = null;
                if (kvp.Value is bool)
                {
                    value = (bool)kvp.Value ? "1" : "0";
                }
                else 
                {
                    value = kvp.Value as string;
                }
                att = doc.CreateAttribute("value");
                att.InnerText = value;
                inode.Attributes.Append(att);
                node.AppendChild(inode);
            }

        }

        /// <summary>
        /// 读取节点信息
        /// </summary>
        /// <param name="node">节点</param>
        public void ReadItem(XmlNode node,Dictionary<string, ConfigItem> dicConfig) 
        {
            string name=null;
            object value=null;
            ConfigItem item = null;
            foreach (XmlNode cnode in node.ChildNodes)
            {
                XmlAttribute att = cnode.Attributes["name"];
                if (att == null)
                {
                    continue;
                }

                name = att.InnerText;

                

                att = cnode.Attributes["value"];
                if (att != null)
                {
                    if (dicConfig.TryGetValue(name, out item))
                    {
                        value = ConvertValue(att.InnerText, item);
                        _dicCheckItem[name] = value;
                    }
                }
            }
        }

        /// <summary>
        /// 转换值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private static object ConvertValue(string value, ConfigItem item) 
        {
            object ret = value;
            if (item.Type == ConfigItemType.Check)
            {
                bool bvalue = false;
                if (!bool.TryParse(value, out bvalue))
                {
                    bvalue = value == "1";
                }
                ret = bvalue;
            }
            return ret;
        }
        /// <summary>
        /// 注释
        /// </summary>
        public string Summary
        {
            get { return _belongProperty.DocSummary; }
        }
        /// <summary>
        /// 类型名
        /// </summary>
        public string TypeName
        {
            get { return _belongProperty.MemberTypeName; }
        }
        /// <summary>
        /// 类型的短名
        /// </summary>
        public string TypeShortName
        {
            get { return _belongProperty.MemberTypeShortName; }
        }
        /// <summary>
        /// 类型的完整名
        /// </summary>
        public string TypeFullName 
        {
            get { return _belongProperty.MemberTypeLookupName; }
        }

        /// <summary>
        /// 对应的属性名
        /// </summary>
        public string PropertyName
        {
            get { return _belongProperty.Name; }
        }



        /// <summary>
        /// 输出成GeneratItem类
        /// </summary>
        /// <returns></returns>
        public Buffalo.GeneratorInfo.Property ToGeneratItem() 
        {
            Buffalo.GeneratorInfo.Property item = null;
            if (_belongProperty == null)
            {
                
                item = new Buffalo.GeneratorInfo.Property(_dicCheckItem, "", "", "", "",null,null);
            }
            else
            {
                item = new Buffalo.GeneratorInfo.Property(_dicCheckItem, TypeFullName,
                    Summary, TypeName, PropertyName, _tabInfo, _relInfo);
            }
            return item;
        }
    }
}
