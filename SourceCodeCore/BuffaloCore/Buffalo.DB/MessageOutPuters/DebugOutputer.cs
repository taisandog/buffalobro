using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Buffalo.Kernel;
using System.Collections;
using System.Threading.Tasks;

namespace Buffalo.DB.MessageOutPuters
{
    /// <summary>
    /// Debug�����
    /// </summary>
    public class DebugOutputer : MessageOutputBase
    {
        /// <summary>
        /// Trace.WriteLineģʽ
        /// </summary>
        /// <param name="str"></param>
        private void TraceWriteLine(string str) 
        {
            Trace.WriteLine(str);
        }
        /// <summary>
        /// Debug.WriteLineģʽ
        /// </summary>
        /// <param name="str"></param>
        private void DebugWriteLine(string str)
        {
            Debug.WriteLine(str);
        }
        /// <summary>
        /// Console.WriteLineģʽ
        /// </summary>
        /// <param name="str"></param>
        private void ConsoleWriteLine(string str)
        {
            Console.WriteLine(str);
        }
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="messType"></param>
        /// <param name="mess"></param>
        public override void OutPut(MessageType messType, MessageInfo mess) 
        {
            string messName = messType.ToString();
            StringBuilder smsg = new StringBuilder();
            smsg.Append(messName);

            object val=mess.Type;
            smsg.Append("[");
            if (val!=null) 
            {
                smsg.Append(val);
            }
            val = mess.ExtendType;
            if (val != null)
            {
                smsg.Append(","+val);
            }
            smsg.Append("]");

            val = mess.Value;
            if (val != null)
            {
                smsg.Append(":"+val);
            }

            Debug.WriteLine(smsg.ToString());

        }

        public async override Task OutPutAsync(MessageType messType, MessageInfo mess)
        {
            string messName = messType.ToString();
            StringBuilder smsg = new StringBuilder();
            smsg.Append(messName);

            object val = mess.Type;
            smsg.Append("[");
            if (val != null)
            {
                smsg.Append(val);
            }
            val = mess.ExtendType;
            if (val != null)
            {
                smsg.Append("," + val);
            }
            smsg.Append("]");

            val = mess.Value;
            if (val != null)
            {
                smsg.Append(":" + val);
            }

            Debug.WriteLine(smsg.ToString());

        }
    }
}
