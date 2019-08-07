using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

#if Ora11
namespace Buffalo.Data.Oracle11
#elif Ora12
namespace Buffalo.Data.Oracle12
#endif
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
