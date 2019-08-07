using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Data.DB2
{
    public class MathFunctions :Buffalo.DB.DataBaseAdapter.Oracle9Adapter.MathFunctions
    {
        public override string DoRandom(string[] values)
        {
            return " rand()";
        }
    }
}
