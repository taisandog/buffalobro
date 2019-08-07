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
        /// ��ҳ���в��ҿؼ�(��ģ������)
        /// </summary>
        /// <param name="controlId">�ؼ�ID</param>
        /// <param name="currentPage">Ҫ���ҵ�ҳ��</param>
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
                if (currentPage.Master != null) //����Ǵ���ģ���ҳ�棬�Զ��������ģ�棬�ҳ��ؼ�
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
        private static PropertyInfoHandle pInfoHandle = FastValueGetSet.GetPropertyInfoHandle("ContentPlaceHolders", typeof(MasterPage));//����IL��ģ��������������ó���
        /// <summary>
        /// �ݹ��������ģ���������
        /// </summary>
        /// <param name="masterPage">��ǰģ��</param>
        /// <param name="id">�ؼ�ID</param>
        /// <param name="currentPage">Ҫ���ҵ�ҳ��</param>
        /// <returns></returns>
        private static Control FindControlInMaster(MasterPage masterPage, string id, Stack<IList> stkNames)
        {
            Control ctr = null;
            IList lst = (IList)pInfoHandle.GetValue(masterPage);
            stkNames.Push(lst);
            if (masterPage.Master != null) //���ģ��֮�ϻ���ģ����������
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
        /// ����ģ����ҿؼ�
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
        /// ��ȡ�ؼ���ViewState
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
