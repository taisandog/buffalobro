using Buffalo.Kernel.FastReflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 实体事件工具
    /// </summary>
    public class ObjectEventsUnit
    {
        /// <summary>
        /// 缓存类型和拥有事件的集合
        /// </summary>
        private static ConcurrentDictionary<Type, List<SetFieldValueHandle>> _dicEventHandle = new ConcurrentDictionary<Type, List<SetFieldValueHandle>>();

        private const BindingFlags EventObjectAllFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
        /// <summary>
        /// 清除事件绑定的函数
        /// </summary>
        /// <param name="objectHasEvents">拥有事件的实例</param>
        public static void ClearAllEvents(object objectHasEvents)
        {
            if (objectHasEvents == null)
            {
                return;
            }
            try
            {
                Type objType = objectHasEvents.GetType();
                List<SetFieldValueHandle> lsthandle = null;
                if (!_dicEventHandle.TryGetValue(objType, out lsthandle))
                {
                    lsthandle = new List<SetFieldValueHandle>();
                    EventInfo[] events = objType.GetEvents(EventObjectAllFlag);
                    if (events != null && events.Length > 0)
                    {
                        for (int i = 0; i < events.Length; i++)
                        {
                            EventInfo ei = events[i];

                            FieldInfo fi = objType.GetField(ei.Name, EventObjectAllFlag);
                            if (fi == null)
                            {
                                continue;
                            }
                            SetFieldValueHandle handle = FastFieldGetSet.GetSetValueHandle(fi);
                            if (handle == null)
                            {
                                continue;
                            }
                            lsthandle.Add(handle);
                        }
                    }
                    _dicEventHandle[objType] = lsthandle;
                }


                foreach (SetFieldValueHandle handle in lsthandle)
                {
                    handle.Invoke(objectHasEvents, null);
                }


            }
            catch (Exception ex)
            {
            }
        }
    }
}
