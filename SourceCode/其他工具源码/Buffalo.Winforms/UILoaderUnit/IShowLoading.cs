using System;
using System.Collections.Generic;

using System.Text;

namespace Buffalo.Winforms.UILoaderUnit
{
    public interface IShowLoading
    {
        /// <summary>
        /// 显示加载框
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        object ShowLoading(AsynMethodHandle method,object[] args) ;
        /// <summary>
        /// 显示加载框
        /// </summary>
        void HideLoading();
    }
}
