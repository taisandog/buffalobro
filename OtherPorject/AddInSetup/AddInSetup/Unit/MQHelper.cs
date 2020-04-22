using Buffalo.ArgCommon;
using Buffalo.MQ;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static APIResault TestMQ(string name,string liskey,string type,bool getlastvalue,string connstring)
        {
            MQUnit.SetMQInfo(name, type, connstring);
            _lis = MQUnit.GetMQListener(name);
            _lis.OnMQReceived += _lis_OnMQReceived;
            _lis.OnMQException += _lis_OnMQException;
            try
            {
                _lis.StartListend(new string[] { liskey });
                
                if (!_lis.WaitStart(10000))
                {
                    return ApiCommon.GetFault("等待队列初始化超时");
                }
                _handle.Reset();

                MQConnection conn = MQUnit.GetMQConnection(name);
                int num = Guid.NewGuid().GetHashCode();
                byte[] sendByte = BitConverter.GetBytes(num);
                //byte[] sendByte = Encoding.UTF8.GetBytes(num.ToString());
                using (MQBatchAction ba = conn.StartBatchAction())
                {
                    conn.Send(liskey, sendByte);
                }
                if (getlastvalue)
                {
                    Thread.Sleep(5000);
                }
                else if (!_handle.WaitOne(10000))
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

        private static void _lis_OnMQException(MQListener sender, Exception ex)
        {
            
        }

        private static void _lis_OnMQReceived(MQListener sender, string exchange, string routingKey, byte[] body, int partition, long offset)
        {
            _retNum = BitConverter.ToInt32(body,0);
            Debug.WriteLine(_retNum);
            _handle.Set();
        }
    }
}
