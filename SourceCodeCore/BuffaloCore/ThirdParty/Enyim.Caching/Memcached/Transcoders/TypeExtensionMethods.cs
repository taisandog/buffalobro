﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    internal static class TypeExtensionMethods
    {
        public static TypeCode GetTypeCode(this Type type)
        {
            if (type == null)
            {
                return TypeCode.Empty;
            }
            else if (type == typeof(Boolean))
            {
                return TypeCode.Boolean;
            }
            else if (type == typeof(Char))
            {
                return TypeCode.Char;
            }
            else if (type == typeof(SByte))
            {
                return TypeCode.SByte;
            }
            else if (type == typeof(Byte))
            {
                return TypeCode.Byte;
            }
            else if (type == typeof(Int16))
            {
                return TypeCode.Int16;
            }
            else if (type == typeof(UInt16))
            {
                return TypeCode.UInt16;
            }
            else if (type == typeof(Int32))
            {
                return TypeCode.Int32;
            }
            else if (type == typeof(UInt32))
            {
                return TypeCode.UInt32;
            }
            else if (type == typeof(Int64))
            {
                return TypeCode.Int64;
            }
            else if (type == typeof(UInt64))
            {
                return TypeCode.UInt64;
            }
            else if (type == typeof(Single))
            {
                return TypeCode.Single;
            }
            else if (type == typeof(Double))
            {
                return TypeCode.Double;
            }
            else if (type == typeof(Decimal))
            {
                return TypeCode.Decimal;
            }
            else if (type == typeof(DateTime))
            {
                return TypeCode.DateTime;
            }
            else if (type == typeof(String))
            {
                return TypeCode.String;
            }
            else if (type == typeof(Guid))
            {
                return TypeCode.String;
            }
            else
            {
                return TypeCode.Object;
            }
        }
    }
}
