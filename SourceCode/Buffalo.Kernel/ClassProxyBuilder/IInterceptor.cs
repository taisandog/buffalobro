using System;
using System.Collections.Generic;
using System.Text;
/** 
 * @原作者:benben
 * @创建时间:2012-2-19 09:02
 * @链接:http://www.189works.com/article-43203-1.html
 * @说明:.NET IL动态代理创建
*/
namespace Buffalo.Kernel.ClassProxyBuilder
{
    /// <summary>
    /// 拦截器接口
    /// </summary>
    public interface IInterceptor
    {
        /// <summary>
        /// 方法调用前
        /// </summary>
        /// <param name="operationName">方法名</param>
        /// <param name="inputs">参数</param>
        /// <returns>状态对象，用于调用后传入</returns>
        object BeforeCall(string operationName, object[] inputs);

        /// <summary>
        /// 方法调用后
        /// </summary>
        /// <param name="operationName">方法名</param>
        /// <param name="returnValue">结果</param>
        /// <param name="correlationState">状态对象</param>
        void AfterCall(object obj, string operationName, object returnValue, object correlationState);
        IInterceptor GetDefault();
    }
}
