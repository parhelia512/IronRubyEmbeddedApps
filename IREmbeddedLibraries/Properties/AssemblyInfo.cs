using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("IREmbeddedLibraries")]
[assembly: AssemblyDescription("Assembly containing Ruby source files from library. http://github.com/rifraf/IronRubyEmbeddedApps")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#elif BETA
[assembly: AssemblyConfiguration("Beta")]
#else
[assembly: AssemblyConfiguration("")]
#endif
[assembly: AssemblyCompany("djlSoft")]
[assembly: AssemblyProduct("IREmbeddedLibraries")]
[assembly: AssemblyCopyright("Copyright David Lake © 2010-2021")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]

[assembly: AssemblyVersion("1.3.0.32687")]
[assembly: AssemblyFileVersion("1.3.0.32687")]
[assembly: AssemblyInformationalVersionAttribute("1.3.0.32687")]
