using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Buffalo.Kernel;

namespace MemcacheClient
{
    public delegate object ReadInfo(BinaryReader reader);
    public delegate void WriteInfo(BinaryWriter writer,object value);
    /// <summary>
    /// 读写器类型管理器
    /// </summary>
    public class MemTypeManager
    {
        private static Dictionary<int, MemTypeItem> _dicMemTypeItem = null;
        private static Dictionary<string, MemTypeItem> _dicMemTypeItemByFullName = null;

        private static bool _isInit=InitTypes();

        private static bool InitTypes()
        {
            _dicMemTypeItem = new Dictionary<int, MemTypeItem>();
            _dicMemTypeItemByFullName = new Dictionary<string, MemTypeItem>();
            MemTypeItem item = null;
            AddItem<bool>(1, ReadBoolean,WriteBoolean);

            AddItem<short>(2, ReadInt16,WriteInt16);
            AddItem<int>(3, ReadInt, WriteInt);
            AddItem<long>(4, ReadInt64, WriteInt64);

            AddItem<ushort>(5, ReadUInt16, WriteUInt16);
            AddItem<uint>(6, ReadUInt, WriteUInt);
            AddItem<ulong>(7, ReadUInt64, WriteUInt64);

            AddItem<byte>(8, ReadByte, WriteByte);
            AddItem<byte[]>(9, ReadBytes, WriteBytes);
            AddItem<char>(10, ReadChar,WriteChar);
            AddItem<char[]>(11, ReadChars,WriteChars);
            AddItem<decimal>(12, ReadDecimal,WriteDecimal);
            AddItem<double>(13, ReadDouble,WriteDouble);
            AddItem<sbyte>(14, ReadSByte,WriteSByte);
            AddItem<float>(15, ReadSingle,WriteSingle);
            
            AddItem<string>(16, ReadString, WriteString);
            
            AddItem<DateTime>(17, ReadDateTime, WriteDateTime);
            AddItem<TimeSpan>(18, ReadTimeSpan, WriteTimeSpan);
            return true;
        }

