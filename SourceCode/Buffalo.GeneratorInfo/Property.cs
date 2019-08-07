using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.GeneratorInfo
{
    /// <summary>
    /// �������Ե���
    /// </summary>
    public class Property
    {
        /// <summary>
        /// �������Ե���
        /// </summary>
        /// <param name="dicCheckItem">ѡ�е���</param>
        /// <param name="typeFullName">��Ӧ���ֶ�����ȫ��</param>
        /// <param name="summary">ע��</param>
        /// <param name="typeName">������</param>
        /// <param name="propertyName">��Ӧ��������</param>
        /// <param name="dbInfo">���ԵĹ������ݿ���Ϣ</param>
        /// <param name="relInfo">������Ϣ</param>
        public Property(Dictionary<string, object> dicCheckItem, string typeFullName,
            string summary, string typeName, string propertyName,
            TableInfo tableInfo, RelationInfo relInfo) 
        {
            _dicCheckItem = dicCheckItem;
            _propertyName = propertyName;
            _typeFullName = typeFullName;
            _summary = summary;
            _typeName=typeName;
            _tableInfo = tableInfo;
            _relInfo = relInfo;
        }
        private TableInfo _tableInfo;
        /// <summary>
        /// ���ԵĹ������ݿ���Ϣ
        /// </summary>
        public TableInfo TableInfo 
        {
            get 
            {
                return _tableInfo;
            }
        }

        private RelationInfo _relInfo;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public RelationInfo RelInfo
        {
            get { return _relInfo; }
        }

        private Dictionary<string, object> _dicCheckItem = null;

        /// <summary>
        /// ѡ�е���
        /// </summary>
        internal Dictionary<string, object> CheckItem
        {
            get
            {
                return _dicCheckItem;
            }
        }

        public int ItemCount 
        {
            get 
            {
                return _dicCheckItem.Count;
            }
        }
        /// <summary>
        /// ��ȡֵ
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
        /// <summary>
        /// ��ȡ�Ƿ�ѡ����
        /// </summary>
        /// <param name="itemName">������</param>
        /// <returns></returns>
        public bool HasItem(string itemName)
        {
            object ret = false;
            if (_dicCheckItem.TryGetValue(itemName, out ret))
            {
                if (ret is bool)
                {
                    return (bool)ret;
                }
            }
            return false;
        }
        private string _typeFullName;

        /// <summary>
        /// ��Ӧ���ֶ�����
        /// </summary>
        public string TypeFullName
        {
            get { return _typeFullName; }
            
        }

        private string _summary;

        /// <summary>
        /// ע��
        /// </summary>
        public string Summary
        {
            get { return _summary; }
        }

        private string _typeName;

        /// <summary>
        /// ������
        /// </summary>
        public string TypeName
        {
            get { return _typeName; }
        }

        private string _propertyName;
        /// <summary>
        /// ��Ӧ��������
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }
    }
}
