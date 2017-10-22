using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NullGuard;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Akka TestKit Dependancy Injection Helpers")]
[assembly: AssemblyDescription("Akka DependencyResolver that mocks child Actors into Stubs or TestProbes.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Connel Hooley")]
[assembly: AssemblyProduct("ConnelHooley.AkkaTestingHelpers.DI")]
[assembly: AssemblyCopyright("Copyright Connel Hooley 2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("9d580f8e-32c5-48f7-b3a9-4bfec3f5ad79")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.*")]

[assembly: NullGuard(ValidationFlags.All)]
[assembly: InternalsVisibleTo("FileSystem.Fakes")]
[assembly: InternalsVisibleTo("FileSystem.Tests")]
[assembly: InternalsVisibleTo("TargetAssembly.Fakes")]
[assembly: InternalsVisibleTo("TestAssembly")]
[assembly: InternalsVisibleTo("ConnelHooley.AkkaTestingHelpers.DI.SmallTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

