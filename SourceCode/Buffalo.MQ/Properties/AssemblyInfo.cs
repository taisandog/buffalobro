using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 有关程序集的常规信息通过下列属性集
// 控制。更改这些属性值可修改
// 与程序集关联的信息。
#if (NET_1_0)
[assembly: AssemblyTitle("Buffalo.MQ for .NET Framework 1.0")]
#elif (NET_1_1)
[assembly: AssemblyTitle("Buffalo.MQ for .NET Framework 1.1")]
#elif (NET_2_0)
[assembly: AssemblyTitle("Buffalo.MQ for .NET Framework 2.0")]
#elif (NET_3_0)
[assembly: AssemblyTitle("Buffalo.MQ for .NET Framework 3.0")]
#elif (NET_3_5)
[assembly: AssemblyTitle("Buffalo.MQ for .NET Framework 3.5")]
#elif (NET_4_0)
[assembly: AssemblyTitle("Buffalo.MQ for .NET Framework 4.0")]
#elif (NET_4_5)
[assembly: AssemblyTitle("Buffalo.MQ for .NET Framework 4.5")]
#elif (NET_4_5_1)
[assembly: AssemblyTitle("Buffalo.MQ for .NET Framework 4.5.1")]
#elif (NET_4_6)
[assembly: AssemblyTitle("Buffalo.MQ for .NET Framework 4.6")]
#elif (NET_4_6_2)
[assembly: AssemblyTitle("Buffalo.MQ for .NET Framework 4.6.2")]
#elif (NET_4_7_2)
[assembly: AssemblyTitle("Buffalo.MQ for .NET Framework 4.7.2")]
#elif (NET_4_8)
[assembly: AssemblyTitle("Buffalo.MQ for .NET Framework 4.8")]
#elif (NETCF_1_0)
[assembly: AssemblyTitle("Buffalo.MQ for .NET Compact Framework 1.0")]
#elif (NETCF_2_0)
[assembly: AssemblyTitle("Buffalo.MQ for .NET Compact Framework 2.0")]
#elif (MONO_1_0)
[assembly: AssemblyTitle("Buffalo.MQ for Mono 1.0")]
#elif (MONO_2_0)
[assembly: AssemblyTitle("Buffalo.MQ for Mono 2.0")]
#else
[assembly: AssemblyTitle("Buffalo.MQ")]
#endif

[assembly: AssemblyDescription("Buffalo.MQLibrary")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
[assembly: AssemblyProduct("Buffalo.DB(Debug)")]
#else
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyProduct("Buffalo.MQ")]
#endif
[assembly: AssemblyCompany("Buffalo")]

[assembly: AssemblyCopyright("版权所有 (C) Buffalo 2012")]
[assembly: AssemblyTrademark("BuffaloLibrary")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 会使此程序集中的类型
//对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型
//请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("6cedc1e6-bd83-4d69-8b52-f0534e16f688")]

// 程序集的版本信息由下列四个值组成: 
//
//      主版本
//      次版本
//      生成号
//      修订号
//
// 可以指定所有值，也可以使用以下所示的 "*" 预置版本号和修订号
//通过使用 "*"，如下所示:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.7.2")]
[assembly: AssemblyFileVersion("1.7.2")]
