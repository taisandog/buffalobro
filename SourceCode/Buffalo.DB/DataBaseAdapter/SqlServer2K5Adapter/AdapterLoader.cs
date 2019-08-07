using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2K5Adapter
{
    /// <summary>
    ///   ≈‰∆˜º”‘ÿ∆˜
    /// </summary>
    public class AdapterLoader : IAdapterLoader
    {
        /// <summary>
        /// µ±«∞ ˝æ›ø‚  ≈‰∆˜
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
