using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    /// <summary>
    /// 属性的枚举类
    /// </summary>
    public class ClassFieldEnumerator : DictionaryEnumerator<string, FieldInfoHandle>
    {
        public ClassFieldEnumerator(Dictionary<string, FieldInfoHandle> dic)
            :base(dic)
        {

        }
    }
}
