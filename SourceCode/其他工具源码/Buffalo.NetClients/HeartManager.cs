using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Buffalo.NetClients
{

    /// <summary>
    /// 心跳,超时,数据重发管理
    /// </summary>
    public class HeartManager
    {
        private int _hreatTimeOut;
        /// <summary>
        /// 超时时间
        /// </summary>
        public int HreatTimeOut
        {
            get 
            {
                return _hreatTimeOut;
            }
        }

        private int _hreatSendTime;

        /// <summary>
        /// 心跳发送时间
        /// </summary>
        public int HreatSendTime
        {
            get { return _hreatSendTime; }
        }

        
        /// <summary>
        /// 客户端数
        /// </summary>
        private LinkedList<NetConnection> _clients;
       

        /// <summary>
        /// 检查线程
        /// </summary>
        private Thread _checkThread;

        /// <summary>
        /// 心跳管理
        /// </summary>
        /// <param name="hreatTimeOut">心跳超时(毫秒)</param>
        /// <param name="hreatSendTime">心跳发送时间(毫秒，0则不发送)</param>
        public HeartManager(int hreatTimeOut, int hreatSendTime)
        {
            _clients = new LinkedList<NetConnection>();
            _hreatTimeOut = hreatTimeOut;
            _hreatSendTime = hreatSendTime;
        }

        /// <summary>
        /// 开启检查
        /// </summary>
        public void Start()
        {
            if (_checkThread == null)
            {
                _running = true;
                _checkThread = new Thread(new ThreadStart(CheckRun));
                _checkThread.Start();
            }
        }
        /// <summary>
        /// 开启检查
        /// </summary>
        public void Stop()
        {
            _running = false;
            if (_checkThread != null)
            {
                _checkThread.Abort();
                Thread.Sleep(100);
            }
        }
        /// <summary>
        /// 添加到心跳
        /// </summary>
        /// <param name="client"></param>
        public void AddClient(NetConnection client)
        {
            lock (_clients)
            {
                _clients.AddLast(client);
            }
        }


        private bool _running = false;

        private void CheckRun()
        {
            LinkedListNode<NetConnection> curNode = null;
            LinkedListNode<NetConnection> node = null;
            while (_running)
            {
                lock (_clients)
                {
                    node = _clients.First;
                    if (node==null)
                    {
                        while (node != null)
                        {
                            curNode = node;
                            node = node.Next;
                            NetConnection conn = curNode.Value;
                            if (conn == null)
                            {
                                _clients.Remove(curNode);
                            }
                            else if (_hreatSendTime > 0 && ((DateTime.Now - conn.LastSendTime).TotalMilliseconds > _hreatSendTime))
                            {
                                conn.SendHeard();
                            }
                            else if (_hreatTimeOut>0 &&((DateTime.Now - conn.LastReceiveTime).TotalMilliseconds > _hreatTimeOut))
                            {
                                conn.CloseConnection();
                                _clients.Remove(curNode);
                            }
                        }
                    }
                    else
                    {

                        _checkThread = null;
                        break;
                    }
                }
                Thread.Sleep(500);
            }
        }
    }

}
