using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Buffalo.Kernel.FastReflection
{
    public static class CallContext
    {
        static ConcurrentDictionary<string, ThreadLocal<object>> _state = new ConcurrentDictionary<string, ThreadLocal<object>>();

        /// <summary>
        /// Stores a given object and associates it with the specified name.
        /// </summary>
        /// <param name="name">The name with which to associate the new item in the call context.</param>
        /// <param name="data">The object to store in the call context.</param>
        public static void SetData(string name, object data)
        {
            ThreadLocal<object> item = new ThreadLocal<object>();
            item.Value = data;
            _state[name] = item;
        }

        /// <summary>
        /// Retrieves an object with the specified name from the <see cref="CallContext"/>.
        /// </summary>
        /// <param name="name">The name of the item in the call context.</param>
        /// <returns>The object in the call context associated with the specified name, or <see langword="null"/> if not found.</returns>
        public static object GetData(string name)
        {
            ThreadLocal<object> data = null;
            if(!_state.TryGetValue(name, out data))
            {
                return null;
            }
            return data.Value;
        }
    }
}
