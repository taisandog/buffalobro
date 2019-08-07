using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

using Buffalo.DB.CommBase;
using Buffalo.DB.DataFillers;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;

#if Ora11
namespace Buffalo.Data.Oracle11
#elif Ora12
namespace Buffalo.Data.Oracle12
#endif
{
    /// <summary>
    /// ”Œ±Í∑÷“≥
    /// </summary>
    public class CursorPageCutter : Buffalo.DB.DataBaseAdapter.Oracle9Adapter.CursorPageCutter
    {
        
    }
}
