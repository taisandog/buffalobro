using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if (NET_1_0)
[assembly: AssemblyTitle("Buffalo.IOCP for .NET Framework 1.0")]
#elif (NET_1_1)
[assembly: AssemblyTitle("Buffalo.IOCP for .NET Framework 1.1")]
#elif (NET_2_0)
[assembly: AssemblyTitle("Buffalo.IOCP for .NET Framework 2.0")]
#elif (NET_3_0)
[assembly: AssemblyTitle("Buffalo.IOCP for .NET Framework 3.0")]
#elif (NET_3_5)
[assembly: AssemblyTitle("Buffalo.IOCP for .NET Framework 3.5")]
#elif (NET_4_0)
[assembly: AssemblyTitle("Buffalo.IOCP for .NET Framework 4.0")]
#elif (NET_4_5)
[assembly: AssemblyTitle("Buffalo.IOCP for .NET Framework 4.5")]
#elif (NET_4_5_1)
[assembly: AssemblyTitle("Buffalo.IOCP for .NET Framework 4.5.1")]
#elif (NET_4_6)
[assembly: AssemblyTitle("Buffalo.IOCP for .NET Framework 4.6")]
#elif (NET_4_6_2)
[assembly: AssemblyTitle("Buffalo.IOCP for .NET Framework 4.6.2")]
#elif (NET_4_7_2)
[assembly: AssemblyTitle("Buffalo.IOCP for .NET Framework 4.7.2")]
#elif (NET_4_8)
[assembly: AssemblyTitle("Buffalo.IOCP for .NET Framework 4.8")]
#elif (NETCF_1_0)
[assembly: AssemblyTitle("Buffalo.IOCP for .NET Compact Framework 1.0")]
#elif (NETCF_2_0)
[assembly: AssemblyTitle("Buffalo.IOCP for .NET Compact Framework 2.0")]
#elif (MONO_1_0)
[assembly: AssemblyTitle("Buffalo.IOCP for Mono 1.0")]
#elif (MONO_2_0)
[assembly: AssemblyTitle("Buffalo.IOCP for Mono 2.0")]
#else
[assembly: AssemblyTitle("Buffalo.IOCP")]
#endif

[assembly: AssemblyDescription("Buffalo Socket Unit Info Library")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
[assembly: AssemblyProduct("Buffalo.IOCP(Debug)")]
#else
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyProduct("Buffalo.IOCP")]
#endif
[assembly: AssemblyCompany("Buffalo")]

[assembly: AssemblyCopyright("版权所有 (C) Microsoft 2013")]
[assembly: AssemblyTrademark("BuffaloLibrary")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 会使此程序集中的类型
//对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型
//请将此类型的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("66e465ca-ab11-4c1c-b8c1-26885d70ba6b")]

// 程序集的版本信息由下列四个值组成: 
//
//      主版本
//      次版本
//      生成号
//      修订号
//
//可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值
//通过使用 "*"，如下所示:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
