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
    /// 查找条件类型
    /// </summary>
    public enum ScopeType :int
    {
        /// <summary>
        /// in条件(如果集合为空，则返回1=2)
        /// </summary>
        [Description("in条件")]
        IN=0,
        /// <summary>
        /// notin条件(如果集合为空，则返回1=1)
        /// </summary>
        [Description("not in条件")]
        NotIn = 1,
        /// <summary>
        /// 大于等于
        /// </summary>
        [Description("大于等于")]
        MoreThen=2,
        /// <summary>
        /// 大于
        /// </summary>
        [Description("大于")]
        More = 3,
        /// <summary>
        /// 小于
        /// </summary>
        [Description("小于")]
        Less = 4,
        /// <summary>
        /// 小于等于
        /// </summary>
        [Description("小于等于")]
        LessThen = 5,
        /// <summary>
        /// 在..之间
        /// </summary>
        [Description("在..之间")]
        Between = 6,
        /// <summary>
        /// 不等于
        /// </summary>
        [Description("不等于")]
        NotEqual = 7,
        /// <summary>
        /// 等于
        /// </summary>
        [Description("等于")]
        Equal = 8,
        /// <summary>
        /// 包含
        /// </summary>
        [Description("包含")]
        Like = 9,
        /// <summary>
        /// 开头字符
        /// </summary>
        [Description("开头字符")]
        StarWith = 10,
        /// <summary>
        /// 结束字符
        /// </summary>
        [Description("结束字符")]
        EndWith = 11,
        /// <summary>
        /// 全文检索
        /// </summary>
        [Description("全文检索")]
        Contains = 12,
        /// <summary>
        /// 条件
        /// </summary>
        [Description("条件")]
        Scope = 13,

        /// <summary>
        /// BQL条件
        /// </summary>
        [Description("BQL条件")]
        Condition=14
    }

    /// <summary>
    /// 连接条件类型
    /// </summary>
    public enum ConnectType:int
    {
        /// <summary>
        /// And条件
        /// </summary>
        And=0,
        /// <summary>
        /// or条件
        /// </summary>
        OR=1
    }
    /// <summary>
    /// 描述查找条件的类
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
        /// 条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value1">值1</param>
        /// <param name="value2">值2</param>
        /// <param name="scopeType">条件类型</param>
        /// <param name="connectType">连接类型</param>
        public Scope(string propertyName,object value1, object value2, ScopeType scopeType, ConnectType connectType) 
        {
            this.propertyName = propertyName;
            this.value1 = value1;
            this.value2 = value2;
            this.scopeType = scopeType;
            this.connectType = connectType;
        }

        /// <summary>
        /// 获取In的集合的哈希表集合
        /// </summary>
        /// <returns></returns>
        public Hashtable GetInCollection() 
        {
            if (this.scopeType != ScopeType.IN) 
            {
                throw new Exception("此项不是In类型！");
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
        /// 值1
        /// </summary>
        public object Value1 
        {
            get 
            {
                return value1;
            }
        }

        /// <summary>
        /// 值2
        /// </summary>
        public object Value2
        {
            get
            {
                return value2;
            }
        }

        /// <summary>
        /// 查找条件
        /// </summary>
        public ScopeType ScopeType
        {
            get
            {
                return scopeType;
            }
        }

        /// <summary>
        /// 连接类型
        /// </summary>
        public ConnectType ConnectType 
        {
            get 
            {
                return connectType;
            }
        }

        /// <summary>
        /// 返回此条件对应的属性
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
