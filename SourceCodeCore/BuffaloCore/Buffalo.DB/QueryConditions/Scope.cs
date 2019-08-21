using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Collections;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.Kernel;
using Buffalo.Kernel.Defaults;
namespace Buffalo.DB.QueryConditions
{
    /// <summary>
    /// ������������
    /// </summary>
    public enum ScopeType :int
    {
        /// <summary>
        /// in����(�������Ϊ�գ��򷵻�1=2)
        /// </summary>
        [Description("in����")]
        IN=0,
        /// <summary>
        /// notin����(�������Ϊ�գ��򷵻�1=1)
        /// </summary>
        [Description("not in����")]
        NotIn = 1,
        /// <summary>
        /// ���ڵ���
        /// </summary>
        [Description("���ڵ���")]
        MoreThen=2,
        /// <summary>
        /// ����
        /// </summary>
        [Description("����")]
        More = 3,
        /// <summary>
        /// С��
        /// </summary>
        [Description("С��")]
        Less = 4,
        /// <summary>
        /// С�ڵ���
        /// </summary>
        [Description("С�ڵ���")]
        LessThen = 5,
        /// <summary>
        /// ��..֮��
        /// </summary>
        [Description("��..֮��")]
        Between = 6,
        /// <summary>
        /// ������
        /// </summary>
        [Description("������")]
        NotEqual = 7,
        /// <summary>
        /// ����
        /// </summary>
        [Description("����")]
        Equal = 8,
        /// <summary>
        /// ����
        /// </summary>
        [Description("����")]
        Like = 9,
        /// <summary>
        /// ��ͷ�ַ�
        /// </summary>
        [Description("��ͷ�ַ�")]
        StarWith = 10,
        /// <summary>
        /// �����ַ�
        /// </summary>
        [Description("�����ַ�")]
        EndWith = 11,
        /// <summary>
        /// ȫ�ļ���
        /// </summary>
        [Description("ȫ�ļ���")]
        Contains = 12,
        /// <summary>
        /// ����
        /// </summary>
        [Description("����")]
        Scope = 13,

        /// <summary>
        /// BQL����
        /// </summary>
        [Description("BQL����")]
        Condition=14
    }

    /// <summary>
    /// ������������
    /// </summary>
    public enum ConnectType:int
    {
        /// <summary>
        /// And����
        /// </summary>
        And=0,
        /// <summary>
        /// or����
        /// </summary>
        OR=1
    }
    /// <summary>
    /// ����������������
    /// </summary>
    public class Scope
    {
        private string propertyName;
        private object value1;
        private object value2;
        private ScopeType scopeType;
        private ConnectType connectType;
        private Hashtable inKeys;

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <param name="value1">ֵ1</param>
        /// <param name="value2">ֵ2</param>
        /// <param name="scopeType">��������</param>
        /// <param name="connectType">��������</param>
        public Scope(string propertyName,object value1, object value2, ScopeType scopeType, ConnectType connectType) 
        {
            this.propertyName = propertyName;
            this.value1 = value1;
            this.value2 = value2;
            this.scopeType = scopeType;
            this.connectType = connectType;
        }

        /// <summary>
        /// ��ȡIn�ļ��ϵĹ�ϣ����
        /// </summary>
        /// <returns></returns>
        public Hashtable GetInCollection() 
        {
            if (this.scopeType != ScopeType.IN) 
            {
                throw new Exception("�����In���ͣ�");
            }

            if (inKeys == null)
            {
                IEnumerable list = (IEnumerable)Value1;
                inKeys = new Hashtable();
                foreach (object obj in list)
                {
                    if (!inKeys.ContainsKey(obj))
                    {
                        inKeys.Add(obj, true);
                    }
                }
            }
            return inKeys;
        }

        /// <summary>
        /// ֵ1
        /// </summary>
        public object Value1 
        {
            get 
            {
                return value1;
            }
        }

        /// <summary>
        /// ֵ2
        /// </summary>
        public object Value2
        {
            get
            {
                return value2;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public ScopeType ScopeType
        {
            get
            {
                return scopeType;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public ConnectType ConnectType 
        {
            get 
            {
                return connectType;
            }
        }

        /// <summary>
        /// ���ش�������Ӧ������
        /// </summary>
        public string PropertyName 
        {
            get 
            {
                return propertyName;
            }
        }

        public override string ToString()
        {
            BQLValueItem qvalue = value1 as BQLValueItem;
            if (!CommonMethods.IsNull(qvalue))
            {
                return qvalue.DisplayValue(BQLValueItem.GetKeyInfo());
            }
            else 
            {
                string pName = propertyName;
                
                DbType dbType =DbType.AnsiString ;
                if (value1 != null) 
                {
                    dbType = DefaultType.ToDbType(value1.GetType());
                }
                return DataAccessCommon.FormatScorp(this, null, pName, dbType, 0, null);
            }

            return base.ToString();
        }

        
    }
}
