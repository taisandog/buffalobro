using System;
using System.Collections.Generic;
using System.Text;
/** 
 * @ԭ����:benben
 * @����ʱ��:2012-2-19 09:02
 * @����:http://www.189works.com/article-43203-1.html
 * @˵��:.NET IL��̬������
*/
namespace Buffalo.Kernel.ClassProxyBuilder
{
    /// <summary>
    /// �������ӿ�
    /// </summary>
    public interface IInterceptor
    {
        /// <summary>
        /// ��������ǰ
        /// </summary>
        /// <param name="operationName">������</param>
        /// <param name="inputs">����</param>
        /// <returns>״̬�������ڵ��ú���</returns>
        object BeforeCall(string operationName, object[] inputs);

        /// <summary>
        /// �������ú�
        /// </summary>
        /// <param name="operationName">������</param>
        /// <param name="returnValue">���</param>
        /// <param name="correlationState">״̬����</param>
        void AfterCall(object obj, string operationName, object returnValue, object correlationState);
        IInterceptor GetDefault();
    }
}
