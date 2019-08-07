using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.GeneratorInfo
{
    /// <summary>
    /// Ҫ����ʵ����Ϣ
    /// </summary>
    public class EntityInfo
    {
        /// <summary>
        /// Ҫ����ʵ����Ϣ
        /// </summary>
        /// <param name="dbName">���ݿ���</param>
        /// <param name="fileName">���ļ���</param>
        /// <param name="nameSpace">�����ռ�</param>
        /// <param name="className">����</param>
        /// <param name="summary">ע��</param>
        /// <param name="baseTypeName">������</param>
        /// <param name="dicGenericInfo">������Ϣ</param>
        /// <param name="lstAllProperty">��������</param>
        /// <param name="model">��ѡ����Ϣ</param>
        public EntityInfo(string dbName, string fileName, string nameSpace, string className, 
            string summary, string baseTypeName,List<Property> lstAllProperty,
            Dictionary<string, List<string>> dicGenericInfo, Property model) 
        {
            _dbName = dbName;
            _fileName = fileName; 
            _baseTypeName = baseTypeName;
            _className = className;
            _dicGenericInfo = dicGenericInfo;
            _fileName = fileName;
            _model = model;
            _namespace = nameSpace;
            _summary = summary;
            _allProperty = lstAllProperty;

            foreach (Property pro in _allProperty)
            {
                if (pro.TableInfo == null) 
                {
                    continue;
                }
                if (pro.TableInfo.IsPrimary)
                {
                    _primaryProperty = pro;
                    break;
                }
            }
        }

        private string _dbName;
        /// <summary>
        /// ��ͼ��
        /// </summary>
        public string DBName
        {
            get { return _dbName; }
        }

        private string _fileName;
        /// <summary>
        /// �ļ���
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }
        private string _namespace;
        /// <summary>
        /// �����ռ�
        /// </summary>
        public string Namespace
        {
            get { return _namespace; }
        }

        private string _className;
        /// <summary>
        /// ����
        /// </summary>
        public string ClassName
        {
            get { return _className; }
        }
        /// <summary>
        /// ��ȫ��
        /// </summary>
        public string FullName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(_namespace))
                {
                    sb.Append(_namespace);
                    sb.Append(".");
                }
                sb.Append(_className);
                return sb.ToString();
            }
        }

        private string _summary;

        /// <summary>
        /// ע��
        /// </summary>
        public string Summary
        {
            get { return _summary; }
        }
        private string _baseTypeName;
        /// <summary>
        /// ������
        /// </summary>
        public string BaseTypeName
        {
            get
            {

                return _baseTypeName;
            }
        }

        private Dictionary<string, List<string>> _dicGenericInfo =null;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public Dictionary<string, List<string>> GenericInfo
        {
            get { return _dicGenericInfo; }
        }
        private List<Property> _allProperty;
        /// <summary>
        /// ��������
        /// </summary>
        public List<Property> AllProperty
        {
            get { return _allProperty; }
        }
        private Property _primaryProperty;
        /// <summary>
        /// ������Ӧ������
        /// </summary>
        public Property PrimaryProperty
        {
            get { return _primaryProperty; }
        }


        private Property _model=null;


        /// <summary>
        /// ѡ�е���
        /// </summary>
        internal Dictionary<string, object> CheckItem
        {
            get
            {
                return _model.CheckItem;
            }
        }
        /// <summary>
        /// ��ȡֵ
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public string GetValue(string itemName)
        {
            return _model.GetValue(itemName);
        }

        /// <summary>
        /// ��ȡ�Ƿ�ѡ����
        /// </summary>
        /// <param name="itemName">������</param>
        /// <returns></returns>
        public bool HasItem(string itemName)
        {

            return _model.HasItem(itemName);
        }

    }
}
