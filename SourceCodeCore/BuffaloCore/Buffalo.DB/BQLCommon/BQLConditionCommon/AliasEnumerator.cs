using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.FastReflection.ClassInfos;


namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// ���Ե�ö����
    /// </summary>
    internal class AliasEnumerator : DictionaryEnumerator<string, QueryParamCollection>
    {
        internal AliasEnumerator(Dictionary<string, QueryParamCollection> dic)
            :base(dic)
        {

        }
    }
}
