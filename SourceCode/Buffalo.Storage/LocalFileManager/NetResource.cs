using System;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using System.Text;

namespace Buffalo.Storage.LocalFileManager
{
    [StructLayout(LayoutKind.Sequential)]

    public class NetResource
    {

        public ResourceScope Scope;
        public ResourceType ResourceType;
        public ResourceDisplaytype DisplayType;
        public ResourceUsage Usage;
        public string LocalName;
        public string RemoteName;
        public string Comment;
        public string Provider;
    }

    public enum ResourceScope : int
    {

        Connected = 1,
        GlobalNetwork,
        Remembered,
        Recent,
        Context
    };

    public enum ResourceType : int
    {

        Any = 0,
        Disk = 1,
        Print = 2,
        Reserved = 8,
    }

    public enum ResourceDisplaytype : int
    {

        Generic = 0x0,
        Domain = 0x01,
        Server = 0x02,
        Share = 0x03,
        File = 0x04,
        Group = 0x05,
        Network = 0x06,
        Root = 0x07,
        Shareadmin = 0x08,
        Directory = 0x09,
        Tree = 0x0a,
        Ndscontainer = 0x0b
    }
    public enum ResourceUsage:int
    {
        CONNECTABLE = 0x00000001,
        CONTAINER = 0x00000002,
        OLOCALDEVICE = 0x00000004,
        SIBLING = 0x00000008,
        ATTACHED = 0x00000010,
        ALL = (CONNECTABLE | CONTAINER | ATTACHED),
    }

}
