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
    /// ��ȡ����ȫ����������
    /// </summary>
    public class FullTextConfigManager
    {
        //private static Dictionary<string, bool> dicFullTextConfig = null;

        ///// <summary>
        ///// ������ȫ������������
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
        #region ��ʼ������
        ///// <summary>
        ///// ��ʼ������
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
        ///// ������Ϣ
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
        /// ����like��ֵ
        /// </summary>
        /// <param name="value">ֵ</param>
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
        /// ����Like�Ĳ�ѯ�ַ���
        /// </summary>
        /// <param name="scope">������</param>
        /// <param name="list">�����б�</param>
        /// <param name="paranName">�������ֶ���</param>
        /// <param name="type">��ǰ�����ݿ�����</param>
        /// <param name="lstIndex">��ǰ�����ı�ʶδ���ͬ���ֶεĲ�����������Ϊ0</param>
        /// <param name="entityType">��ǰʵ�������</param>
        /// <param name="connectString">�������ӵ��ַ���</param>
        /// <param name="isFreeText">�Ƿ�ȫ�ļ���</param>
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
