using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections.Concurrent;

namespace Buffalo.Kernel.Defaults
{
    public class DefaultType
    {
        private static TypeItem[] dbTypeMapping;
        private static Dictionary<string, TypeItem> _dicDefaultValues = InitDefauleValues();


        private static Dictionary<string, TypeItem> InitDefauleValues()
        {
            Dictionary<string, TypeItem> ret = new Dictionary<string, TypeItem>();
            dbTypeMapping = new TypeItem[28];


            TypeItem item;


            item = new TypeItem(typeof(DateTime), DefaultValue.DefaultDateTime,DbType.DateTime);
            ret[item.ItemType.FullName]=item;
            dbTypeMapping[(int)item.DbType] = item;
            dbTypeMapping[(int)DbType.Date] = item;
            dbTypeMapping[(int)DbType.Time] = item;
            dbTypeMapping[26] = item;//DateTime2
            dbTypeMapping[27] = item;//DateTimeOffset

            

            item=new TypeItem(typeof(byte[]), null, DbType.Binary);
            ret[item.ItemType.FullName]=item;
            dbTypeMapping[(int)item.DbType] = item;

            


            item=new TypeItem(typeof(System.DBNull), System.DBNull.Value, DbType.Object);
            ret[item.ItemType.FullName] = item;
            dbTypeMapping[(int)item.DbType] = item;

            


            item=new TypeItem(typeof(Guid), DefaultValue.DefaultGuid,DbType.Guid);
            ret[item.ItemType.FullName] = item;
            dbTypeMapping[(int)item.DbType] = item;


            item=new TypeItem(typeof(int), DefaultValue.DefaultInt,DbType.Int32);
            ret[item.ItemType.FullName] = item;
            dbTypeMapping[(int)item.DbType] = item;


            item = new TypeItem(typeof(double), DefaultValue.DefaultDouble, DbType.Double);
            ret[item.ItemType.FullName] = item;
            dbTypeMapping[(int)item.DbType] = item;


            item = new TypeItem(typeof(float), DefaultValue.DefaultFloat, DbType.Single);
            ret[item.ItemType.FullName] = item;
            dbTypeMapping[(int)item.DbType] = item;


            item = new TypeItem(typeof(decimal), DefaultValue.DefaultDecimal, DbType.Decimal);
            ret[item.ItemType.FullName] = item;
            dbTypeMapping[(int)item.DbType] = item;
            dbTypeMapping[(int)DbType.Currency] = item;
            dbTypeMapping[(int)DbType.VarNumeric] = item;

            item = new TypeItem(typeof(sbyte), DefaultValue.DefaultSbyte, DbType.SByte);
            ret[item.ItemType.FullName] = item;
            dbTypeMapping[(int)item.DbType] = item;

            item = new TypeItem(typeof(byte), DefaultValue.DefaultSbyte, DbType.Byte);
            ret[item.ItemType.FullName] = item;
            dbTypeMapping[(int)item.DbType] = item;

            item = new TypeItem(typeof(short), DefaultValue.DefaultShort, DbType.Int16);
            ret[item.ItemType.FullName] = item;
            dbTypeMapping[(int)item.DbType] = item;


            item = new TypeItem(typeof(long), DefaultValue.DefaultLong, DbType.Int64);
            ret[item.ItemType.FullName] = item;
            dbTypeMapping[(int)item.DbType] = item;

            item = new TypeItem(typeof(string), null, DbType.String);
            ret[item.ItemType.FullName] = item;
            dbTypeMapping[(int)item.DbType] = item;
            dbTypeMapping[(int)DbType.StringFixedLength] = item;
            dbTypeMapping[(int)DbType.AnsiString] = item;
            dbTypeMapping[(int)DbType.AnsiStringFixedLength] = item;

            return ret;
        }

        /// <summary>
        /// ��ȡC#����
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetCSharpVariableType(DbType type)
        {
            int index = (int)type;
            if (index >= 0 && index < dbTypeMapping.Length) 
            {
                TypeItem item = dbTypeMapping[index];
                if (item != null) 
                {
                    return item.ItemType;
                }
            }
            return null;
        }

