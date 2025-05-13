using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
/** 
@author 
@version 创建时间：2011-12-1
显示类图注释
*/
namespace Buffalo.DBTools.DocSummary
{
    internal class MemberItem
    {
        // Fields
        private string _ColName;
        private string _IndexName;
        public bool AdvanceProp;
        public bool AllowedEdit;
        public bool AllowedShow;
        public bool AllowedSubauery;
        public string Cascade = "none";
        public string ChirldKey = "";
        public List<string> ChirldKeys;
        public string ClassAss = "";
        public int ColLength = 0xff;
        public bool Computecol;
        public bool EditShow;
        public bool IsMain;
        public bool Lazy = true;
        public int MaxValue;
        public int MinValue;
        public bool NeedInsert = true;
        public bool NeedUpdate = true;
        public bool Not_Null;
        public bool NotAlloedEmp;
        public bool NotAllowedSame;
        public string PropDescribe = "";
        public bool QuickSearch;
        public int RowWidth = 80;
        public bool ShowDefault;
        public string ShowFoot = "";
        public bool ShowMoneyLine;
        public string ShowName = "";
        public string ShowTag = "";
        private Member source;
        public bool UseToQuery;
        public bool UseToQueryDefault;

        // Methods
        public MemberItem(Member tm, XmlNode XmlFile, XmlNode DHSetting)
        {
            this.source = tm;
            if (XmlFile != null)
            {
                try
                {
                    if (XmlFile.Attributes["column"] != null)
                    {
                        this.ColName = XmlFile.Attributes["column"].Value;
                    }
                    if (XmlFile.Attributes["update"] != null)
                    {
                        this.NeedUpdate = bool.Parse(XmlFile.Attributes["update"].Value);
                    }
                    if (XmlFile.Attributes["insert"] != null)
                    {
                        this.NeedInsert = bool.Parse(XmlFile.Attributes["insert"].Value);
                    }
                    if (XmlFile.Attributes["not-null"] != null)
                    {
                        this.Not_Null = bool.Parse(XmlFile.Attributes["not-null"].Value);
                    }
                    if (XmlFile.Attributes["length"] != null)
                    {
                        this.ColLength = int.Parse(XmlFile.Attributes["length"].Value);
                        if (tm.MemberTypeLookupName == "System.String")
                        {
                            this.MaxValue = this.ColLength;
                        }
                        else
                        {
                            this.MaxValue = 0x989680;
                        }
                    }
                    if (XmlFile.Attributes["cascade"] != null)
                    {
                        this.Cascade = XmlFile.Attributes["cascade"].Value;
                    }
                    if (XmlFile.Attributes["index"] != null)
                    {
                        this.IndexName = XmlFile.Attributes["index"].Value;
                    }
                    if (XmlFile.Attributes["lazy"] != null)
                    {
                        if (XmlFile.Name == "many-to-one")
                        {
                            if (XmlFile.Attributes["lazy"].Value.ToLower() == "proxy")
                            {
                                this.Lazy = true;
                            }
                            else
                            {
                                this.Lazy = false;
                            }
                        }
                        else
                        {
                            this.Lazy = bool.Parse(XmlFile.Attributes["lazy"].Value);
                        }
                    }
                    if (XmlFile.Name == "id")
                    {
                        this.IsMain = true;
                        this.Not_Null = true;
                    }
                    if (XmlFile["key"] != null)
                    {
                        XmlElement element = XmlFile["key"];
                        if ((element.Attributes["column"] != null) && (element.Attributes["column"].Value != ""))
                        {
                            this.ChirldKey = element.Attributes["column"].Value;
                        }
                    }
                    if (XmlFile.Attributes["formula"] != null)
                    {
                        this.ColName = XmlFile.Attributes["formula"].Value;
                        this.Computecol = true;
                    }
                }
                catch
                {
                }
            }
            this.ShowName = this.source.DocSummary;
            this.ShowFoot = "None";
            if (DHSetting != null)
            {
                this.UseToQuery = this.TryGetNodeAtt(DHSetting, "UseToQuery", "False") == "True";
                this.UseToQueryDefault = this.TryGetNodeAtt(DHSetting, "UseToQueryDefault", "False") == "True";
                this.AllowedSubauery = this.TryGetNodeAtt(DHSetting, "AllowedSubauery", "False") == "True";
                this.AllowedShow = this.TryGetNodeAtt(DHSetting, "AllowedShow", "False") == "True";
                this.ShowDefault = this.TryGetNodeAtt(DHSetting, "ShowDefault", "False") == "True";
                this.ShowName = this.TryGetNodeAtt(DHSetting, "ShowName", this.source.DocSummary);
                this.ShowFoot = this.TryGetNodeAtt(DHSetting, "ShowFoot", "None");
                this.ShowTag = this.TryGetNodeAtt(DHSetting, "ShowTag", "");
                this.AllowedEdit = this.TryGetNodeAtt(DHSetting, "AllowedEdit", "False") == "True";
                this.NotAllowedSame = this.TryGetNodeAtt(DHSetting, "NotAllowedSame", "False") == "True";
                this.NotAlloedEmp = this.TryGetNodeAtt(DHSetting, "NotAlloedEmp", "False") == "True";
                this.EditShow = this.TryGetNodeAtt(DHSetting, "EditShow", "False") == "True";
                this.MaxValue = int.Parse(this.TryGetNodeAtt(DHSetting, "MaxValue", "0"));
                this.MinValue = int.Parse(this.TryGetNodeAtt(DHSetting, "MinValue", "0"));
                this.RowWidth = int.Parse(this.TryGetNodeAtt(DHSetting, "RowWidth", "80"));
                this.PropDescribe = this.TryGetNodeAtt(DHSetting, "PropDescribe", this.ShowName);
                this.AdvanceProp = this.TryGetNodeAtt(DHSetting, "AdvanceProp", "False") == "True";
                this.QuickSearch = this.TryGetNodeAtt(DHSetting, "QuickSearch", "False") == "True";
                this.ShowMoneyLine = this.TryGetNodeAtt(DHSetting, "ShowMoneyLine", "False") == "True";
            }
            this.ChirldKeys = new List<string>();
        }

