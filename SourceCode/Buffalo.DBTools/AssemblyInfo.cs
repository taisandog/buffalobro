using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


#if (NET_1_0)
[assembly: AssemblyTitle("BuffaloDBTools for .NET Framework 1.0")]
#elif (NET_1_1)
[assembly: AssemblyTitle("BuffaloDBTools for .NET Framework 1.1")]
#elif (NET_2_0)
[assembly: AssemblyTitle("BuffaloDBTools for .NET Framework 2.0")]
#elif (NET_3_0)
[assembly: AssemblyTitle("BuffaloDBTools for .NET Framework 3.0")]
#elif (NET_3_5)
[assembly: AssemblyTitle("BuffaloDBTools for .NET Framework 3.5")]
#elif (NET_4_0)
[assembly: AssemblyTitle("BuffaloDBTools for .NET Framework 4.0")]
#elif (NET_4_5)
[assembly: AssemblyTitle("BuffaloDBTools for .NET Framework 4.5")]
#elif (NET_4_5_1)
[assembly: AssemblyTitle("BuffaloDBTools for .NET Framework 4.5.1")]
#elif (NETCF_1_0)
[assembly: AssemblyTitle("BuffaloDBTools for .NET Compact Framework 1.0")]
#elif (NETCF_2_0)
[assembly: AssemblyTitle("BuffaloDBTools for .NET Compact Framework 2.0")]
#elif (MONO_1_0)
[assembly: AssemblyTitle("BuffaloDBTools for Mono 1.0")]
#elif (MONO_2_0)
[assembly: AssemblyTitle("BuffaloDBTools for Mono 2.0")]
#else
[assembly: AssemblyTitle("BuffaloDBTolls")]
#endif


//

[assembly: AssemblyDescription("BuffaloCodeToolsPackage")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
[assembly: AssemblyProduct("Buffalo.DBTools(Debug)")]
#else
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyProduct("Buffalo.DBToolsPackage")]
#endif

[assembly: AssemblyCompany("Buffalo")]

[assembly: AssemblyCopyright("版权所有 (C) Buffalo 2012")]
[assembly: AssemblyTrademark("BuffaloLibrary")]
[assembly: AssemblyCulture("")]
//
// 程序集的版本信息由下面四个值组成:
//
//      主版本
//      次版本
//      修订号
//      内部版本号
//
// 您可以指定所有这些值，也可以按照如下所示通过使用“*”来使用
// “修订号”和“内部版本号”的默认值:

[assembly: AssemblyVersion("1.1.0.0")]
[assembly: AssemblyFileVersion("1.1.0.0")]
//
// 要对程序集进行签名，必须指定要使用的密钥。有关程序集签名的更多信息，请参考
// Microsoft .NET Framework 文档。
//
// 使用下面的属性控制用于签名的密钥。
//
// 注意:
//   (*) 如果未指定任何密钥，则无法对程序集签名。
//   (*) KeyName 是指计算机上的加密服务
//       提供程序(CSP)中已经安装的密钥。
//   (*) 如果密钥文件和密钥名称属性都已指定，则
//       按如下方式进行处理:
//       (1) 如果可在 CSP 中找到 KeyName，则使用该密钥。
//       (2) 如果 KeyName 不存在而 KeyFile 存在，
//           则在 CSP 中安装并使用该文件中的密钥。
//   (*) “延迟签名”是一个高级选项 - 有关它的更多信息，
//       请参阅 Microsoft .NET Framework 文档。
//
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]