        /// <summary>
        /// 获取类型信息
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static MemTypeItem GetTypeInfo(Type objType) 
        {
            string key = objType.FullName;
            MemTypeItem item = null;
            if (_dicMemTypeItemByFullName.TryGetValue(key, out item)) 
            {
                return item;
            }
            return null;
        }
        /// <summary>
        /// 根据类型ID获取类型信息
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <returns></returns>
        public static MemTypeItem GetTypeByID(int typeID)
        {
            
            MemTypeItem item = null;
            if (_dicMemTypeItem.TryGetValue(typeID, out item))
            {
                return item;
            }
            return null;
        }
        /// <summary>
        /// 添加项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeID">分配的ID</param>
        /// <param name="readInfo">读取器</param>
        private static void AddItem<T>(int typeID, ReadInfo readInfo,WriteInfo writeInfo)
        {
            MemTypeItem item = new MemTypeItem(typeID, typeof(T), readInfo, writeInfo);
            _dicMemTypeItem[typeID] = item;
            _dicMemTypeItemByFullName[item.ItemType.FullName] = item;
        }
        public static object ReadBoolean(BinaryReader reader)
        {
            bool isNull = reader.ReadBoolean();
            if (isNull) 
            {
                return null;
            }
            return reader.ReadBoolean();
        }
        public static void WriteBoolean(BinaryWriter writer, object value) 
        {
            bool isNull = value == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            writer.Write((bool)value);
        }
        public static object ReadInt16(BinaryReader reader)
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            return reader.ReadInt16();
        }
        public static void WriteInt16(BinaryWriter writer, object value)
        {
            bool isNull = value == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            writer.Write((short)value);
        }
        public static object ReadDateTime(BinaryReader reader)
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            double time = reader.ReadDouble();
            DateTime dt = CommonMethods.ConvertIntDateTime(time);
            return dt;
        }
        public static void WriteDateTime(BinaryWriter writer, object value)
        {
            bool isNull = value == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            double time = CommonMethods.ConvertDateTimeInt((DateTime)value);
            writer.Write(time);
        }
        public static object ReadTimeSpan(BinaryReader reader)
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            double time = reader.ReadDouble();
            return TimeSpan.FromSeconds(time);
            
        }
        public static void WriteTimeSpan(BinaryWriter writer, object value)
        {
            bool isNull = value == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            TimeSpan ts = (TimeSpan)value;
            double time = ts.TotalSeconds;
            writer.Write(time);
        }
        public static object ReadInt(BinaryReader reader)
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            return reader.ReadInt32();
        }
        public static void WriteInt(BinaryWriter writer, object value)
        {
            bool isNull = value == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            writer.Write((int)value);
        }
        public static object ReadInt64(BinaryReader reader)
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            return reader.ReadInt64();
        }
        public static void WriteInt64(BinaryWriter writer, object value)
        {
            bool isNull = value == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            writer.Write((long)value);
        }
        public static object ReadUInt16(BinaryReader reader)
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            return reader.ReadUInt16();
        }
        public static void WriteUInt16(BinaryWriter writer, object value)
        {
            bool isNull = value == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            writer.Write((ushort)value);
        }
        public static object ReadUInt(BinaryReader reader)
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            return reader.ReadUInt32();
        }
        public static void WriteUInt(BinaryWriter writer, object value)
        {
            bool isNull = value == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            writer.Write((uint)value);
        }
        public static object ReadUInt64(BinaryReader reader)
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            return reader.ReadUInt64();
        }
        public static void WriteUInt64(BinaryWriter writer, object value)
        {
            bool isNull = value == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            writer.Write((ulong)value);
        }
        public static object ReadByte(BinaryReader reader) 
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            return reader.ReadByte();
        }
        public static void WriteByte(BinaryWriter writer, object value)
        {
            bool isNull = value == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            writer.Write((byte)value);
        }
        //public static object ReadBytes(BinaryReader reader) { return reader.ReadBytes(); }
        public static object ReadChar(BinaryReader reader) 
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            return reader.ReadChar(); 
        }
        public static void WriteChar(BinaryWriter writer, object value)
        {
            bool isNull = value == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            writer.Write((char)value);
        }
        //public static object ReadChars(BinaryReader reader) { return reader.ReadChars(); }
        public static object ReadDecimal(BinaryReader reader) 
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            return reader.ReadDecimal(); 
        }
        public static void WriteDecimal(BinaryWriter writer, object value)
        {
            bool isNull = value == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            writer.Write((decimal)value);
        }
        public static object ReadDouble(BinaryReader reader) 
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            return reader.ReadDouble(); 
        }
        public static void WriteDouble(BinaryWriter writer, object value)
        {
            bool isNull = value == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            writer.Write((double)value);
        }
        public static object ReadSByte(BinaryReader reader)
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            return reader.ReadSByte(); 
        }
        public static void WriteSByte(BinaryWriter writer, object value)
        {
            bool isNull = value == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            writer.Write((sbyte)value);
        }
        public static object ReadSingle(BinaryReader reader)
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            return reader.ReadSingle();
        }
        public static void WriteSingle(BinaryWriter writer, object value)
        {
            bool isNull = value == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            writer.Write((float)value);
        }

        public static object ReadString(BinaryReader reader) 
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            int len = reader.ReadInt32();
            byte[] buffer = reader.ReadBytes(len);
            return MemDataSerialize.DefaultEncoding.GetString(buffer); 
        }
        /// <summary>
        /// 读取字节数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static object ReadChars(BinaryReader reader)
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            int len = reader.ReadInt32();
            char[] buffer = reader.ReadChars(len);

            return buffer;
        }
        /// <summary>
        /// 读取字节数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static object ReadBytes(BinaryReader reader)
        {
            bool isNull = reader.ReadBoolean();
            if (isNull)
            {
                return null;
            }
            int len = reader.ReadInt32();
            byte[] buffer = reader.ReadBytes(len);

            return buffer;
        }



        public static void WriteString(BinaryWriter writer, object value)
        {
            
            string str = value as string;

            bool isNull = str == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }

            byte[] buffer = MemDataSerialize.DefaultEncoding.GetBytes(str);
            writer.Write(buffer.Length);
            writer.Write(buffer);
        }

        /// <summary>
        /// 写入值
        /// </summary>
        /// <param name="writer">写入器</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void WriteChars(BinaryWriter writer, object value)
            
        {

            char[] arr = value as char[];
            bool isNull = arr == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            writer.Write(arr.Length);
            writer.Write(arr);
        }
        /// <summary>
        /// 写入值
        /// </summary>
        /// <param name="writer">写入器</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void WriteBytes(BinaryWriter writer, object value)
        {
            byte[] arr = value as byte[];
            bool isNull = arr == null;
            writer.Write(isNull);//写入是否为空
            if (isNull)
            {
                return;
            }
            writer.Write(arr.Length);
            writer.Write(arr);
        }

    }
}
