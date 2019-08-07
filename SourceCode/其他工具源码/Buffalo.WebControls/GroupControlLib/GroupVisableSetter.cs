using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Buffalo.WebControls.GroupControlLib
{
    public class GroupVisableSetter:System.Web.UI.Page
    {
        /// <summary>
        /// ��ȡViewState����
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        internal static string GetViewStateName(string groupName) 
        {
            return "GroupVisable_" + groupName;
        }
        /// <summary>
        /// ������Ŀɼ���
        /// </summary>
        /// <param name="groupNam"></param>
        /// <returns></returns>
        public bool this[string groupName] 
        {
            get 
            {
                return ViewState[GetViewStateName(groupName)] == null ? false : (bool)ViewState[GetViewStateName(groupName)];
            }
            set 
            {

                ViewState[GetViewStateName(groupName)] = value;
            }
        }

        /// <summary>
        /// ��������ɼ���
        /// </summary>
        /// <param name="viewState">ViewState</param>
        /// <param name="groupName">����</param>
        /// <param name="visable">�ɼ���</param>
        public static void SetGroupVisable(StateBag viewState,string groupName,bool visable)
        {
            viewState[GetViewStateName(groupName)] = visable;
        }
        /// <summary>
        /// ��ȡ����ɼ���
        /// </summary>
        /// <param name="viewState">ViewState</param>
        /// <param name="groupName">����</param>
        public static bool GetGroupVisable(StateBag viewState, string groupName)
        {
            return viewState[GetViewStateName(groupName)] == null ? false : (bool)viewState[GetViewStateName(groupName)];
        }
    }
}
