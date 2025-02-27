using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Buffalo.DB.EntityInfos;

namespace MemcacheClient
{
    
    /// <summary>
    /// 读写器的类型项
    /// </summary>
    public class MemTypeItem
    {


        /// <summary>
        /// 读写器的类型项
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="itemType"></param>
        /// <param name="readHandle"></param>
        /// <param name="writerHandle"></param>
        internal MemTypeItem(int typeID, Type itemType, ReadInfo readHandle, WriteInfo writerHandle)
        {
            _typeID = typeID;
            _itemType = itemType;
            _readHandle = readHandle;
            _writerHandle = writerHandle;
        }

       
        /// <summary>
        /// 数值类型ID
        /// </summary>
        private int _typeID;

        public int TypeID
        {
            get { return _typeID; }
        }

        private Type _itemType;
        /// <summary>
        /// 类型
        /// </summary>
        public Type ItemType
        {
            get { return _itemType; }
        }

        private ReadInfo _readHandle;
        /// <summary>
        /// 读取方法
        /// </summary>
        public ReadInfo ReadHandle
        {
            get { return _readHandle; }
        }

        private WriteInfo _writerHandle;
        /// <summary>
        /// 写入方法
        /// </summary>
        public WriteInfo WriterHandle
        {
            get { return _writerHandle; }
        }
    }
}
