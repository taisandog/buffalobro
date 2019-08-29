
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Buffalo.ArgCommon;
using Buffalo.Storage.Win32;


namespace Buffalo.Storage.LocalFileManager
{
    /// <summary>
    /// 文件登录访问API
    /// </summary>
    public class FileAPI
    {
        [DllImport("mpr.dll", EntryPoint = "WNetAddConnection2")]
        private static extern uint WNetAddConnection2(NetResource lpNetResource, string lpPassword, string lpUsername, uint dwFlags);

        [DllImport("Mpr.dll", EntryPoint = "WNetCancelConnection2")]
        private static extern uint WNetCancelConnection2(string lpName, uint dwFlags, bool fForce);
        /// <summary>
        /// 永久性连接
        /// </summary>
        private const int CONNECT_UPDATE_PROFILE = 0x1;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpError">指定一个变量，用于装载网络错误代码。具体的代码由网络供应商决定 </param>
        /// <param name="lpErrorBuf">指定一个字串缓冲区，用于装载网络错误的说明 </param>
        /// <param name="nErrorBufSize">lpErrorBuf缓冲区包含的字符数量 </param>
        /// <param name="lpNameBuf">用于装载网络供应商名字的字串缓冲区 </param>
        /// <param name="nNameBufSize">lpNameBuf缓冲区的字符数量 </param>
        /// <returns></returns>
        [DllImport("Mpr.dll")]
        public static extern uint WNetGetLastError(ref int lpError, StringBuilder lpErrorBuf, int nErrorBufSize, StringBuilder lpNameBuf, int nNameBufSize);
        public static APIResault WNetAddConnection(string username, string password, string remoteName, string localName)
        {
            NetResource netResource = new NetResource();
            netResource.Scope = ResourceScope.GlobalNetwork; //RESOURCE_GLOBALNET
            netResource.ResourceType = ResourceType.Any; //RESOURCETYPE_ANY
            netResource.DisplayType = ResourceDisplaytype.Share; //RESOURCEDISPLAYTYPE_GENERIC
            netResource.Usage = ResourceUsage.CONNECTABLE; //RESOURCEUSAGE_CONNECTABLE
            netResource.LocalName = localName;
            netResource.RemoteName = remoteName.TrimEnd('/', '\\');
            
            //netResource.lpRemoteName = lpComment;
            //netResource.lpProvider = null;
            uint result = WNetAddConnection2(netResource, password, username, 0);
            if (result != ResultWin32.ERROR_SUCCESS && result != ResultWin32.ERROR_SESSION_CREDENTIAL_CONFLICT) 
            {
                string errName = ResultWin32.GetErrorName((int)result);
                //throw new StorageConnectExceptin("用户:" + username + " 登录 " + localName + " 失败:" + errName+"(0x"+result.ToString("X")+")");
                return ApiCommon.GetFault("用户:" + username + " 登录 " + localName + " 失败:" + errName, result);
            }
            return ApiCommon.GetSuccess();
        }
        public static APIResault WNetCancelConnection(string name, uint flags, bool force)
        {
            uint result = WNetCancelConnection2(name, flags, force);
            if (result != ResultWin32.ERROR_SUCCESS && result != ResultWin32.ERROR_SESSION_CREDENTIAL_CONFLICT)
            {
                string errName = ResultWin32.GetErrorName((int)result);
                //throw new StorageConnectExceptin("用户:" + username + " 登录 " + localName + " 失败:" + errName+"(0x"+result.ToString("X")+")");
                return ApiCommon.GetFault("关闭 失败:"+errName, result);
            }
            return ApiCommon.GetSuccess();
        }


    }
}