        /// <summary>
        /// �ж��Ƿ�Ĭ��ֵ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDefaultValue(object value) 
        {
            if (value == null) 
            {
                return true;
            }
            string typeName = value.GetType().FullName;
            TypeItem item = null;
            if (_dicDefaultValues.TryGetValue(typeName, out item)) 
            {
                return value.Equals(item.DefaultValue);
            }
            return false;
        }

        /// <summary>
        /// ��ȡ�����͵�Ĭ��ֵ
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static TypeItem GetDefaultValue(Type objType) 
        {
            string typeName = objType.FullName;
            TypeItem item = null;
            _dicDefaultValues.TryGetValue(typeName, out item);
            return item;
        }

        public readonly static Type IntType = typeof(int?);
        public readonly static Type DoubleType = typeof(double?);
        public readonly static Type StringType = typeof(string);
        public readonly static Type FloatType = typeof(float?);
        public readonly static Type DateTimeType = typeof(DateTime);
        public readonly static Type DecimalType = typeof(decimal?);
        public readonly static Type ByteType = typeof(byte?);
        public readonly static Type SbyteType = typeof(sbyte?);
        public readonly static Type ShortType = typeof(short?);
        public readonly static Type LongType = typeof(long?);
        public readonly static Type BytesType = typeof(byte[]);
        public readonly static Type BooleanType = typeof(bool?);
        public readonly static Type DBNullType = typeof(System.DBNull);
        public readonly static Type UShortType = typeof(ushort?);
        public readonly static Type UIntType = typeof(uint?);
        public readonly static Type ULongType = typeof(ulong?);
        public readonly static Type GUIDType = typeof(Guid);
        public readonly static Type NullableType = typeof(Nullable<>);
        /// <summary>
        /// �ж��Ƿ�����͵�����
        /// </summary>
        /// <param name="type">��ǰ��</param>
        /// <param name="baseType">����</param>
        /// <returns></returns>
        public static bool IsInherit(Type type, Type baseType)
        {
            if (type.BaseType == null) return false;
            if (type.BaseType == baseType) return true;
            return IsInherit(type.BaseType, baseType);
        }

        /// <summary>
        /// �жϱ��������Ƿ������
        /// </summary>
        /// <param name="value"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static bool EqualType(Type vType, Type objType) 
        {

            if (vType == objType) 
            {
                return true;
            }
            Type rType = GetRealValueType(objType);
            if (vType == rType) 
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// ��ȡ��������ߵ�����
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static Type GetRealValueType(Type dataType) 
        {
            if (dataType.IsGenericType)
            {
                Type[] genTypes = dataType.GetGenericArguments();
                if (genTypes.Length > 0)
                {
                    return genTypes[0];
                }
            }
            return dataType;
        }

        /// <summary>
        /// ��ȡ���ͻ�Nullable����ߵ�����
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static Type GetNullableRealType(Type dataType)
        {
            if (dataType.IsGenericType && dataType.BaseType != null && dataType.GetGenericTypeDefinition() == NullableType)
            {
                Type[] genTypes = dataType.GetGenericArguments();
                if (genTypes.Length > 0)
                {
                    return genTypes[0];
                }
            }
            return dataType;
        }
        /// <summary>
        /// ��ȡ������ߵ�����
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isherit"></param>
        /// <returns></returns>
        public static Type[] GetGenericType(Type type,bool isherit) 
        {
            Type rootType = typeof(object);
            do
            {
                if (type.IsGenericType)
                {
                    return type.GetGenericArguments();
                }
                type = type.BaseType;
                if (type==null)
                {
                    break;
                }
            } while (isherit);
            return null;
        }

        /// <summary>
        /// �ж��Ƿ�ʵ���˸ýӿ�
        /// </summary>
        /// <param name="type">��ǰ��</param>
        /// <param name="interfaceType">�ӿ�����</param>
        /// <returns></returns>
        public static bool IsImplement(Type type, Type interfaceType)
        {
            if (!interfaceType.IsInterface) 
            {
                return false;
            }
            Type[] infTypes = type.GetInterfaces();
            foreach (Type objType in infTypes) 
            {
                if (objType == interfaceType) 
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ��C#����ת����SQL����
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public static DbType ToDbType(Type valueType)
        {
            TypeItem item = GetDefaultValue(valueType);
            if (item != null) 
            {
                return item.DbType;
            }
            return DbType.Object;
        }

    }
}
