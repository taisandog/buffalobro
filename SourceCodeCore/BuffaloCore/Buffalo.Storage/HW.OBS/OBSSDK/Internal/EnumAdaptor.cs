namespace OBS.Internal
{
    using OBS.Model;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal static class EnumAdaptor
    {
        private static volatile IDictionary<string, FilterNameEnum> _FilterNameEnumDict;
        private static volatile IDictionary<string, HttpVerb> _HttpVerbEnumDict;
        private static readonly object _lock = new object();
        private static volatile IDictionary<string, GroupGranteeEnum> _ObsGroupGranteeEnumDict;
        private static volatile IDictionary<string, StorageClassEnum> _ObsStorageClassEnumDict;
        private static volatile IDictionary<string, PermissionEnum> _PermissionEnumDict;
        private static volatile IDictionary<string, ProtocolEnum> _ProtocolEnumDict;
        private static volatile IDictionary<string, RuleStatusEnum> _RuleStatusEnumDict;
        private static volatile IDictionary<string, EventTypeEnum> _V2EventTypeEnumDict;
        private static volatile IDictionary<string, GroupGranteeEnum> _V2GroupGranteeEnumDict;
        private static volatile IDictionary<string, StorageClassEnum> _V2StorageClassEnumDict;
        private static volatile IDictionary<string, VersionStatusEnum> _VersionStatusEnumDict;
        public static IDictionary<Enum, string> EnumValueDict = new Dictionary<Enum, string>();

        public static string GetStringValue(Enum value)
        {
            if (Convert.ToInt32(value) == 0)
            {
                return "";
            }
            if (EnumValueDict.ContainsKey(value))
            {
                return EnumValueDict[value];
            }
            object obj2 = _lock;
            lock (obj2)
            {
                if (EnumValueDict.ContainsKey(value))
                {
                    return EnumValueDict[value];
                }
                object[] customAttributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(false);
                StringValueAttribute attribute = (customAttributes.Length != 0) ? (customAttributes[0] as StringValueAttribute) : null;
                string str = (attribute != null) ? attribute.StringValue : value.ToString();
                EnumValueDict.Add(value, str);
                return str;
            }
        }

        public static IDictionary<string, FilterNameEnum> FilterNameEnumDict
        {
            get
            {
                if (_FilterNameEnumDict == null)
                {
                    object obj2 = _lock;
                    lock (obj2)
                    {
                        if (_FilterNameEnumDict == null)
                        {
                            _FilterNameEnumDict = new Dictionary<string, FilterNameEnum>();
                            _FilterNameEnumDict.Add("prefix", FilterNameEnum.Prefix);
                            _FilterNameEnumDict.Add("suffix", FilterNameEnum.Suffix);
                        }
                    }
                }
                return _FilterNameEnumDict;
            }
        }

        public static IDictionary<string, HttpVerb> HttpVerbEnumDict
        {
            get
            {
                if (_HttpVerbEnumDict == null)
                {
                    object obj2 = _lock;
                    lock (obj2)
                    {
                        if (_HttpVerbEnumDict == null)
                        {
                            _HttpVerbEnumDict = new Dictionary<string, HttpVerb>();
                            _HttpVerbEnumDict.Add("GET", HttpVerb.GET);
                            _HttpVerbEnumDict.Add("POST", HttpVerb.POST);
                            _HttpVerbEnumDict.Add("PUT", HttpVerb.PUT);
                            _HttpVerbEnumDict.Add("DELETE", HttpVerb.DELETE);
                            _HttpVerbEnumDict.Add("HEAD", HttpVerb.HEAD);
                        }
                    }
                }
                return _HttpVerbEnumDict;
            }
        }

        public static IDictionary<string, GroupGranteeEnum> ObsGroupGranteeEnumDict
        {
            get
            {
                if (_ObsGroupGranteeEnumDict == null)
                {
                    object obj2 = _lock;
                    lock (obj2)
                    {
                        if (_ObsGroupGranteeEnumDict == null)
                        {
                            _ObsGroupGranteeEnumDict = new Dictionary<string, GroupGranteeEnum>();
                            _ObsGroupGranteeEnumDict.Add("Everyone", GroupGranteeEnum.AllUsers);
                        }
                    }
                }
                return _ObsGroupGranteeEnumDict;
            }
        }

        public static IDictionary<string, StorageClassEnum> ObsStorageClassEnumDict
        {
            get
            {
                if (_ObsStorageClassEnumDict == null)
                {
                    object obj2 = _lock;
                    lock (obj2)
                    {
                        if (_ObsStorageClassEnumDict == null)
                        {
                            _ObsStorageClassEnumDict = new Dictionary<string, StorageClassEnum>();
                            _ObsStorageClassEnumDict.Add("STANDARD", StorageClassEnum.Standard);
                            _ObsStorageClassEnumDict.Add("WARM", StorageClassEnum.Warm);
                            _ObsStorageClassEnumDict.Add("COLD", StorageClassEnum.Cold);
                        }
                    }
                }
                return _ObsStorageClassEnumDict;
            }
        }

        public static IDictionary<string, PermissionEnum> PermissionEnumDict
        {
            get
            {
                if (_PermissionEnumDict == null)
                {
                    object obj2 = _lock;
                    lock (obj2)
                    {
                        if (_PermissionEnumDict == null)
                        {
                            _PermissionEnumDict = new Dictionary<string, PermissionEnum>();
                            _PermissionEnumDict.Add("READ", PermissionEnum.Read);
                            _PermissionEnumDict.Add("WRITE", PermissionEnum.Write);
                            _PermissionEnumDict.Add("READ_ACP", PermissionEnum.ReadAcp);
                            _PermissionEnumDict.Add("WRITE_ACP", PermissionEnum.WriteAcp);
                            _PermissionEnumDict.Add("FULL_CONTROL", PermissionEnum.FullControl);
                        }
                    }
                }
                return _PermissionEnumDict;
            }
        }

        public static IDictionary<string, ProtocolEnum> ProtocolEnumDict
        {
            get
            {
                if (_ProtocolEnumDict == null)
                {
                    object obj2 = _lock;
                    lock (obj2)
                    {
                        if (_ProtocolEnumDict == null)
                        {
                            _ProtocolEnumDict = new Dictionary<string, ProtocolEnum>();
                            _ProtocolEnumDict.Add("http", ProtocolEnum.Http);
                            _ProtocolEnumDict.Add("https", ProtocolEnum.Https);
                        }
                    }
                }
                return _ProtocolEnumDict;
            }
        }

        public static IDictionary<string, RuleStatusEnum> RuleStatusEnumDict
        {
            get
            {
                if (_RuleStatusEnumDict == null)
                {
                    object obj2 = _lock;
                    lock (obj2)
                    {
                        if (_RuleStatusEnumDict == null)
                        {
                            _RuleStatusEnumDict = new Dictionary<string, RuleStatusEnum>();
                            _RuleStatusEnumDict.Add("Enabled", RuleStatusEnum.Enabled);
                            _RuleStatusEnumDict.Add("Disabled", RuleStatusEnum.Disabled);
                        }
                    }
                }
                return _RuleStatusEnumDict;
            }
        }

        public static IDictionary<string, EventTypeEnum> V2EventTypeEnumDict
        {
            get
            {
                if (_V2EventTypeEnumDict == null)
                {
                    object obj2 = _lock;
                    lock (obj2)
                    {
                        if (_V2EventTypeEnumDict == null)
                        {
                            _V2EventTypeEnumDict = new Dictionary<string, EventTypeEnum>();
                            _V2EventTypeEnumDict.Add("s3:ObjectCreated:*", EventTypeEnum.ObjectCreatedAll);
                            _V2EventTypeEnumDict.Add("s3:ObjectCreated:Put", EventTypeEnum.ObjectCreatedPut);
                            _V2EventTypeEnumDict.Add("s3:ObjectCreated:Post", EventTypeEnum.ObjectCreatedPost);
                            _V2EventTypeEnumDict.Add("s3:ObjectCreated:Copy", EventTypeEnum.ObjectCreatedCopy);
                            _V2EventTypeEnumDict.Add("s3:ObjectCreated:CompleteMultipartUpload", EventTypeEnum.ObjectCreatedCompleteMultipartUpload);
                            _V2EventTypeEnumDict.Add("s3:ObjectRemoved:*", EventTypeEnum.ObjectRemovedAll);
                            _V2EventTypeEnumDict.Add("s3:ObjectRemoved:Delete", EventTypeEnum.ObjectRemovedDelete);
                            _V2EventTypeEnumDict.Add("s3:ObjectRemoved:DeleteMarkerCreated", EventTypeEnum.ObjectRemovedDeleteMarkerCreated);
                        }
                    }
                }
                return _V2EventTypeEnumDict;
            }
        }

        public static IDictionary<string, GroupGranteeEnum> V2GroupGranteeEnumDict
        {
            get
            {
                if (_V2GroupGranteeEnumDict == null)
                {
                    object obj2 = _lock;
                    lock (obj2)
                    {
                        if (_V2GroupGranteeEnumDict == null)
                        {
                            _V2GroupGranteeEnumDict = new Dictionary<string, GroupGranteeEnum>();
                            _V2GroupGranteeEnumDict.Add("http://acs.amazonaws.com/groups/global/AllUsers", GroupGranteeEnum.AllUsers);
                            _V2GroupGranteeEnumDict.Add("http://acs.amazonaws.com/groups/global/AuthenticatedUsers", GroupGranteeEnum.AuthenticatedUsers);
                            _V2GroupGranteeEnumDict.Add("http://acs.amazonaws.com/groups/s3/LogDelivery", GroupGranteeEnum.LogDelivery);
                        }
                    }
                }
                return _V2GroupGranteeEnumDict;
            }
        }

        public static IDictionary<string, StorageClassEnum> V2StorageClassEnumDict
        {
            get
            {
                if (_V2StorageClassEnumDict == null)
                {
                    object obj2 = _lock;
                    lock (obj2)
                    {
                        if (_V2StorageClassEnumDict == null)
                        {
                            _V2StorageClassEnumDict = new Dictionary<string, StorageClassEnum>();
                            _V2StorageClassEnumDict.Add("STANDARD", StorageClassEnum.Standard);
                            _V2StorageClassEnumDict.Add("STANDARD_IA", StorageClassEnum.Warm);
                            _V2StorageClassEnumDict.Add("GLACIER", StorageClassEnum.Cold);
                        }
                    }
                }
                return _V2StorageClassEnumDict;
            }
        }

        public static IDictionary<string, VersionStatusEnum> VersionStatusEnumDict
        {
            get
            {
                if (_VersionStatusEnumDict == null)
                {
                    object obj2 = _lock;
                    lock (obj2)
                    {
                        if (_VersionStatusEnumDict == null)
                        {
                            _VersionStatusEnumDict = new Dictionary<string, VersionStatusEnum>();
                            _VersionStatusEnumDict.Add("Enabled", VersionStatusEnum.Enabled);
                            _VersionStatusEnumDict.Add("Suspended", VersionStatusEnum.Suspended);
                        }
                    }
                }
                return _VersionStatusEnumDict;
            }
        }
    }
}

