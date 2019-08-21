using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace Buffalo.DB.PropertyAttributes
{
    public class TableAttribute : System.Attribute
    {
        private string _tableName;
        private string _belongDB;



        /// <summary>
        /// ���ʾ
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="connectionKey">�����ַ����ļ�</param>
        public TableAttribute(string belongDB,string tableName)
        {
            this._tableName = tableName;
            this._belongDB = belongDB;
        }
        /// <summary>
        /// ���ʾ
        /// </summary>
        public TableAttribute():this("","")
        {

        }

        private bool _allowLazy;

        /// <summary>
        /// �Ƿ������ӳټ���
        /// </summary>
        public bool AllowLazy
        {
            get { return _allowLazy; }
            set { _allowLazy = value; }
        }

        private string _description;

        /// <summary>
        /// �ֶ�ע��
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public string BelongDB
        {
            get { return _belongDB; }
            set { _belongDB = value; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string TableName 
        {
            get 
            {
                return _tableName;
            }
            set 
            {
                _tableName = value;
            }
        }
        

    }
}
