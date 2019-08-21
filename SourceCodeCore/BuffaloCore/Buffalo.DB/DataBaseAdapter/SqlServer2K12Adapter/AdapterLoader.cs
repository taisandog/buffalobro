using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2K12Adapter
{
    public class AdapterLoader: IAdapterLoader
    {
        /// <summary>
        /// 当前数据库适配器
        /// </summary>
        public IDBAdapter DbAdapter 
        {
            get 
            {
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
                return new Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.MathFunctions();
            }
        }
        public IConvertFunction ConvertFunctions 
        {
            get 
            {
                return new Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.ConvertFunction();
            }
        }
        public ICommonFunction CommonFunctions 
        {
            get 
            {
                return new Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.CommonFunction();
            }
        }

        public IDBStructure DBStructure
        {
            get
            {
                return new DBStructure();
            }
        }

    }
}


