using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using System.Data;
using Buffalo.DB.DbCommon;

#if Ora11
namespace Buffalo.Data.Oracle11
#elif Ora12
namespace Buffalo.Data.Oracle12
#endif
{
    /// <summary>
    /// 主键序列管理
    /// </summary>
    public class SequenceManager : Buffalo.DB.DataBaseAdapter.Oracle9Adapter.SequenceManager
    {
       

    }
}
