using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace Buffalo.Kernel.HttpServerModel
{
    /// <summary>
    /// ������ί��
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    public delegate void RequestProcessingHandle(RequestInfo request,ResponseInfo response);

    /// <summary>
    /// ������ί��
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    public delegate void RequestSendingBodyHandle(RequestInfo request, ResponseInfo response,Socket requestSocket);
    /// <summary>
    /// ģ��Web������ģ��
    /// </summary>
    public class ServerModel
    {
        private TcpListener _listener;
        /// <summary>
        /// �������¼�
        /// </summary>
        public event RequestProcessingHandle OnRequestProcessing;
        /// <summary>
        /// �������ڷ������ݵ��¼�
        /// </summary>
        public event RequestSendingBodyHandle OnRequestSendingBody;
        Thread _lisThread = null;
        bool isrunning=false;
        int _port = 0;

        

        /// <summary>
        /// �˿�
        /// </summary>
        public int Port
        {
            get { return _port; }
        }
        IPAddress _ip;

        /// <summary>
        /// IP
        /// </summary>
        public IPAddress IP
        {
            get { return _ip; }
        }
        static readonly Encoding DefaultEncoding = System.Text.Encoding.UTF8;

        /// <summary>
        /// ��ʼ������ģ��
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="lisPort">�˿�</param>
        /// <param name="encoding">����</param>
        public ServerModel(string ip,int lisPort) 
        {
            if (string.IsNullOrEmpty(ip))
            {
                _ip = Dns.GetHostByName(Dns.GetHostName()).AddressList[0];
            }
            else 
            {
                _ip = IPAddress.Parse(ip);
            }
            _port = lisPort;
            
        }


        /// <summary>
        /// ��ʼ����
        /// </summary>
        public void StarServer() 
        {
            _listener = new TcpListener(_ip, _port);
            isrunning = true;
            _lisThread = new Thread(new ThreadStart(ModelListen));
            _listener.Start();
            _lisThread.Start();
        }

        /// <summary>
        /// ����
        /// </summary>
        private void ModelListen() 
        {
            while (isrunning)
            {
                //����������
                Socket requestSocket = _listener.AcceptSocket();
                
                    ParameterizedThreadStart prmThd = new ParameterizedThreadStart(DelSocket);
                    Thread thd = new Thread(new ParameterizedThreadStart(DelSocket));

                    thd.Start(requestSocket);
                
            }
        }

        private void DelSocket(object prm)
        {
            Socket requestSocket = prm as Socket;
            if (requestSocket == null)
            {
                return;
            }
            try
            {
                byte[] receiveContent = null;//���յ�����
                if (requestSocket.Connected)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Byte[] bufferContent = new Byte[1024];
                        int receiveBytes = 0;
                        do
                        {
                            receiveBytes = requestSocket.Receive(bufferContent, bufferContent.Length, 0);
                            ms.Write(bufferContent, 0, receiveBytes);
                        } while (receiveBytes >= bufferContent.Length);
                        if (ms.Length <= 0)
                        {
                            return;
                        }
                        receiveContent = ms.ToArray();


                    }
                }
                else
                {
                    return;
                }
                requestSocket.SendTimeout = 5000;
                string content = DefaultEncoding.GetString(receiveContent);
                RequestInfo info = new RequestInfo(content);
                if (OnRequestProcessing != null)
                {
                    Encoding encoding = GetEncoding(info);
                    using (ResponseInfo resInfo = new ResponseInfo(encoding))
                    {
                        OnRequestProcessing(info, resInfo);
                        byte[] rcontent = resInfo.ResponseContent;
                        long len = resInfo.Length;
                        if (len == 0)
                        {
                            len = rcontent.Length;
                        }

                        
                        string header = CreateHeader(info.HttpVersion,info, resInfo, len, " 200 OK", encoding);
                        byte[] headContent = encoding.GetBytes(header);

                        requestSocket.Send(headContent);

                        if (OnRequestSendingBody != null)
                        {
                            OnRequestSendingBody(info, resInfo, requestSocket);
                        }
                        if (rcontent.Length > 0)
                        {
                            requestSocket.Send(rcontent);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine(ex.Message);
#endif
            }
            finally 
            {
                try
                {
                    
                    requestSocket.Close();

                }
                catch { }
            }
        }
        

        /// <summary>
        /// ��ȡ����ı���
        /// </summary>
        /// <param name="resInfo"></param>
        /// <returns></returns>
        private Encoding GetEncoding(RequestInfo resInfo) 
        {
            if (string.IsNullOrEmpty(resInfo.AcceptEncoding)) 
            {
                return DefaultEncoding;
            }
            try
            {
                Encoding encoding = Encoding.GetEncoding(resInfo.AcceptEncoding);
                if (encoding != null)
                {
                    return encoding;
                }
            }
            catch { }
            return DefaultEncoding;
        }

       

        /// <summary>
        /// ����ͷ
        /// </summary>
        /// <param name="sHttpVersion"></param>
        /// <param name="sMIMEHeader"></param>
        /// <param name="iTotBytes"></param>
        /// <param name="sStatusCode"></param>
        private string CreateHeader(string sHttpVersion, RequestInfo request, ResponseInfo resInfo, long iTotBytes, string sStatusCode, Encoding encoding)
        {

            StringBuilder sBuffer = new StringBuilder();


            sBuffer.AppendLine("HTTP/1.1 "+resInfo.StatusCode+" OK");
            if (string.IsNullOrEmpty(resInfo.MimeType))
            {
                resInfo.MimeType = "text/html"; // Ĭ�� text/html
            }

            sBuffer.AppendLine(sHttpVersion + sStatusCode);
            if (resInfo.MimeType.Equals("text/html", StringComparison.CurrentCultureIgnoreCase))
            {
                sBuffer.AppendLine("Content-Type: " + resInfo.MimeType + ";charset=" + encoding.WebName);
                sBuffer.AppendLine("Content-Encoding: " + encoding.WebName);
            }
            if (!resInfo.Header.ContainsKey("Accept-Ranges"))
            {
                sBuffer.AppendLine("Accept-Ranges: none");
            }

            //if (request.Header.ContainsKey("range"))
            //{
            //    resInfo.Header["Content-Range"] = request.Header["range"].Replace('=', ' ') + "/" + iTotBytes;
            //}

            if (!resInfo.Header.ContainsKey("Server"))
            {
                resInfo.Header["Server"] = "Buffalo Mini Server";
            }
            if (!resInfo.Header.ContainsKey("Content-Length"))
            {
                resInfo.Header["Content-Length"] = iTotBytes.ToString();
            }

            foreach (KeyValuePair<string, string> kvp in resInfo.Header) 
            {
                sBuffer.AppendLine(kvp.Key + ": " + kvp.Value);
            }
            sBuffer.AppendLine("");
            return sBuffer.ToString();
        }

        /// <summary>
        /// �رշ���
        /// </summary>
        public void StopServer() 
        {
            if (_listener != null) 
            {
                _listener.Stop();
            }
           
            isrunning = false;
            if (_lisThread != null)
            {
                _lisThread.Abort();

            }
        }
    }
}
