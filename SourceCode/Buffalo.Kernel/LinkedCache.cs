using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 带过期时间保存了活跃度的Dictionary
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public class LinkedCache<T, K> : LinkedDictionary<T, K>
    {
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetValue(T key, K value)
        {

        }
    }
}
