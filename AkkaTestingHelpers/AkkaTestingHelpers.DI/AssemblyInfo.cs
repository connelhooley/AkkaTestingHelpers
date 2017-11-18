using System.Runtime.CompilerServices;
using NullGuard;

[assembly: NullGuard(ValidationFlags.All)]
[assembly: InternalsVisibleTo("ConnelHooley.AkkaTestingHelpers.DI.Fakes")]
[assembly: InternalsVisibleTo("ConnelHooley.AkkaTestingHelpers.DI.SmallTests")]
[assembly: InternalsVisibleTo("ConnelHooley.AkkaTestingHelpers.DI.MediumTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("FileSystem.Fakes")]
[assembly: InternalsVisibleTo("FileSystem.Tests")]