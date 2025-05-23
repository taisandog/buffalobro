using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.Data.Oracle
{
    /// <summary>
    /// ������������
    /// </summary>
    public class AdapterLoader : IAdapterLoader
    {
        /// <summary>
        /// ��ǰ���ݿ�������
        /// </summary>
        public IDBAdapter DbAdapter 
        {
            get 
            {
                if(_version=="11" || _version == "12")
                {
                    return new DBAdapter11();
                }
                return new DBAdapter();
            }
        }

        public IAggregateFunctions AggregateFunctions 
        {
            get
            {
                return new AggregateFunctions();
            }
        }

        public IMathFunctions MathFunctions 
        {
            get 
            {
                return new MathFunctions();
            }
        }
        public IConvertFunction ConvertFunctions 
        {
            get 
            {
                return new ConvertFunction();
            }
        }
        public ICommonFunction CommonFunctions 
        {
            get 
            {
                return new CommonFunction();
            }
        }

        public IDBStructure DBStructure
        {
            get
            {
                return new DBStructure();
            }
        }
        private string _version;
        public void SetDBVersion(string version)
        {
            _version = version;
        }
    }
}
