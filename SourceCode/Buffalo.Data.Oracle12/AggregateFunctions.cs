using System;
using System.Collections.Generic;
using System.Text;

#if Ora11
namespace Buffalo.Data.Oracle11
#elif Ora12
namespace Buffalo.Data.Oracle12
#endif
{
    /// <summary>
    /// 聚合函数处理
    /// </summary>
    public class AggregateFunctions: Buffalo.DB.DataBaseAdapter.Oracle9Adapter.AggregateFunctions
    {
        
    }
}
