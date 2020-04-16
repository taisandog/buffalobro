using Buffalo.ArgCommon;
using Buffalo.MQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AddInSetup.Unit
{
    /// <summary>
    /// 队列工具
    /// </summary>
    public class MQHelper
    {
        static MQListener _lis = null;
        static AutoResetEvent _handle=new AutoResetEvent(false);

        static int _retNum = 0;
        public static APIResault TestMQ(string name,string liskey,string type,string connstring)
        {
            MQUnit.SetMQInfo(name, type, connstring);
            _lis = MQUnit.GetMQListener(name);
            _lis.OnMQReceived += _lis_OnMQReceived;
           
            try
            {
                _lis.StartListend(new string[] { liskey });
                _handle.Reset();

                MQConnection conn = MQUnit.GetMQConnection(name);
                int num = Guid.NewGuid().GetHashCode();
                conn.Send(liskey, BitConverter.GetBytes(num));

                if (!_handle.WaitOne(5000))
                {
                    return ApiCommon.GetFault("等待队列推送超时");
                }
                if (_retNum != num)
                {
                    return ApiCommon.GetFault("队列接收错误，发送:" + num + "，接收:" + _retNum);
                }
                return ApiCommon.GetSuccess();
            }
            catch (Exception ex)
            {
                return ApiCommon.GetException(ex);
            }
            finally
            {
                _lis.Close();
            }
        }

        private static void _lis_OnMQReceived(MQListener sender, string exchange, string routingKey, byte[] body)
        {
            _retNum = BitConverter.ToInt32(body,0);
            _handle.Set();
        }
    }
}
