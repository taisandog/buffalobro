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
        /// 通知函数符号另一端的字段的数值类型
        /// </summary>
        /// <param name="type"></param>
        internal virtual void ShowDbType(DbType type)
        {
            this.ValueDbType = type;
        }

        /// <summary>
        /// 给字段定义一个别名
        /// </summary>
        /// <param name="asName">别名</param>
        /// <returns></returns>
        public BQLAliasParamHandle As(string asName)
        {
            BQLAliasParamHandle item = new BQLAliasParamHandle(this, asName);
            return item;
        }
        /// <summary>
        /// 给字段定义一个别名
        /// </summary>
        /// <param name="asName">别名</param>
        /// <returns></returns>
        public BQLAliasParamHandle As()
        {
            return As(null);
        }
        ///// <summary>
        ///// StarWith条件
        ///// </summary>
        ///// <param name="item">条件</param>
        ///// <returns></returns>
        //public BQLConditionItem StarWith(object item)
        //{
        //    BQLValueItem oValue = BQLValueItem.ToValueItem(item);
        //    oValue.ValueDbType = this.ValueDbType;
        //    return new BQLConditionItem(this, new BQLValueItem[] { oValue }, BQLConditionManager.DoStarWith);
        //}

        ///// <summary>
        ///// EndWith条件
        ///// </summary>
        ///// <param name="item">条件</param>
        ///// <returns></returns>
        //public BQLConditionItem EndWith(object item)
        //{

        //    BQLValueItem oValue = BQLValueItem.ToValueItem(item);
        //    oValue.ValueDbType = this.ValueDbType;
        //    return new BQLConditionItem(this, new BQLValueItem[] { oValue }, BQLConditionManager.DoEndWith);
        //}
        /// <summary>
        /// Between条件
        /// </summary>
        /// <param name="star">开始值</param>
        /// <param name="end">结束值</param>
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
        /// 全文检索的条件
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
        /// 查找字符
        /// </summary>
        /// <param name="value">要查找的字符</param>
        /// <param name="start">起始位置</param>
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
        /// 截取字符串
        /// </summary>
        /// <param name="start">字符串起始位置</param>
        /// <param name="length">字符串长度</param>
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
        /// Like条件
        /// </summary>
        /// <param name="item">值</param>
        /// <param name="type">Like方式</param>
        /// <param name="caseType">大小写参数</param>
        /// <returns></returns>
        public BQLLikeItem Like(object item, BQLLikeType type, BQLCaseType caseType)
        {

            BQLValueItem oValue = BQLValueItem.ToValueItem(item);
            oValue.ValueDbType = this.ValueDbType;
            return new BQLLikeItem(this, oValue, type, caseType);
        }
        /// <summary>
        /// Like条件
        /// </summary>
        /// <param name="item">值</param>
        /// <returns></returns>
        public BQLLikeItem Like(object item)
        {
            return Like(item, BQLLikeType.Like, BQLCaseType.CaseByDB);
        }
        /// <summary>
        /// Like条件
        /// </summary>
        /// <param name="item">值</param>
        /// <param name="type">Like方式</param>
        /// <returns></returns>
        public BQLLikeItem Like(object item, BQLLikeType type)
        {
            return Like(item, type, BQLCaseType.CaseByDB);
        }
        /// <summary>
        /// FreeText条件
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
        /// 时间类型按指定格式转换到字符串
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
        /// 字符串按指定格式转换到时间类型
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
        /// 字符串按指定格式转换到时间类型
        /// </summary>
        /// <param name="dbType">转换到指定类型</param>
        /// <returns></returns>
        public CsqConvertFunction ConvertTo(DbType dbType)
        {
            CsqConvertFunction handle = new CsqConvertFunction(this, dbType, DBConvertFunction.ConvetTo);
            //handle.ValueDataType = DefaultType.DateTimeType;
            handle.ValueDbType = dbType;
            return handle;
        }
        /// <summary>
        /// 是否空值
        /// </summary>
        /// <returns></returns>
        internal virtual bool IsNullValue() 
        {
            return false;
        }

        protected DbType _valueDbType = DbType.Object;

        /// <summary>
        /// 对应的数据库类型
        /// </summary>
        internal DbType ValueDbType
        {
            get { return _valueDbType; }
            set { _valueDbType = value; }
        }
        internal abstract string DisplayValue(KeyWordInfomation info);

        internal abstract void FillInfo(KeyWordInfomation info);

        /// <summary>
        /// 执行填充信息操作
        /// </summary>
        /// <param name="value">要执行的值</param>
        /// <param name="info">信息</param>
        public static void DoFillInfo(BQLValueItem value, KeyWordInfomation info) 
        {
            if (!CommonMethods.IsNull(value)) 
            {
                value.FillInfo(info);
            }
        }

        /// <summary>
        /// 格式化值类型的值
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
        /// 把传进来的值转换成BQL能识别的值项
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
            if (type.IsEnum) //判断枚举
            {
                Type realType = Enum.GetUnderlyingType(type);
                value = Convert.ChangeType(value,realType);
                return new BQLValueTypeItem(value);
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))//判断Nullable
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
        /// 统一数据库值类型
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
        /// 获取信息
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
        /// 获取默认的Key信息
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
        /// 获取默认的Key信息
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
//            Debug.WriteLine(this.GetType().FullName + "被释放");
//        }
//#endif
    }
}