        public XmlNode ToProperty(XmlDocument doc)
        {
            XmlNode node;
            if (!this.IsMain)
            {
                if (this.IsMany_one)
                {
                    node = doc.CreateNode(XmlNodeType.Element, "many-to-one", "");
                }
                else if (this.IsOne_Many)
                {
                    node = doc.CreateNode(XmlNodeType.Element, "set", "");
                    XmlElement newChild = doc.CreateElement("key");
                    XmlAttribute attribute = doc.CreateAttribute("column");
                    attribute.Value = this.ChirldKey;
                    newChild.Attributes.Append(attribute);
                    XmlElement element2 = doc.CreateElement("one-to-many");
                    XmlAttribute attribute2 = doc.CreateAttribute("class");
                    attribute2.Value = this.ClassAss;
                    element2.Attributes.Append(attribute2);
                    node.AppendChild(newChild);
                    node.AppendChild(element2);
                }
                else
                {
                    node = doc.CreateNode(XmlNodeType.Element, "property", "");
                }
            }
            else
            {
                node = doc.CreateNode(XmlNodeType.Element, "id", "");
                XmlElement element3 = doc.CreateElement("generator");
                XmlAttribute attribute3 = doc.CreateAttribute("class");
                attribute3.Value = "native";
                element3.Attributes.Append(attribute3);
                node.AppendChild(element3);
            }
            XmlAttribute attribute4 = doc.CreateAttribute("lazy");
            if (this.IsOne_Many)
            {
                attribute4.Value = this.Lazy.ToString().ToLower();
            }
            else if (this.Lazy)
            {
                attribute4.Value = "proxy";
            }
            else
            {
                attribute4.Value = "false";
            }
            XmlAttribute attribute5 = doc.CreateAttribute("cascade");
            attribute5.Value = this.Cascade;
            XmlAttribute attribute6 = doc.CreateAttribute("index");
            attribute6.Value = this.IndexName;
            XmlAttribute attribute7 = doc.CreateAttribute("name");
            attribute7.Value = this.PropName;
            XmlAttribute attribute8 = doc.CreateAttribute("column");
            attribute8.Value = this.ColName;
            XmlAttribute attribute9 = doc.CreateAttribute("type");
            attribute9.Value = this.ColType;
            XmlAttribute attribute10 = doc.CreateAttribute("update");
            attribute10.Value = this.NeedUpdate.ToString().ToLower();
            XmlAttribute attribute11 = doc.CreateAttribute("insert");
            attribute11.Value = this.NeedInsert.ToString().ToLower();
            XmlAttribute attribute12 = doc.CreateAttribute("not-null");
            attribute12.Value = this.Not_Null.ToString().ToLower();
            XmlAttribute attribute13 = doc.CreateAttribute("length");
            attribute13.Value = this.ColLength.ToString();
            node.Attributes.Append(attribute7);
            if (this.Computecol)
            {
                XmlAttribute attribute14 = doc.CreateAttribute("formula");
                attribute14.Value = this.ColName;
                node.Attributes.Append(attribute14);
                return node;
            }
            if (!this.IsMany_one && !this.IsOne_Many)
            {
                if (this.source.MemberType is ClrEnumeration)
                {
                    attribute9.Value = attribute9.Value + "," + this.source.MemberType.OwnerNamespace.VSProject.AccessibleName;
                }
                node.Attributes.Append(attribute9);
            }
            if (this.IndexName != null)
            {
                node.Attributes.Append(attribute6);
            }
            if (!this.IsMain && !this.IsOne_Many)
            {
                node.Attributes.Append(attribute12);
            }
            if (!this.IsOne_Many)
            {
                if (this.ColName != this.PropName)
                {
                    node.Attributes.Append(attribute8);
                }
                if (!this.NeedUpdate)
                {
                    node.Attributes.Append(attribute10);
                }
                if (!this.NeedInsert)
                {
                    node.Attributes.Append(attribute11);
                }
            }
            if (this.ColType == "System.String")
            {
                node.Attributes.Append(attribute13);
            }
            if (this.IsMany_one || this.IsOne_Many)
            {
                node.Attributes.Append(attribute5);
                node.Attributes.Append(attribute4);
            }
            return node;
        }

