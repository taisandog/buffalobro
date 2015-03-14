using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    /// <summary>
    /// 属性的枚举类
    /// </summary>
    public class ClassPropertyEnumerator : DictionaryEnumerator<string,PropertyInfoHandle>
    {
        public ClassPropertyEnumerator(Dictionary<string,PropertyInfoHandle> dic)
            :base(dic)
        {

        }
    }
}
