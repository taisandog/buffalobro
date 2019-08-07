using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Buffalo.Kernel
{
    /// <summary>
    /// ���Զ��ͷŵ����������õȴ���ʱ
    /// </summary>
    public class Lock : IDisposable
    {
        /// <summary>
        /// Ĭ�ϳ�ʱ����
        /// </summary>
        public static int defaultMillisecondsTimeout = 15000; // 15S
        private object _obj;

        /// <summary>
        /// ���� 
        /// </summary>
        /// <param name="obj">��Ҫ��ס�Ķ���</param>
        public Lock(object obj)
        {
            TryGet(obj, defaultMillisecondsTimeout, true);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="obj">��Ҫ��ס�Ķ���</param>
        /// <param name="millisecondsTimeout">��ʱ����</param>
        public Lock(object obj, int millisecondsTimeout)
        {
            TryGet(obj, millisecondsTimeout, true);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="obj">��Ҫ��ס�Ķ���</param>
        /// <param name="millisecondsTimeout">��ʱ����</param>
        /// <param name="throwTimeoutException">�Ƿ��׳���ʱ�쳣</param>
        public Lock(object obj, int millisecondsTimeout, bool throwTimeoutException)
        {
            TryGet(obj, millisecondsTimeout, throwTimeoutException);
        }

        private void TryGet(object obj, int millisecondsTimeout, bool throwTimeoutException)
        {
            if (Monitor.TryEnter(obj, millisecondsTimeout))
            {
                _obj = obj;
            }
            else
            {
                if (throwTimeoutException)
                {
                    throw new TimeoutException("������:"+obj.ToString()+"�Ѿ���ʱ");
                }
            }
        }

        /// <summary>
        /// ���٣����ͷ���
        /// </summary>
        public void Dispose()
        {
            if (_obj != null)
            {
                Monitor.Exit(_obj);
            }
        }

        /// <summary>
        /// ��ȡ�ڻ�ȡ��ʱ�Ƿ����ȴ���ʱ
        /// </summary>
        public bool IsTimeout
        {
            get
            {
                return _obj == null;
            }
        }
    }
}
