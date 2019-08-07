using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Buffalo.Kernel.FastReflection.ClassInfos;
using Buffalo.Kernel.FastReflection;
using Buffalo.WebKernel.WebCommons;
using System.Security.Permissions;

namespace Buffalo.WebControls.GroupControlLib
{
    
    public class ControlGroup :System.Web.UI.WebControls.Panel
    {
        public string groupName;

        /// <summary>
        /// �ؼ�����
        /// </summary>
        [Bindable(true),Description("�ؼ�����")]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Localizable(true)]
        public string GroupName 
        {
            get 
            {
                return groupName;
            }
            set 
            {
                groupName = value;
            }
        }

        /// <summary>
        /// �Ƿ��Ѿ����ù�
        /// </summary>
        private bool IsSet 
        {
            get 
            {
                return ViewState["IsSet"] != null;
            }
            set 
            {
                ViewState["IsSet"] = value;
            }
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsSet)
            {
                this.Visible = GetGroupVisable();
                IsSet = true;
            }
        }
        /// <summary>
        /// ��ȡ��ɼ���
        /// </summary>
        /// <returns></returns>
        private bool GetGroupVisable() 
        {

            Control ctr=null;
            ctr = Page;
            string gName = GroupVisableSetter.GetViewStateName(groupName);
            do
            {
                
                if (ctr != null)
                {
                    StateBag viewState = WebPageCommon.GetControlViewState(ctr) as StateBag;
                    if (viewState != null && viewState[gName] != null)
                    {
                        return (bool)viewState[gName];
                    }
                }
                ctr = ctr.Parent;
            } while (ctr != null);
            return true;
        }
    }
}
