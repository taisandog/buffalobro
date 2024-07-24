using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.Kernel;
using System.Data.Common;
using System.Collections;

namespace Buffalo.DB.CacheManager
{
    /// <summary>
    /// 内存数据行读取器
    /// </summary>
    public class MemCacheReader : DbDataReader,IDataReader
    {
        /// <summary>
        /// 数据
        /// </summary>
        private DataSet _data;
        /// <summary>
        /// 当前数据索引
        /// </summary>
        private int _currentIndex = 0;
        /// <summary>
        /// 当前数据表
        /// </summary>
        private DataTable _currentData;
        /// <summary>
        /// 当前数据表的索引
        /// </summary>
        private int _currentRowIndex = -1;
        /// <summary>
        /// 当前行
        /// </summary>
        private DataRow _currentRow;

        public MemCacheReader(DataSet ds)
        {
            _data = ds;
            _currentData = _data.Tables[_currentIndex];
        }

        #region IDataReader 成员

        

        public override int Depth
        {
            get { return 1; }
        }

        public override DataTable GetSchemaTable()
        {
            return null;
        }

        public override bool IsClosed
        {
            get { return false; }
        }

        public override bool NextResult()
        {
            if (_data.Tables.Count < _currentIndex + 2)
            {
                return false;
            }
            _currentIndex++;
            _currentData = _data.Tables[_currentIndex];
            _currentRowIndex = -1;
            _currentRow = null;
            return true;
        }

        public override bool Read()
        {
            if (!HasRows)
            {
                return false;
            }
            _currentRowIndex++;
            _currentRow = _currentData.Rows[_currentRowIndex];
            return true;
        }

        public override int RecordsAffected
        {
            get
            {
                return 0;
            }
        }

        #endregion

        #region IDisposable 成员

       

        #endregion

        #region IDataRecord 成员

        public override int FieldCount
        {
            get
            {
                return _currentData.Columns.Count;
            }
        }

        public override bool GetBoolean(int i)
        {
            return (bool)_currentRow[i];
        }

        public override byte GetByte(int i)
        {
            return (byte)_currentRow[i];
        }

        public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            byte[] arr = (byte[])_currentRow[i];
            Array.Copy(arr, (int)fieldOffset, buffer, bufferoffset, length);
            return length;
        }

        public override char GetChar(int i)
        {
            return (char)_currentRow[i];
        }

        public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            char[] arr = (char[])_currentRow[i];
            Array.Copy(arr, (int)bufferoffset, buffer, bufferoffset, length);
            return length;
        }

       

        public override string GetDataTypeName(int i)
        {
            return _currentData.Columns[i].DataType.FullName;
        }

        public override DateTime GetDateTime(int i)
        {
            return (DateTime)_currentRow[i];
        }

        public override decimal GetDecimal(int i)
        {
            return (decimal)_currentRow[i];
        }

        public override double GetDouble(int i)
        {
            return (double)_currentRow[i];
        }

        public override Type GetFieldType(int i)
        {
            return _currentData.Columns[i].DataType;
        }

        public override float GetFloat(int i)
        {
            return (float)_currentRow[i];
        }

        public override Guid GetGuid(int i)
        {
            return (Guid)_currentRow[i];
        }

        public override short GetInt16(int i)
        {
            return (short)_currentRow[i];
        }

        public override int GetInt32(int i)
        {
            return (int)_currentRow[i];
        }

        public override long GetInt64(int i)
        {
            return (long)_currentRow[i];
        }

        public override string GetName(int i)
        {
            return _currentData.Columns[i].ColumnName;
        }

        public override int GetOrdinal(string name)
        {
            for (int i = 0; i < _currentData.Columns.Count; i++)
            {
                DataColumn col = _currentData.Columns[i];
                if (col.ColumnName == name)
                {
                    return i;
                }
            }
            return -1;
        }

        public override string GetString(int i)
        {
            return (string)_currentRow[i];
        }

        public override object GetValue(int i)
        {
            return _currentRow[i];
        }

        public override int GetValues(object[] values)
        {
            for (int i = 0; i < _currentData.Columns.Count; i++)
            {
                if (i >= values.Length)
                {
                    return values.Length;
                }
                values[i] = _currentRow[i];
            }
            return _currentData.Columns.Count;
        }

        public override bool IsDBNull(int i)
        {
            return _currentRow[i] is DBNull;
        }

        public override object this[string name]
        {
            get
            {
                return _currentRow[name];
            }
        }

        public override object this[int i]
        {
            get
            {
                return _currentRow[i];
            }
        }

        public override IEnumerator GetEnumerator()
        {
            return _currentData.Rows.GetEnumerator();
        }
        public override bool HasRows
        {
            get 
            {
                return _currentData.Rows.Count > _currentRowIndex + 1;
            }
        }
        #endregion
    }
}
