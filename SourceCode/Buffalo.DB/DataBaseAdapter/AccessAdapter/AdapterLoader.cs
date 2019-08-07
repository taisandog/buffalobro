using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.DB.DataBaseAdapter.AccessAdapter
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

    }
}
