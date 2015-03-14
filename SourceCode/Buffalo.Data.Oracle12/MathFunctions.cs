using System;
using System.Collections.Generic;
using System.Text;

#if Ora11
namespace Buffalo.Data.Oracle11
#elif Ora12
namespace Buffalo.Data.Oracle12
#endif
{
    public class MathFunctions : Buffalo.DB.DataBaseAdapter.Oracle9Adapter.MathFunctions
    {
        
    }
}
