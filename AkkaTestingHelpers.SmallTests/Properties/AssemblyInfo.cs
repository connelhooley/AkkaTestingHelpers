using System.Reflection;
using System.Runtime.InteropServices;
using Xunit;

[assembly: AssemblyTitle("AkkaTestingHelpers.DI.SmallTests")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("AkkaTestingHelpers.DI.SmallTests")]
[assembly: AssemblyCopyright("Copyright ©  2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: Guid("307381b8-1dd1-4877-8a09-eb86a91721eb")]

// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Tests cannot run in parallel due to fakes overridding classes during other tests
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]
