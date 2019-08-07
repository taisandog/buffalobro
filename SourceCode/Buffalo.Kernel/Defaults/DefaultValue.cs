using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.Defaults
{
    public class DefaultValue
    {
        private DefaultValue() { }
        /// <summary>
        /// 整型默认值
        /// </summary>
        public static readonly int? DefaultIntValue = null;
        /// <summary>
        /// 布尔型默认值
        /// </summary>
        public static readonly bool? DefaultBooleanValue = null;
        /// <summary>
        /// 双精度默认值
        /// </summary>
        public static readonly double? DefaultDoubleValue = null;
        /// <summary>
        /// 浮点型默认值
        /// </summary>
        public static readonly float? DefaultFloatValue = null;
        /// <summary>
        /// 时间默认值
        /// </summary>
        public static readonly DateTime? DefaultDateTimeValue = null;
        /// <summary>
        /// Decimal默认值
        /// </summary>
        public static readonly decimal? DefaultDecimalValue = null;
        /// <summary>
        /// 字节默认值
        /// </summary>
        public static readonly byte? DefaultByteValue = null;
        /// <summary>
        /// 小字节默认值
        /// </summary>
        public static readonly sbyte? DefaultSbyteValue = null;
        /// <summary>
        /// 短整型默认值
        /// </summary>
        public static readonly short? DefaultShortValue = null;
        /// <summary>
        /// 长整型默认值
        /// </summary>
        public static readonly long? DefaultLongValue = null;
        /// <summary>
        /// 无符号整型默认值
        /// </summary>
        public static readonly uint? DefaultUintValue = null;
        /// <summary>
        /// 无符号短整型默认值
        /// </summary>
        public static readonly ushort? DefaultUshortValue = null;
        /// <summary>
        /// 无符号长整型默认值
        /// </summary>
        public static readonly ulong? DefaultUlongValue = null;
        /// <summary>
        /// Guid类型
        /// </summary>
        public static readonly Guid? DefaultGuidValue = null;

        /// <summary>
        /// 整型默认值
        /// </summary>
        public static readonly int DefaultInt = Int32.MinValue;

        /// <summary>
        /// 双精度默认值
        /// </summary>
        public static readonly double DefaultDouble = Double.MinValue;
        /// <summary>
        /// 浮点型默认值
        /// </summary>
        public static readonly float DefaultFloat = float.MinValue;

        /// <summary>
        /// Decimal默认值
        /// </summary>
        public static readonly decimal DefaultDecimal = decimal.MinValue;
        /// <summary>
        /// 小字节默认值
        /// </summary>
        public static readonly sbyte DefaultSbyte = sbyte.MinValue;
        /// <summary>
        /// 短整型默认值
        /// </summary>
        public static readonly short DefaultShort = short.MinValue;
        /// <summary>
        /// 长整型默认值
        /// </summary>
        public static readonly long DefaultLong = long.MinValue;
        /// <summary>
        /// Guid默认值
        /// </summary>
        public static readonly Guid DefaultGuid = Guid.Empty;
        /// <summary>
        /// 时间默认值
        /// </summary>
        public static readonly DateTime DefaultDateTime = DateTime.MinValue;
        /// <summary>
        /// 把值按照指定格式转换成字符串
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="formatString">格式字符串</param>
        /// <returns></returns>
        public static string ValueToString(object value, string formatString)
        {
            if (value is IFormattable)
            {
                IFormattable fom = (IFormattable)value;
                return fom.ToString(formatString, null);
            }
            return value.ToString();
        }
        /// <summary>
        /// 对两个object值进行比较(-1为val1&lt;val2,0为val1=val2,1为val1&gt;val2)
        /// </summary>
        /// <param name="val1">值1</param>
        /// <param name="val2">值2</param>
        /// <returns>-1为val1&lt;val2,0为val1=val2,1为val1&gt;val2</returns>
        public static int Compare(object val1, object val2)
        {
            if (val1 != null && val2 != null)
            {
                IComparable com1 = (IComparable)val1;
                IComparable com2 = (IComparable)val2;
                return com1.CompareTo(com2);
            }
            else if (val1 == null && val2 != null)
            {
                return -1;
            }
            else if (val1 != null && val2 == null)
            {
                return 1;
            }
            else if (val1 == null && val2 == null)
            {
                return 0;
            }
            return 0;
        }
        /// <summary>
        /// 获取类型的默认值
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
    }
}
