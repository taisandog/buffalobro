using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.Data.MySQL
{

    public class CommonFunction :ICommonFunction
    {
        public virtual string IsNull(string[] values)
        {
            return "isNull(" + values[0] + "," + values[1] + ")";
        }

        public virtual string Len(string[] values)
        {
            return "LENGTH(" + values[0] + ")";
        }

        public virtual string Distinct(string[] values)
        {
            StringBuilder sbValues = new StringBuilder();
            foreach (string value in values)
            {
                sbValues.Append(value + ",");
            }

            if (sbValues.Length > 0)
            {
                sbValues.Remove(sbValues.Length - 1, 1);
            }

            return "distinct " + sbValues.ToString() + " ";
        }


    }
}
