using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buffalo.Kernel;
using Buffalo.Kernel.Defaults;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Buffalo.QueryCache
{
    public delegate RedisValue RedisConverteHandle(object value);
    public delegate object RedisToValueHandle(RedisValue value);
    public class RedisConverter
    {
        #region 转换函数
        private static RedisValue IntToValue(object value)
        {
            int val = (int)value;
            return val;
        }
        private static RedisValue BoolToValue(object value)
        {
            bool val = (bool)value;
            return val;
        }
        private static RedisValue ByteToValue(object value)
        {
            byte val = (byte)value;
            return val;
        }
        private static RedisValue CharToValue(object value)
        {
            char val = (char)value;
            return val;
        }
        private static RedisValue DateTimeToValue(object value)
        {
            DateTime val = (DateTime)value;
            return CommonMethods.ConvertDateTimeInt(val);
        }
        private static RedisValue DecimalToValue(object value)
        {
            decimal val = (decimal)value;
            return (double)val;
        }
        private static RedisValue DoubleToValue(object value)
        {
            double val = (double)value;
            return val;
        }
        private static RedisValue ShortToValue(object value)
        {
            short val = (short)value;
            return val;
        }
        private static RedisValue LongToValue(object value)
        {
            long val = (long)value;
            return val;
        }
        private static RedisValue SbyteToValue(object value)
        {
            sbyte val = (sbyte)value;
            return val;
        }
        private static RedisValue FloatToValue(object value)
        {
            float val = (float)value;
            return val;
        }
        private static RedisValue StringToValue(object value)
        {
            string val = (string)value;
            return val;
        }
        private static RedisValue UshortToValue(object value)
        {
            ushort val = (ushort)value;
            return val;
        }
        
        private static RedisValue UintToValue(object value)
        {
            uint val = (uint)value;
            return val;
        }
        private static RedisValue UlongToValue(object value)
        {
            ulong val = (ulong)value;
            return val;
        }

        private static RedisValue BytesToValue(object value)
        {
            byte[] val = (byte[])value;
            return val;
        }
        #endregion
        #region RedisValue 转实际类型函数
        private static object ValueToInt(RedisValue value)
        {
            //if (value.IsNull) 
            //{
            //    return 0;
            //}
            return (int)value;
        }
        private static object ValueToBool(RedisValue value)
        {
            //if (value.IsNull)
            //{
            //    return false;
            //}
            return (bool)value;
        }
        private static object ValueToByte(RedisValue value)
        {
            //if (value.IsNull)
            //{
            //    return (byte)0;
            //}
            return (byte)value;
        }
        private static object ValueToChar(RedisValue value)
        {
            //if (value.IsNull)
            //{
            //    return '\0';
            //}
            return (char)value;
        }
        private static object ValueToDateTime(RedisValue value)
        {
            if (value.IsNull)
            {
                return DateTime.MinValue;
            }
            double val = (double)value;
            return CommonMethods.ConvertIntDateTime(val);
        }
        private static object ValueToDecimal(RedisValue value)
        {
            //if (value.IsNull)
            //{
            //    return 0m;
            //}
            double val = (double)value;
            return (decimal)val;
        }
        private static object ValueToDouble(RedisValue value)
        {
            //if (value.IsNull)
            //{
            //    return 0d;
            //}
            return (double)value;
        }
        private static object ValueToShort(RedisValue value)
        {
            //if (value.IsNull)
            //{
            //    return (short)0;
            //}
            return (short)value;
        }
        private static object ValueToLong(RedisValue value)
        {
            //if (value.IsNull)
            //{
            //    return 0L;
            //}
            return (long)value;
        }
        private static object ValueToSbyte(RedisValue value)
        {
            //if (value.IsNull)
            //{
            //    return (sbyte)0;
            //}
            return (sbyte)value;
        }
        private static object ValueToFloat(RedisValue value)
        {
            //if (value.IsNull)
            //{
            //    return 0f;
            //}
            return (float)value;
        }
        private static object ValueToString(RedisValue value)
        {
            //if (value.IsNull)
            //{
            //    return null;
            //}
            return (string)value;
        }
        private static object ValueToUshort(RedisValue value)
        {
            //if (value.IsNull)
            //{
            //    return null;
            //}
            return (string)value;
        }

        private static object ValueToUint(RedisValue value)
        {

            return (uint)value; 
        }
        private static object ValueToUlong(RedisValue value)
        {

            return (ulong)value;
        }

        private static object ValueToBytes(RedisValue value)
        {
            
            return (byte[])value;
        }

        #endregion
        /// <summary>
        /// 允许转换的类型
        /// </summary>
        private static Dictionary<Type, RedisToValueHandle> _dicAllowConvert = LoadAllowConvert();

        /// <summary>
        /// 转换的函数
        /// </summary>
        private static Dictionary<Type, RedisConverteHandle> _dicRedisConvert = LoadAllowRedisConvert();

        /// <summary>
        /// 加载转换函数
        /// </summary>
        /// <returns></returns>
        private static Dictionary<Type, RedisConverteHandle> LoadAllowRedisConvert()
        {
            Dictionary<Type, RedisConverteHandle> dic = new Dictionary<Type, RedisConverteHandle>();
            dic[typeof(bool)] = BoolToValue;
            dic[typeof(byte)] = ByteToValue;
            dic[typeof(char)] = CharToValue;
            dic[typeof(DateTime)] = DateTimeToValue;
            dic[typeof(decimal)] = DecimalToValue;
            dic[typeof(double)] = DoubleToValue;
            dic[typeof(short)] = ShortToValue;
            dic[typeof(int)] = IntToValue;
            dic[typeof(long)] = LongToValue;
            dic[typeof(sbyte)] = SbyteToValue;
            dic[typeof(float)] = FloatToValue;
            dic[typeof(string)] = StringToValue;
            dic[typeof(ushort)] = UshortToValue;
            dic[typeof(uint)] = UintToValue;
            dic[typeof(ulong)] = UlongToValue;
            dic[typeof(byte[])] = BytesToValue;
            return dic;
        }
        /// <summary>
        /// 加载允许转换类型
        /// </summary>
        /// <returns></returns>
        private static Dictionary<Type, RedisToValueHandle> LoadAllowConvert()
        {
            Dictionary<Type, RedisToValueHandle> dic = new Dictionary<Type, RedisToValueHandle>();
            dic[typeof(bool)] = ValueToBool;
            dic[typeof(byte)] = ValueToByte;
            dic[typeof(char)] = ValueToChar;
            dic[typeof(DateTime)] = ValueToDateTime;
            dic[typeof(decimal)] = ValueToDecimal;
            dic[typeof(double)] = ValueToDouble;
            dic[typeof(short)] = ValueToShort;
            dic[typeof(int)] = ValueToInt;
            dic[typeof(long)] = ValueToLong;
            dic[typeof(sbyte)] = ValueToSbyte;
            dic[typeof(float)] = ValueToFloat;
            dic[typeof(string)] = ValueToString;
            dic[typeof(ushort)] = ValueToUshort;
            dic[typeof(uint)] = ValueToUint;
            dic[typeof(ulong)] = ValueToUlong;
            dic[typeof(byte[])] = ValueToBytes;
            return dic;
        }

        
        /// <summary>
        /// RedisValue值转换成指定值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static E RedisValueToValue<E>(RedisValue value)
        {
            if (value.IsNullOrEmpty)
            {
                return default(E);
            }
            Type type = typeof(E);
            type = DefaultType.GetNullableRealType(type);
            RedisToValueHandle handle=null;
            if (_dicAllowConvert.TryGetValue(type, out handle))
            {

                return (E)handle(value);
            }
            string json = value.ToString();
            return JsonConvert.DeserializeObject<E>(json);
        }


        /// <summary>
        /// 值转换成RedisValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RedisValue ValueToRedisValue(object value)
        {
            if (value==null)
            {
                return RedisValue.Null;
            }
            Type type = value.GetType();
            type = DefaultType.GetNullableRealType(type);
            RedisConverteHandle handle = null;
            if (_dicRedisConvert.TryGetValue(type, out handle)) 
            {
                return handle(value);
            }
            string json = JsonConvert.SerializeObject(value);
            return json;
        }
    }
}
