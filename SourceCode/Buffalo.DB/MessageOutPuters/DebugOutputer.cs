using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Buffalo.Kernel;
using System.Collections;

namespace Buffalo.DB.MessageOutPuters
{
    public class DebugOutputer : MessageOutputBase
    {
        /// <summary>
        ///  ‰≥ˆ–≈œ¢
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
#if DEBUG
            Debug.WriteLine(smsg.ToString());
#else
            Trace.WriteLine(smsg.ToString());
#endif
        }

    }
}
