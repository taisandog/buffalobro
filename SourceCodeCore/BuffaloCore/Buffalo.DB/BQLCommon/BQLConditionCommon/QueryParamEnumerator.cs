using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.FastReflection.ClassInfos;


namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// ���Ե�ö����
    /// </summary>
    internal class QueryParamEnumerator : DictionaryEnumerator<string, ParamInfo>
    {
        public QueryParamEnumerator(Dictionary<string, ParamInfo> dic)
            :base(dic)
        {

        }
    }
}
