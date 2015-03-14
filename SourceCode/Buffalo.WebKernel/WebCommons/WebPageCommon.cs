using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web.UI;
using Buffalo.Kernel.FastReflection;

namespace Buffalo.WebKernel.WebCommons
{
    public class WebPageCommon
    {
        /// <summary>
        /// 在页面中查找控件(跨模板容器)
        /// </summary>
        /// <param name="controlId">控件ID</param>
        /// <param name="currentPage">要查找的页面</param>
        /// <returns></returns>
        public static Control FindControl(string controlId, Control objControl)
        {
            if (controlId == null || controlId == "")
            {
                return null;
            }
            Control ret = null;
            Page currentPage = objControl as Page;
            if (currentPage!=null)
            {
                if (currentPage.Master != null) //如果是带有模版的页面，自动穷举所有模版，找出控件
                {
                    ret = FindControlInMaster(currentPage.Master, controlId, new Stack<IList>());
                }
            }
            if (ret == null)
            {
                Control ctr = objControl;
                while (ctr != null)
                {
                    ret = objControl.FindControl(controlId);
                    if (ret != null) 
                    {
                        break;
                    }
                    ctr = ctr.Parent;
                }
            }
            return ret;
        }
        private static PropertyInfoHandle pInfoHandle = FastValueGetSet.GetPropertyInfoHandle("ContentPlaceHolders", typeof(MasterPage));//利用IL把模版了类的子容器拿出来
        /// <summary>
        /// 递归穷举所有模板里的容器
        /// </summary>
        /// <param name="masterPage">当前模板</param>
        /// <param name="id">控件ID</param>
        /// <param name="currentPage">要查找的页面</param>
        /// <returns></returns>
        private static Control FindControlInMaster(MasterPage masterPage, string id, Stack<IList> stkNames)
        {
            Control ctr = null;
            IList lst = (IList)pInfoHandle.GetValue(masterPage);
            stkNames.Push(lst);
            if (masterPage.Master != null) //如果模板之上还有模板则继续穷举
            {
                ctr = FindControlInMaster(masterPage.Master, id, stkNames);
            }
            else
            {
                Queue<Control> nextMaster = new Queue<Control>();
                nextMaster.Enqueue(masterPage);
                ctr = FindInChildMaster(nextMaster, stkNames, id);
            }
            return ctr;
        }
        /// <summary>
        /// 在字模板查找控件
        /// </summary>
        /// <param name="masterPages"></param>
        /// <param name="stkNames"></param>
        /// <param name="contorlId"></param>
        /// <returns></returns>
        private static Control FindInChildMaster(Queue<Control> containers, Stack<IList> stkNames, string contorlId)
        {
            IList lst = null;
            if (stkNames.Count > 0)
            {
                lst = stkNames.Pop();
            }
            Control ctr = null;
            Queue<Control> nextMaster = new Queue<Control>();

            foreach (Control container in containers)
            {
                ctr = container.FindControl(contorlId);
                if (ctr != null)
                {
                    return ctr;
                }
                if (lst != null)
                {
                    foreach (object objContent in lst)
                    {
                        Control crtContainer = container.FindControl(objContent.ToString());
                        if (crtContainer != null)
                        {
                            nextMaster.Enqueue(crtContainer);
                        }
                    }
                }

            }
            if (nextMaster.Count > 0)
            {
                ctr = FindInChildMaster(nextMaster, stkNames, contorlId);
            }
            return ctr;
        }

        private static GetFieldValueHandle viewInfo = FastFieldGetSet.GetGetValueHandle(typeof(Control), "_viewState");
        /// <summary>
        /// 获取控件的ViewState
        /// </summary>
        /// <param name="ctr"></param>
        /// <returns></returns>
        public static StateBag GetControlViewState(Control ctr) 
        {
            if (ctr == null) 
            {
                return null;
            }
            StateBag _viewState = viewInfo(ctr) as StateBag;
            return _viewState;
        }
    }
}
