using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
namespace OBS.Internal.Log
{
    
    /// <summary>
    /// 日志系统，需修改
    /// </summary>
    internal static class LoggerMgr
    {
        private static readonly object _lock = new object();
        private static MethodInfo debug;
        private static MethodInfo error;
        private static MethodInfo info;
        private static volatile bool inited = false;
        private static PropertyInfo isDebugEnabled;
        private static PropertyInfo isErrorEnabled;
        private static PropertyInfo isInfoEnabled;
        private static PropertyInfo isWarnEnabled;
        private static volatile object logger;
        private static MethodInfo warn;

        internal static void Debug(string param)
        {
            Debug(param, null);
        }

        internal static void Debug(string param, Exception exception)
        {
            if (logger != null)
            {
                object[] parameters = new object[] { param, exception };
                debug.Invoke(logger, parameters);
            }
        }

        internal static void Error(string param)
        {
            Error(param, null);
        }

        internal static void Error(string param, Exception exception)
        {
            if (logger != null)
            {
                object[] parameters = new object[] { param, exception };
                error.Invoke(logger, parameters);
            }
        }

        internal static void Info(string param)
        {
            Info(param, null);
        }

        internal static void Info(string param, Exception exception)
        {
            if (logger != null)
            {
                object[] parameters = new object[] { param, exception };
                info.Invoke(logger, parameters);
            }
        }

        internal static void Initialize()
        {
            if (!inited && (logger == null))
            {
                object obj2 = _lock;
                lock (obj2)
                {
                    if (!inited && (logger == null))
                    {
                        inited = true;
                        try
                        {
                            FileInfo info = null;
                            if (File.Exists("Log4Net.config"))
                            {
                                info = new FileInfo("Log4Net.config");
                            }
                            else if (File.Exists("Log4Net.xml"))
                            {
                                info = new FileInfo("Log4Net.xml");
                            }
                            else if (File.Exists("log4net.config"))
                            {
                                info = new FileInfo("log4net.config");
                            }
                            else if (File.Exists("log4net.xml"))
                            {
                                info = new FileInfo("log4net.xml");
                            }
                            if (info != null)
                            {
                                Assembly assembly = Assembly.LoadFile(Environment.CurrentDirectory + "/log4net.dll");
                                Type type = assembly.GetType("log4net.LogManager", true, true);
                                Type type2 = assembly.GetType("log4net.Repository.ILoggerRepository", true, true);
                                Type[] types = new Type[] { typeof(string) };
                                object[] parameters = new object[] { "LoggerMgrRepository" };
                                object obj3 = type.GetMethod("CreateRepository", types).Invoke(null, parameters);
                                Type[] typeArray2 = new Type[] { type2, typeof(FileInfo) };
                                object[] objArray2 = new object[] { obj3, info };
                                assembly.GetType("log4net.Config.XmlConfigurator", true, true).GetMethod("ConfigureAndWatch", typeArray2).Invoke(null, objArray2);
                                Type[] typeArray3 = new Type[] { typeof(string), typeof(string) };
                                object[] objArray3 = new object[] { "LoggerMgrRepository", "LoggerMgr" };
                                logger = type.GetMethod("GetLogger", typeArray3).Invoke(null, objArray3);
                                Type type3 = logger.GetType();
                                isDebugEnabled = type3.GetProperty("IsDebugEnabled");
                                isInfoEnabled = type3.GetProperty("IsInfoEnabled");
                                isWarnEnabled = type3.GetProperty("IsWarnEnabled");
                                isErrorEnabled = type3.GetProperty("IsErrorEnabled");
                                Type[] typeArray4 = new Type[] { typeof(object), typeof(Exception) };
                                debug = type3.GetMethod("Debug", typeArray4);
                                Type[] typeArray5 = new Type[] { typeof(object), typeof(Exception) };
                                LoggerMgr.info = type3.GetMethod("Info", typeArray5);
                                Type[] typeArray6 = new Type[] { typeof(object), typeof(Exception) };
                                warn = type3.GetMethod("Warn", typeArray6);
                                Type[] typeArray7 = new Type[] { typeof(object), typeof(Exception) };
                                error = type3.GetMethod("Error", typeArray7);
                            }
                        }
                        catch (Exception)
                        {
                            logger = null;
                        }
                    }
                }
            }
        }

        internal static void Warn(string param)
        {
            Warn(param, null);
        }

        internal static void Warn(string param, Exception exception)
        {
            if (logger != null)
            {
                object[] parameters = new object[] { param, exception };
                warn.Invoke(logger, parameters);
            }
        }

        internal static bool IsDebugEnabled
        {
            get
            {
                if (logger == null)
                {
                    return false;
                }
                return (bool) isDebugEnabled.GetValue(logger, null);
            }
        }

        internal static bool IsErrorEnabled
        {
            get
            {
                if (logger == null)
                {
                    return false;
                }
                return (bool) isErrorEnabled.GetValue(logger, null);
            }
        }

        internal static bool IsInfoEnabled
        {
            get
            {
                if (logger == null)
                {
                    return false;
                }
                return (bool) isInfoEnabled.GetValue(logger, null);
            }
        }

        internal static bool IsWarnEnabled
        {
            get
            {
                if (logger == null)
                {
                    return false;
                }
                return Convert.ToBoolean(isWarnEnabled.GetValue(logger, null));
            }
        }
    }
}

