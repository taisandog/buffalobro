using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.DataBaseAdapter.IBMDB2V9Adapter
{
    public class MathFunctions :Buffalo.DB.DataBaseAdapter.Oracle9Adapter.MathFunctions
    {
        public override string DoRandom(string[] values)
        {
            return " rand()";
        }
    }
}
