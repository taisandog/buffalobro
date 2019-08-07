using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using System.Data;

#if Ora11
namespace Buffalo.Data.Oracle11
#elif Ora12
namespace Buffalo.Data.Oracle12
#endif
{

    public class CommonFunction : Buffalo.DB.DataBaseAdapter.Oracle9Adapter.CommonFunction
    {
        

    }
}