        public override string ToString()
        {
            return string.Format("{4}{0}:{1}-{3}[{2}]", new object[] { this.source.SignaturePersistent, this.source.MemberTypeShortName, this.ColName, this.source.DocSummary, this.IsMain ? "*" : "" });
        }

        private string TryGetNodeAtt(XmlNode node, string Attname, string Errres)
        {
            if (node == null)
            {
                return Errres;
            }
            if (node.Attributes[Attname] == null)
            {
                return Errres;
            }
            return node.Attributes[Attname].Value;
        }

        // Properties
        public string ChirldClass
        {
            get
            {
                if (this.source.TypeArguments == "")
                {
                    return "";
                }
                return this.source.TypeArguments.Replace("<", "").Replace(">", "");
            }
        }

        public string ColName
        {
            get
            {
                if (this._ColName == null)
                {
                    this._ColName = this.PropName;
                }
                return this._ColName;
            }
            set
            {
                this._ColName = value;
            }
        }

        public string ColType
        {
            get
            {
                if (this.source.MemberTypeName.IndexOf("[]") != -1)
                {
                    return (this.source.MemberTypeLookupName + "[]");
                }
                return this.source.MemberTypeLookupName;
            }
        }

        public string IndexName
        {
            get
            {
                return this._IndexName;
            }
            set
            {
                if ((value != null) && (value.Trim() == ""))
                {
                    value = null;
                }
                this._IndexName = value;
            }
        }

        public bool IsMany_one
        {
            get
            {
                switch (this.source.MemberTypeLookupName)
                {
                    case "System.Char":
                    case "System.Boolean":
                    case "System.Byte":
                    case "System.DateTime":
                    case "System.Decimal":
                    case "System.Double":
                    case "System.Guid":
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Single":
                    case "System.TimeSpan":
                    case "System.String":
                        return false;
                }
                if (this.source.MemberType is ClrEnumeration)
                {
                    return false;
                }
                if (this.source.TypeArguments != "")
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsOne_Many
        {
            get
            {
                return (this.source.TypeArguments != "");
            }
        }

        public string MenberSummary
        {
            get
            {
                return this.source.DocSummary;
            }
        }

        public ClrType MenberType
        {
            get
            {
                return this.source.MemberType;
            }
        }

        public string MenberTypeString
        {
            get
            {
                return this.source.MemberTypeName;
            }
        }

        public string PropName
        {
            get
            {
                return this.source.SignaturePersistent;
            }
        }
    }

 

}
