using System.Runtime.CompilerServices;
using NullGuard;

[assembly: NullGuard(ValidationFlags.All)]
[assembly: InternalsVisibleTo("ConnelHooley.AkkaTestingHelpers.SmallTests")]
[assembly: InternalsVisibleTo("ConnelHooley.AkkaTestingHelpers.MediumTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]