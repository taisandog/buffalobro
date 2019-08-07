using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using Buffalo.Win32Kernel;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// 实体字段基类
    /// </summary>
    public class EntityFieldBase
    {
        protected CodeElementPosition _cp = null;
        protected ClrField _fInfo = null;

        public ClrField FInfo
        {
            get { return _fInfo; }
        }
        protected string _propertyName = null;
        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }
        protected string _defaultValue = null;
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }
        /// <summary>
        /// 对应的字段名
        /// </summary>
        public string FieldName
        {
            get { return _fInfo.Name; }
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
        protected EntityConfig _belongEntity;
        /// <summary>
        /// 所属实体
        /// </summary>
        public EntityConfig BelongEntity
        {
            get { return _belongEntity; }
        }
        /// <summary>
        /// 注释
        /// </summary>
        public string Summary
        {
            get { return _fInfo.DocSummary; }
        }
        /// <summary>
        /// 开始行
        /// </summary>
        public virtual int StarLine
        {
            get { return _cp.StartLine; }
        }
        /// <summary>
        /// 结束行
        /// </summary>
        public virtual int EndLine
        {
            get { return _cp.EndLine; }
        }
        /// <summary>
        /// 类型名
        /// </summary>
        public virtual string TypeName
        {
            get { return _fInfo.MemberTypeShortName; }
        }
        /// <summary>
        /// 类型全名
        /// </summary>
        public virtual string TypeFullName
        {
            get { return _fInfo.MemberTypeName; }
        }
        /// <summary>
        /// 把名字转成帕斯卡命名法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="replaceSpace"></param>
        /// <returns></returns>
        public static string ToPascalName(string name,bool replaceSpace) 
        {
            string propertyName = name.TrimStart('_');
            propertyName = propertyName.Substring(0, 1).ToUpper() + propertyName.Substring(1, propertyName.Length - 1);
            if (replaceSpace) 
            {
                propertyName = ReplaceBlock(propertyName);
            }
            return propertyName;
        }
        /// <summary>
        /// 把名字转成帕斯卡命名法
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToPascalName(string name)
        {

            return ToPascalName(name,true);
        }
        /// <summary>
        /// 把名字转成骆驼命名法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="replaceSpace"></param>
        /// <returns></returns>
        public static string ToCamelName(string name, bool replaceSpace)
        {
            string camelName = name.TrimStart('_');
            camelName = camelName.Substring(0, 1).ToLower() + camelName.Substring(1, camelName.Length - 1);
            if (replaceSpace)
            {
                camelName = ReplaceBlock(camelName);
            }
            return camelName;
        }

        /// <summary>
        /// 过滤无效字符
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        private static string ReplaceBlock(string name) 
        {
            string retName = name.Replace(" ", "_");
            retName = retName.Replace(":", "");
            retName = retName.Replace("&", "");
            retName = retName.Replace("=", "");
            retName = retName.Replace("*", "");
            return retName;
        }
        /// <summary>
        /// 把名字转成骆驼命名法
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToCamelName(string name)
        {
            return ToCamelName(name,true);
        }
        private static Dictionary<string, DataTypeInfos> _dicTypeInfos = InitTypeInfos();
        
        private static string[] _allTypes={"Guid","float","float?","Single","Single?","bool","bool?","Boolean","Boolean?",
        "DateTime","decimal","decimal?","Decimal","Decimal?","double","double?","Double","Double?","byte","byte?","Byte","Byte?",
        "sbyte","sbyte?","SByte","SByte?","short","short?","int","int?","Int32","Int32?","long","long?","Int64","Int64?",
        "ushort","ushort?","UInt16","UInt16?","uint","uint?","UInt32","UInt32?","ulong","ulong?","UInt64","UInt64?",
         "string","String","Byte[]","byte[]"
        };

        /// <summary>
        /// 所有支持类型的集合
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllSupportTypes()
        {
            return _allTypes;
        }

        /// <summary>
        /// 初始化类型信息
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, DataTypeInfos> InitTypeInfos() 
        {
            
            Dictionary<string, DataTypeInfos> dic = new Dictionary<string, DataTypeInfos>();
            DataTypeInfos info = new DataTypeInfos("Guid", new DbType[]{DbType.Guid},"DefaultValue.DefaultGuidValue", "null", 0);
            dic["Guid"] = info;

            info = new DataTypeInfos("float", new DbType[] { DbType.Single, DbType.Currency, DbType.Decimal,DbType.Double },
                "DefaultValue.DefaultFloat", "DefaultValue.DefaultFloatValue", 0);
            dic["float"] = info;
            //info = new DataTypeInfos("Single", new DbType[] { DbType.Single, DbType.Currency, DbType.Decimal }, "DefaultValue.DefaultFloatValue", false);
            dic["Single"] = info;

            info = new DataTypeInfos("bool", new DbType[] { 
                DbType.Boolean, DbType.Int16, DbType.Int32,
                DbType.Int64, DbType.SByte, DbType.UInt16,
                DbType.UInt32, DbType.UInt64 }, 
                "false","DefaultValue.DefaultBooleanValue", 0);
            dic["bool"] = info;
            //info = new DataTypeInfos("Boolean", "Boolean", "DefaultValue.DefaultBooleanValue", false);
            dic["Boolean"] = info;

            info = new DataTypeInfos("DateTime", new DbType[] { DbType.DateTime, DbType.DateTime2, DbType.DateTimeOffset, DbType.Date ,DbType.Time},
                "DefaultValue.DefaultDateTimeValue", "null", 0);
            dic["DateTime"] = info;

            info = new DataTypeInfos("decimal", new DbType[] { DbType.Decimal,DbType.Single, DbType.Currency , DbType.Double }, 
                "DefaultValue.DefaultDecimal","DefaultValue.DefaultDecimalValue", 0);
            dic["decimal"] = info;
            dic["Decimal"] = info;

            info = new DataTypeInfos("double", new DbType[] { DbType.Double,DbType.Single, DbType.Currency, DbType.Decimal }, 
                "DefaultValue.DefaultDouble","DefaultValue.DefaultDoubleValue", 0);
            dic["double"] = info;
            dic["Double"] = info;

            info = new DataTypeInfos("short", new DbType[] { DbType.Int16,DbType.SByte }, 
                "DefaultValue.DefaultShort","DefaultValue.DefaultShortValue", 0);
            dic["short"] = info;
            dic["Int16"] = info;
            
            info = new DataTypeInfos("int", new DbType[] {DbType.Int32,DbType.Int16, DbType.SByte},
                "DefaultValue.DefaultInt","DefaultValue.DefaultIntValue", 0);
            dic["int"] = info;
            dic["Int32"] = info;

            info = new DataTypeInfos("long", new DbType[] { DbType.Int64,DbType.Int16, DbType.SByte, DbType.Int32 },
                "DefaultValue.DefaultLong","DefaultValue.DefaultLongValue", 0);
            dic["long"] = info;
            //info = new DataTypeInfos("Int64", "Int64", "DefaultValue.DefaultLongValue", false);
            dic["Int64"] = info;

            info = new DataTypeInfos("ushort", new DbType[] { DbType.Int16, DbType.SByte}, 
                "ushort.MinValue","DefaultValue.DefaultUshortValue", 0);
            dic["ushort"] = info;
            //info = new DataTypeInfos("UInt16", "UInt16", "DefaultValue.DefaultUshortValue", false);
            dic["UInt16"] = info;

            info = new DataTypeInfos("uint", new DbType[] { DbType.Int64,DbType.Int32, DbType.Int16, DbType.SByte, },
                "0","DefaultValue.DefaultUintValue", 0);
            dic["uint"] = info;
            dic["UInt32"] = info;

            info = new DataTypeInfos("ulong", new DbType[] { DbType.Int64 ,DbType.Int16, DbType.SByte, DbType.Int32 }, 
                "0","DefaultValue.DefaultUlongValue", 0);
            dic["ulong"] = info;
            dic["UInt64"] = info;

            info = new DataTypeInfos("string", new DbType[] { DbType.AnsiString, DbType.AnsiStringFixedLength, DbType.String, DbType.StringFixedLength },
                "null","null", 32);
            dic["string"] = info;
            dic["String"] = info;


            info = new DataTypeInfos("byte", new DbType[] { DbType.Byte, DbType.SByte }, 
                "0","DefaultValue.DefaultByteValue", 0);
            dic["byte"] = info;
            dic["Byte"] = info;

            info = new DataTypeInfos("sbyte", new DbType[] { DbType.Byte, DbType.SByte }, 
                "DefaultValue.DefaultSbyte","DefaultValue.DefaultSbyteValue", 0);
            dic["sbyte"] = info;
            dic["SByte"] = info;

            info = new DataTypeInfos("byte[]", new DbType[] { DbType.Binary },
                "null","null", 512);
            dic["byte[]"] = info;
            return dic;
        }

        
            /// <summary>
        /// 根据类型名获取信息
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static DataTypeInfos GetTypeInfo(Member source) 
        {
            if (source.MemberType is ClrEnumeration) 
            {
                return GetTypeInfo("int");
            }
            return GetTypeInfo(source.MemberTypeShortName);
        } 
        /// <summary>
        /// 根据类型名获取信息
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static DataTypeInfos GetTypeInfo(string typeName) 
        {
            string key=typeName.Trim().Trim('?');
            DataTypeInfos ret=null;
            if (_dicTypeInfos.TryGetValue(key, out ret)) 
            {

                return ret;
            }
            return null;
        }

        /// <summary>
        /// 判断类型是否为非空
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool IsNullProperty(string typeName) 
        {
            DataTypeInfos info = GetTypeInfo(typeName);
            if (info != null)
            {
                if (typeName.Trim().LastIndexOf('?') >= 0)
                {
                    return true;
                }
                else
                {
                    return info.DefaultValue == info.DefaultNullValue;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取数值默认类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetMemberVariableDefaultValue(string typeName)
        {
            DataTypeInfos info = GetTypeInfo(typeName);
            if (info != null) 
            {
                if (typeName.Trim().LastIndexOf('?') >= 0)
                {
                    return info.DefaultNullValue;
                }
                return info.DefaultValue;
            }
            return "null";
        }

        private static ComboBoxItemCollection _lstPropertyTypeItem = InitPropertyType();

        /// <summary>
        /// 初始化类型集合
        /// </summary>
        /// <returns></returns>
        private static ComboBoxItemCollection InitPropertyType() 
        {
            ComboBoxItemCollection lst = new ComboBoxItemCollection();
            lst.Add(new ComboBoxItem("普通", EntityPropertyType.Normal));
            lst.Add(new ComboBoxItem("主键", EntityPropertyType.PrimaryKey));
            lst.Add(new ComboBoxItem("自增长主键", EntityPropertyType.PrimaryKey | EntityPropertyType.Identity));
            lst.Add(new ComboBoxItem("自增长", EntityPropertyType.Identity));
            lst.Add(new ComboBoxItem("版本号", EntityPropertyType.Version));
            return lst;
        }

        
        /// <summary>
        /// 属性类型集合
        /// </summary>
        public static ComboBoxItemCollection PropertyTypeItems
        {
            get { return _lstPropertyTypeItem; }
        }

    }
}
