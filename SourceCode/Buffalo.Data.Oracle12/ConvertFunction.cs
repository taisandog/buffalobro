using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

#if Ora11
namespace Buffalo.Data.Oracle11
#elif Ora12
namespace Buffalo.Data.Oracle12
#endif
{
    /// <summary>
    /// ��ֵת������
    /// </summary>
    public class ConvertFunction : Buffalo.DB.DataBaseAdapter.Oracle9Adapter.ConvertFunction
    {

        
    }
}
