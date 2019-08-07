using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Buffalo.DB.EntityInfos;

namespace MemcacheClient
{
    
    /// <summary>
    /// ��д����������
    /// </summary>
    public class MemTypeItem
    {


        /// <summary>
        /// ��д����������
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
        /// ��ֵ����ID
        /// </summary>
        private int _typeID;

        public int TypeID
        {
            get { return _typeID; }
        }

        private Type _itemType;
        /// <summary>
        /// ����
        /// </summary>
        public Type ItemType
        {
            get { return _itemType; }
        }

        private ReadInfo _readHandle;
        /// <summary>
        /// ��ȡ����
        /// </summary>
        public ReadInfo ReadHandle
        {
            get { return _readHandle; }
        }

        private WriteInfo _writerHandle;
        /// <summary>
        /// д�뷽��
        /// </summary>
        public WriteInfo WriterHandle
        {
            get { return _writerHandle; }
        }
    }
}
