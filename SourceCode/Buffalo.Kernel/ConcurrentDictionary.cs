using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
#if(NET_2_0 || NET_3_5)
namespace System.Collections.Concurrent
{
    public class ConcurrentDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        
        public bool TryRemove(TKey key, out TValue value)
        {
            if (TryGetValue(key, out value)) 
            {
                return this.Remove(key);
            }
            return false;
        }

    }       
}
#endif