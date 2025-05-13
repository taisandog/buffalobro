
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.IOCP
{
    /// <summary>
    /// 清空事件
    /// </summary>
    public class EventHandleClean
    {
        private static ConcurrentDictionary<Type, List<FieldInfo>> _dicEventHandle = new ConcurrentDictionary<Type, List<FieldInfo>>();

        private const BindingFlags AllFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
        /// <summary>
        /// 清除事件绑定的函数
        /// </summary>
        /// <param name="objectHasEvents">拥有事件的实例</param>
        /// <param name="eventName">事件名称</param>
        public static void ClearAllEvents(object objectHasEvents)
        {
            if (objectHasEvents == null)
            {
                return;
            }
            try
            {
                Type objType = objectHasEvents.GetType();
                List<FieldInfo> lsthandle = null;
                if (!_dicEventHandle.TryGetValue(objType, out lsthandle))
                {
                    lsthandle = new List<FieldInfo>();
                    EventInfo[] events = objType.GetEvents(AllFlag);
                    if (events != null && events.Length > 0)
                    {
                        for (int i = 0; i < events.Length; i++)
                        {
                            EventInfo ei = events[i];

                            FieldInfo fi = objType.GetField(ei.Name, AllFlag);
                            if (fi == null)
                            {
                                continue;
                            }
                            //SetFieldValueHandle handle = FastFieldGetSet.GetSetValueHandle(fi);
                            //if (handle == null)
                            //{
                            //    continue;
                            //}
                            lsthandle.Add(fi);
                        }
                    }
                    _dicEventHandle[objType] = lsthandle;
                }
                

                foreach(FieldInfo handle in lsthandle)
                {
                    handle.SetValue(objectHasEvents, null);
                }

                
            }
            catch(Exception ex)
            {
            }
        }
    }
}
