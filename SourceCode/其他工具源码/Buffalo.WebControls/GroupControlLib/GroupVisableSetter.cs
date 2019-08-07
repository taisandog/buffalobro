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
        /// 获取ViewState名字
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        internal static string GetViewStateName(string groupName) 
        {
            return "GroupVisable_" + groupName;
        }
        /// <summary>
        /// 设置组的可见性
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
        /// 设置其组可见性
        /// </summary>
        /// <param name="viewState">ViewState</param>
        /// <param name="groupName">组名</param>
        /// <param name="visable">可见性</param>
        public static void SetGroupVisable(StateBag viewState,string groupName,bool visable)
        {
            viewState[GetViewStateName(groupName)] = visable;
        }
        /// <summary>
        /// 获取其组可见性
        /// </summary>
        /// <param name="viewState">ViewState</param>
        /// <param name="groupName">组名</param>
        public static bool GetGroupVisable(StateBag viewState, string groupName)
        {
            return viewState[GetViewStateName(groupName)] == null ? false : (bool)viewState[GetViewStateName(groupName)];
        }
    }
}
