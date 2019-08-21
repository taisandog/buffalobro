using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.Defaults
{
    public class DefaultValue
    {
        private DefaultValue() { }
        /// <summary>
        /// ����Ĭ��ֵ
        /// </summary>
        public static readonly int? DefaultIntValue = null;
        /// <summary>
        /// ������Ĭ��ֵ
        /// </summary>
        public static readonly bool? DefaultBooleanValue = null;
        /// <summary>
        /// ˫����Ĭ��ֵ
        /// </summary>
        public static readonly double? DefaultDoubleValue = null;
        /// <summary>
        /// ������Ĭ��ֵ
        /// </summary>
        public static readonly float? DefaultFloatValue = null;
        /// <summary>
        /// ʱ��Ĭ��ֵ
        /// </summary>
        public static readonly DateTime? DefaultDateTimeValue = null;
        /// <summary>
        /// DecimalĬ��ֵ
        /// </summary>
        public static readonly decimal? DefaultDecimalValue = null;
        /// <summary>
        /// �ֽ�Ĭ��ֵ
        /// </summary>
        public static readonly byte? DefaultByteValue = null;
        /// <summary>
        /// С�ֽ�Ĭ��ֵ
        /// </summary>
        public static readonly sbyte? DefaultSbyteValue = null;
        /// <summary>
        /// ������Ĭ��ֵ
        /// </summary>
        public static readonly short? DefaultShortValue = null;
        /// <summary>
        /// ������Ĭ��ֵ
        /// </summary>
        public static readonly long? DefaultLongValue = null;
        /// <summary>
        /// �޷�������Ĭ��ֵ
        /// </summary>
        public static readonly uint? DefaultUintValue = null;
        /// <summary>
        /// �޷��Ŷ�����Ĭ��ֵ
        /// </summary>
        public static readonly ushort? DefaultUshortValue = null;
        /// <summary>
        /// �޷��ų�����Ĭ��ֵ
        /// </summary>
        public static readonly ulong? DefaultUlongValue = null;
        /// <summary>
        /// Guid����
        /// </summary>
        public static readonly Guid? DefaultGuidValue = null;

        /// <summary>
        /// ����Ĭ��ֵ
        /// </summary>
        public static readonly int DefaultInt = Int32.MinValue;

        /// <summary>
        /// ˫����Ĭ��ֵ
        /// </summary>
        public static readonly double DefaultDouble = Double.MinValue;
        /// <summary>
        /// ������Ĭ��ֵ
        /// </summary>
        public static readonly float DefaultFloat = float.MinValue;

        /// <summary>
        /// DecimalĬ��ֵ
        /// </summary>
        public static readonly decimal DefaultDecimal = decimal.MinValue;
        /// <summary>
        /// С�ֽ�Ĭ��ֵ
        /// </summary>
        public static readonly sbyte DefaultSbyte = sbyte.MinValue;
        /// <summary>
        /// ������Ĭ��ֵ
        /// </summary>
        public static readonly short DefaultShort = short.MinValue;
        /// <summary>
        /// ������Ĭ��ֵ
        /// </summary>
        public static readonly long DefaultLong = long.MinValue;
        /// <summary>
        /// GuidĬ��ֵ
        /// </summary>
        public static readonly Guid DefaultGuid = Guid.Empty;
        /// <summary>
        /// ʱ��Ĭ��ֵ
        /// </summary>
        public static readonly DateTime DefaultDateTime = DateTime.MinValue;
        /// <summary>
        /// ��ֵ����ָ����ʽת�����ַ���
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <param name="formatString">��ʽ�ַ���</param>
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
        /// ������objectֵ���бȽ�(-1Ϊval1&lt;val2,0Ϊval1=val2,1Ϊval1&gt;val2)
        /// </summary>
        /// <param name="val1">ֵ1</param>
        /// <param name="val2">ֵ2</param>
        /// <returns>-1Ϊval1&lt;val2,0Ϊval1=val2,1Ϊval1&gt;val2</returns>
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
        /// ��ȡ���͵�Ĭ��ֵ
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
    }
}
