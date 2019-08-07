using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

#if Ora11
namespace Buffalo.Data.Oracle11
#elif Ora12
namespace Buffalo.Data.Oracle12
#endif
{
    /// <summary>
    /// 生成分页语句的类
    /// </summary>
    public class CutPageSqlCreater : Buffalo.DB.DataBaseAdapter.Oracle9Adapter.CutPageSqlCreater
    {
       
        
    
    }
}
