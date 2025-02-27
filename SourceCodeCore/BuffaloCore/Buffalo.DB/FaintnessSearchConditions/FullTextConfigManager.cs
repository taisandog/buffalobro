using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Buffalo.DB.CommBase;
using System.Data;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.FaintnessSearchConditions
{
    /// <summary>
    /// 获取配置全文搜索的配
    /// </summary>
    public class FullTextConfigManager
    {
        //private static Dictionary<string, bool> dicFullTextConfig = null;

        ///// <summary>
        ///// 返回有全文搜索的配置
        ///// </summary>
        //private static Dictionary<string, bool> DicFullTextConfig 
        //{
        //    get 
        //    {
        //        if (dicFullTextConfig == null) 
        //        {
        //            InitConfig();
        //        }
        //        return dicFullTextConfig;
        //    }
        //}
        #region 初始化配置
        ///// <summary>
        ///// 初始化配置
        ///// </summary>
        //private static void InitConfig() 
        //{
        //    dicFullTextConfig = new Dictionary<string, bool>();
        //    XmlDocument doc = ConfigXmlLoader.LoadXml("FullTextConfig"); ;
        //    if (doc != null) 
        //    {
        //        LoadConfig(doc);
        //    }
        //}
        ///// <summary>
        ///// 加载信息
        ///// </summary>
        ///// <param name="doc">XML</param>
        //private static void LoadConfig(XmlDocument doc) 
        //{
        //    XmlNodeList lstEntity = doc.GetElementsByTagName("entity");
        //    foreach (XmlNode node in lstEntity) 
        //    {
        //        XmlAttribute attName = node.Attributes["name"];
        //        if (attName != null) 
        //        {
        //            string name=attName.Value;
        //            XmlNodeList lstProperty = node.ChildNodes;
        //            foreach (XmlNode nodeProperty in lstProperty) 
        //            {
        //                if (nodeProperty.Name == "property") 
        //                {
        //                    string propertyName=nodeProperty.InnerText;
        //                    string fullName = name + "." + propertyName;
        //                    if (!dicFullTextConfig.ContainsKey(fullName)) 
        //                    {
        //                        dicFullTextConfig.Add(fullName, true);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion

        /// <summary>
        /// 过滤like的值
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        private static string FilterLikeValue(string value) 
        {
            value=value.Replace("'", "''");
            value = value.Replace("[", "[[]");
            value = value.Replace("]", "[]]");
            value = value.Replace("%", "[%]");
            return value;
        }

        /// <summary>
        /// 返回Like的查询字符串
        /// </summary>
        /// <param name="scope">条件类</param>
        /// <param name="list">参数列表</param>
        /// <param name="paranName">所属的字段名</param>
        /// <param name="type">当前的数据库类型</param>
        /// <param name="lstIndex">当前索引的标识未辨别同名字段的参数，可设置为0</param>
        /// <param name="entityType">当前实体的类型</param>
        /// <param name="connectString">条件连接的字符串</param>
        /// <param name="isFreeText">是否全文检索</param>
        /// <returns></returns>
        public static string GetLikeSql(Scope scope, ParamList list, string paranName, DbType type, int lstIndex, Type entityType, string connectString,bool isFreeText) 
        {
            //string fullName = entityType.FullName + "." + scope.PropertyName;
            DBInfo db = EntityInfoManager.GetEntityHandle(entityType).DBInfo;
            string ret = " " + connectString;
            string paramVal = db.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(paranName, lstIndex));
            string paramKey = db.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(paranName, lstIndex));
            if (isFreeText && !NoiseWord.IsNoiseWord(scope.Value1.ToString()))
            {
                if (list != null)
                {
                    ret += db.CurrentDbAdapter.FreeTextLike(paranName, paramVal);
                    
                    list.AddNew(paramKey , type, scope.Value1);

                }
                else
                {
                    ret += db.CurrentDbAdapter.FreeTextLike(paranName, DataAccessCommon.FormatValue(scope.Value1, type, db));
                }
            }
            else
            {
                if (list != null)
                {
                    string curValue = scope.Value1.ToString();
                    curValue=FilterLikeValue(curValue);
                    ret += " (" +db.CurrentDbAdapter.FormatParam(paranName) + " like "+db.CurrentDbAdapter.ConcatString("'%'",paramVal,"'%'")+")";
                    list.AddNew(paramKey, type, curValue);

                }
                else
                {
                    string curValue = scope.Value1.ToString();
                    curValue = FilterLikeValue(curValue);
                    ret += " (" + db.CurrentDbAdapter.FormatParam(paranName)+ " like '%" + curValue + "%')";
                }
            }
            return ret;
        }
    }
}
