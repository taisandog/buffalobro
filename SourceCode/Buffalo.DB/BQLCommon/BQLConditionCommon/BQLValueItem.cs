using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.Kernel.Defaults;
using Buffalo.Kernel;
using Buffalo.DB.BQLCommon.BQLExtendFunction;
using Buffalo.DB.DBFunction;
using Buffalo.DB.BQLCommon.BQLConditions;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.DataBaseAdapter;
using System.Diagnostics;
using System.Threading;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    
    public abstract class BQLValueItem:IDisposable
    {
        /// <summary>
        /// ֪ͨ����������һ�˵��ֶε���ֵ����
        /// </summary>
        /// <param name="type"></param>
        internal virtual void ShowDbType(DbType type)
        {
            this.ValueDbType = type;
        }

        /// <summary>
        /// ���ֶζ���һ������
        /// </summary>
        /// <param name="asName">����</param>
        /// <returns></returns>
        public BQLAliasParamHandle As(string asName)
        {
            BQLAliasParamHandle item = new BQLAliasParamHandle(this, asName);
            return item;
        }
        /// <summary>
        /// ���ֶζ���һ������
        /// </summary>
        /// <param name="asName">����</param>
        /// <returns></returns>
        public BQLAliasParamHandle As()
        {
            return As(null);
        }
        ///// <summary>
        ///// StarWith����
        ///// </summary>
        ///// <param name="item">����</param>
        ///// <returns></returns>
        //public BQLConditionItem StarWith(object item)
        //{
        //    BQLValueItem oValue = BQLValueItem.ToValueItem(item);
        //    oValue.ValueDbType = this.ValueDbType;
        //    return new BQLConditionItem(this, new BQLValueItem[] { oValue }, BQLConditionManager.DoStarWith);
        //}

        ///// <summary>
        ///// EndWith����
        ///// </summary>
        ///// <param name="item">����</param>
        ///// <returns></returns>
        //public BQLConditionItem EndWith(object item)
        //{

        //    BQLValueItem oValue = BQLValueItem.ToValueItem(item);
        //    oValue.ValueDbType = this.ValueDbType;
        //    return new BQLConditionItem(this, new BQLValueItem[] { oValue }, BQLConditionManager.DoEndWith);
        //}
        /// <summary>
        /// Between����
        /// </summary>
        /// <param name="star">��ʼֵ</param>
        /// <param name="end">����ֵ</param>
        /// <returns></returns>
        public BQLConditionItem Between(object star, object end)
        {
            BQLValueItem oValue1 = BQLValueItem.ToValueItem(star);
            BQLValueItem oValue2 = BQLValueItem.ToValueItem(end);
            oValue1.ValueDbType = this.ValueDbType;
            oValue2.ValueDbType = this.ValueDbType;
            return new BQLConditionItem(this, new BQLValueItem[] { oValue1, oValue2 }, BQLConditionManager.DoBetween);
        }

        /// <summary>
        /// ȫ�ļ���������
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public BQLConditionItem Contains(object item)
        {
            BQLValueItem oValue = BQLValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new BQLConditionItem(this, new BQLValueItem[] { oValue }, BQLConditionManager.DoContains);

        }

        /// <summary>
        /// �����ַ�
        /// </summary>
        /// <param name="value">Ҫ���ҵ��ַ�</param>
        /// <param name="start">��ʼλ��</param>
        /// <returns></returns>
        public CsqCommonFunction IndexOf(object value, BQLValueItem start)
        {
            BQLValueItem oValue = BQLValueItem.ToValueItem(value); ;

            start.ValueDbType = DbType.Int32;
            if (oValue.ValueDbType == DbType.Object)
            {
                oValue = DbType.String;
            }
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { oValue, this, start }, DBMathFunction.IndexOf, DbType.Int32);
            //handle.ValueDataType = DefaultType.IntType;
            handle.ValueDbType = DbType.Int32;
            return handle;
        }

        /// <summary>
        /// ��ȡ�ַ���
        /// </summary>
        /// <param name="start">�ַ�����ʼλ��</param>
        /// <param name="length">�ַ�������</param>
        /// <returns></returns>
        public CsqCommonFunction SubString(BQLValueItem start, BQLValueItem length)
        {
            start.ValueDbType = DbType.Int32;
            length.ValueDbType = DbType.Int32;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { this, start, length }, DBMathFunction.SubString, DbType.Int32);
            //handle.ValueDataType = DefaultType.IntType;
            handle.ValueDbType = DbType.Int32;
            return handle;
        }
        /// <summary>
        /// Like����
        /// </summary>
        /// <param name="item">ֵ</param>
        /// <param name="type">Like��ʽ</param>
        /// <param name="caseType">��Сд����</param>
        /// <returns></returns>
        public BQLLikeItem Like(object item, BQLLikeType type, BQLCaseType caseType)
        {

            BQLValueItem oValue = BQLValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new BQLLikeItem(this, oValue, type, caseType);
        }
        /// <summary>
        /// Like����
        /// </summary>
        /// <param name="item">ֵ</param>
        /// <returns></returns>
        public BQLLikeItem Like(object item)
        {
            return Like(item, BQLLikeType.Like, BQLCaseType.CaseByDB);
        }
        /// <summary>
        /// Like����
        /// </summary>
        /// <param name="item">ֵ</param>
        /// <param name="type">Like��ʽ</param>
        /// <returns></returns>
        public BQLLikeItem Like(object item, BQLLikeType type)
        {
            return Like(item, type, BQLCaseType.CaseByDB);
        }
        /// <summary>
        /// FreeText����
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public BQLConditionItem FreeText(object item)
        {
            BQLValueItem oValue = BQLValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new BQLConditionItem(this, new BQLValueItem[] { oValue }, BQLConditionManager.DoFreeText);
        }
        /// <summary>
        /// ʱ�����Ͱ�ָ����ʽת�����ַ���
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public CsqConvertFunction DateTimeToString(string format)
        {
            CsqConvertFunction handle = new CsqConvertFunction(this, format, DBConvertFunction.DateTimeToString);
            //handle.ValueDataType = DefaultType.StringType;
            handle.ValueDbType = DbType.String;
            return handle;
        }
        /// <summary>
        /// �ַ�����ָ����ʽת����ʱ������
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public CsqConvertFunction StringToDateTime(string format)
        {
            CsqConvertFunction handle = new CsqConvertFunction(this, format, DBConvertFunction.StringToDateTime);
            handle.ValueDbType = DbType.DateTime;
            //handle.ValueDataType = DefaultType.DateTimeType;
            return handle;
        }

        /// <summary>
        /// �ַ�����ָ����ʽת����ʱ������
        /// </summary>
        /// <param name="dbType">ת����ָ������</param>
        /// <returns></returns>
        public CsqConvertFunction ConvertTo(DbType dbType)
        {
            CsqConvertFunction handle = new CsqConvertFunction(this, dbType, DBConvertFunction.ConvetTo);
            //handle.ValueDataType = DefaultType.DateTimeType;
            handle.ValueDbType = dbType;
            return handle;
        }
        /// <summary>
        /// �Ƿ��ֵ
        /// </summary>
        /// <returns></returns>
        internal virtual bool IsNullValue() 
        {
            return false;
        }

        protected DbType _valueDbType = DbType.Object;

        /// <summary>
        /// ��Ӧ�����ݿ�����
        /// </summary>
        internal DbType ValueDbType
        {
            get { return _valueDbType; }
            set { _valueDbType = value; }
        }
        internal abstract string DisplayValue(KeyWordInfomation info);

        internal abstract void FillInfo(KeyWordInfomation info);

        /// <summary>
        /// ִ�������Ϣ����
        /// </summary>
        /// <param name="value">Ҫִ�е�ֵ</param>
        /// <param name="info">��Ϣ</param>
        public static void DoFillInfo(BQLValueItem value, KeyWordInfomation info) 
        {
            if (!CommonMethods.IsNull(value)) 
            {
                value.FillInfo(info);
            }
        }

        /// <summary>
        /// ��ʽ��ֵ���͵�ֵ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static ValueType FormatValueType(ValueType value)
        {
            if (value.GetType().IsEnum)
            {
                int ret = (int)value;
                return ret;
            }
            return value;
        }
        /// <summary>
        /// �Ѵ�������ֵת����BQL��ʶ���ֵ��
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BQLValueItem ToValueItem(object value) 
        {
            if (CommonMethods.IsNull(value)) 
            {
                return new BQLValueTypeItem(null);
            }
            BQLQuery query = value as BQLQuery;
            if (!CommonMethods.IsNull(query)) 
            {
                return query.AS(null);
            }

            BQLValueItem item = value as BQLValueItem;
            if (!CommonMethods.IsNull(item))
            {
                return item;
            }

            Type type=value.GetType();
            if (type.IsEnum) //�ж�ö��
            {
                Type realType = Enum.GetUnderlyingType(type);
                value = Convert.ChangeType(value,realType);
                return new BQLValueTypeItem(value);
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))//�ж�Nullable
            {
                Type realType = Nullable.GetUnderlyingType(type);
                value = Convert.ChangeType(value, realType);
                return new BQLValueTypeItem(value);
            }

            return new BQLValueTypeItem(value);
        }
        public static implicit operator BQLValueItem(ValueType value)
        {

            return new BQLValueTypeItem(FormatValueType(value));
        }
        public static implicit operator BQLValueItem(byte[] value)
        {
            return new BQLValueTypeItem(value);
        }
        public static implicit operator BQLValueItem(string value)
        {
            return new BQLValueTypeItem(value);
        }
        
        /// <summary>
        /// ͳһ���ݿ�ֵ����
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        protected internal static void UnityDbType(BQLValueItem value1, BQLValueItem value2)
        {
            if (CommonMethods.IsNull(value1)  || CommonMethods.IsNull(value2)) 
            {
                return;
            }
            DbType type = DbType.Object;
            if (value1.ValueDbType != value2.ValueDbType) 
            {
                if (value1.ValueDbType != DbType.Object) 
                {
                    value2.ShowDbType(value1.ValueDbType);
                } 
                else if (value2.ValueDbType != DbType.Object) 
                {
                    value1.ShowDbType(value2.ValueDbType);
                }
            }
            //if (value1.ValueDbType != DbType.Object) 
            //{
            //    type = value1.ValueDbType;
            //}
            //if (value2.ValueDbType != DbType.Object)
            //{
            //    type = value2.ValueDbType;
            //}

            //if (type != DbType.Object) 
            //{
            //    if (value1.ValueDbType == DbType.Object) 
            //    {
            //        value1.ValueDbType = type;
            //    }
            //    if (value2.ValueDbType == DbType.Object)
            //    {
            //        value2.ValueDbType = type;
            //    }
            //}
        }

        public static BQLOperatorHandle operator +(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLOperatorHandle fHandle = new BQLOperatorHandle(FunctionManager.DoAdd, new BQLValueItem[] { handle, oValue });
            //fHandle.PriorityLevel = 6;
            fHandle.PriorityLevel = OperatorPrecedenceUnit.GetPrecedence("+");
            return fHandle;
        }
        public static BQLOperatorHandle operator -(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLOperatorHandle fHandle = new BQLOperatorHandle(FunctionManager.DoSub, new BQLValueItem[] { handle, oValue });
            //fHandle.PriorityLevel = 6;
            fHandle.PriorityLevel = OperatorPrecedenceUnit.GetPrecedence("-");
            return fHandle;
        }
        public static BQLOperatorHandle operator *(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLOperatorHandle fHandle = new BQLOperatorHandle(FunctionManager.DoMul, new BQLValueItem[] { handle, oValue });
            //fHandle.PriorityLevel = 7;
            fHandle.PriorityLevel = OperatorPrecedenceUnit.GetPrecedence("*");
            return fHandle;
        }
        public static BQLOperatorHandle operator /(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLOperatorHandle fHandle = new BQLOperatorHandle(FunctionManager.DoDiv, new BQLValueItem[] { handle, oValue });
            //fHandle.PriorityLevel = 7;
            fHandle.PriorityLevel = OperatorPrecedenceUnit.GetPrecedence("/");
            return fHandle;
        }
        public static BQLOperatorHandle operator %(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);

            BQLOperatorHandle fHandle = new BQLOperatorHandle(FunctionManager.DoMod, new BQLValueItem[] { handle, oValue });
            //fHandle.PriorityLevel = 3;
            fHandle.PriorityLevel = OperatorPrecedenceUnit.GetPrecedence("%");
            return fHandle;
        }

        public static BQLComparItem operator ==(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoEqual, new BQLValueItem[] { handle, oValue });
            //fHandle.PriorityLevel = 1;
            fHandle.PriorityLevel = OperatorPrecedenceUnit.GetPrecedence("=");
            return fHandle;
        }
        public static BQLComparItem operator !=(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoNotequal, new BQLValueItem[] { handle, oValue });
            //fHandle.PriorityLevel = 1;
            fHandle.PriorityLevel = OperatorPrecedenceUnit.GetPrecedence("!=");
            return fHandle;
        }

        



        public static BQLComparItem operator &(BQLValueItem handle, BQLValueItem value)
        {
            List<BQLValueItem> lstValues = new List<BQLValueItem>();
            if (!CommonMethods.IsNull(handle))
            {
                lstValues.Add(handle);
            }
            if (!CommonMethods.IsNull(value))
            {
                lstValues.Add(value);
            }

            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoAnd, lstValues);
            //fHandle.PriorityLevel = 3;
            fHandle.PriorityLevel = OperatorPrecedenceUnit.GetPrecedence("&");
            return fHandle;
        }
        
        public static BQLComparItem operator |(BQLValueItem handle, BQLValueItem value)
        {
            List<BQLValueItem> lstValues = new List<BQLValueItem>();
            if (!CommonMethods.IsNull(handle))
            {
                lstValues.Add(handle);
            }
            if (!CommonMethods.IsNull(value))
            {
                lstValues.Add(value);
            }
            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoOr, lstValues);
            //fHandle.PriorityLevel = 4;
            fHandle.PriorityLevel = OperatorPrecedenceUnit.GetPrecedence("|");
            return fHandle;
        }
        public static BQLComparItem operator >(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoMore, new BQLValueItem[] { handle, oValue });
            //fHandle.PriorityLevel = 5;
            fHandle.PriorityLevel = OperatorPrecedenceUnit.GetPrecedence(">");
            return fHandle;
        }
        public static BQLComparItem operator >=(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoMorethen, new BQLValueItem[] { handle, oValue });
            //fHandle.PriorityLevel = 5;
            fHandle.PriorityLevel = OperatorPrecedenceUnit.GetPrecedence(">=");
            return fHandle;
        }
        public static BQLComparItem operator <(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoLess, new BQLValueItem[] { handle, oValue });
            //fHandle.PriorityLevel = 5;
            fHandle.PriorityLevel = OperatorPrecedenceUnit.GetPrecedence("<");
            return fHandle;
        }
        public static BQLComparItem operator <=(BQLValueItem handle, object value)
        {
            BQLValueItem oValue = ToValueItem(value);
            UnityDbType(handle, oValue);
            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoLessThen, new BQLValueItem[] { handle, oValue });
            //fHandle.PriorityLevel = 5;
            fHandle.PriorityLevel = OperatorPrecedenceUnit.GetPrecedence("<=");
            return fHandle;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        //private static KeyWordInfomation _putKeyInfo = GetPutKeyInfo();

        /// <summary>
        /// ��ȡ��Ϣ
        /// </summary>
        /// <returns></returns>
        private static KeyWordInfomation NewKeyWordInfomation()
        {
            KeyWordInfomation info = new KeyWordInfomation();
            
            info.Infos = new BQLInfos();
            info.ParamList = null;
            info.OutPutModle = true;
            return info;
        }
        //private static readonly string KeyWordInfomationKey = "$$Buffalo.KeyWordInfomation";
        private static ThreadLocal<KeyWordInfomation> _curKeyWordInfomation = new System.Threading.ThreadLocal<KeyWordInfomation>();
        /// <summary>
        /// ��ȡĬ�ϵ�Key��Ϣ
        /// </summary>
        /// <returns></returns>
        internal static KeyWordInfomation GetKeyInfo() 
        {
            KeyWordInfomation info = _curKeyWordInfomation.Value;
            if (info == null)
            {
                info = NewKeyWordInfomation();
                info.DBInfo = DataAccessLoader.GetFristDBInfo();
                _curKeyWordInfomation.Value = info;
            }
            return info;
        }
        /// <summary>
        /// ��ȡĬ�ϵ�Key��Ϣ
        /// </summary>
        /// <returns></returns>
        internal static KeyWordInfomation GetKeyWordInfomation(DBInfo info)
        {
            KeyWordInfomation kinfo = NewKeyWordInfomation();
            kinfo.DBInfo = info;
            return kinfo;
        }
        public override string ToString()
        {
            return DisplayValue(GetKeyInfo());
        }

        public virtual void Dispose()
        {
            
        }
//#if DEBUG

//        ~BQLValueItem()
//        {
//            Debug.WriteLine(this.GetType().FullName + "���ͷ�");
//        }
//#endif
    }
}
