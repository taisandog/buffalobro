using System.Threading;

namespace Buffalo.Kernel.FastReflection
{
    /// <summary>
    /// 仅用于同步调用链的线程上下文变量。
    /// </summary>
    /// <typeparam name="T">变量类型。</typeparam>
    public class CallContextSync<T>
    {
        private readonly ThreadLocal<T> _value = new ThreadLocal<T>();

        public T Value
        {
            get { return _value.Value; }
            set { _value.Value = value; }
        }
    }

    /// <summary>
    /// 仅用于异步调用链的上下文变量。
    /// </summary>
    /// <typeparam name="T">变量类型。</typeparam>
    public class CallContextAsync<T>
    {
        private readonly AsyncLocal<T> _value = new AsyncLocal<T>();

        public T Value
        {
            get { return _value.Value; }
            set { _value.Value = value; }
        }
    }
}
