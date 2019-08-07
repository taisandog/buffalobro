using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.DBCheckers;



#if Ora11
namespace Buffalo.Data.Oracle11
#elif Ora12
namespace Buffalo.Data.Oracle12
#endif
{
    /// <summary>
    /// 数据库结构特性类
    /// </summary>
    public class DBStructure : Buffalo.DB.DataBaseAdapter.Oracle9Adapter.DBStructure
    {
    }
}
