using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    /// <summary>
    /// ���Ե�ö����
    /// </summary>
    public class ClassPropertyEnumerator : DictionaryEnumerator<string,PropertyInfoHandle>
    {
        public ClassPropertyEnumerator(Dictionary<string,PropertyInfoHandle> dic)
            :base(dic)
        {

        }
    }
}
