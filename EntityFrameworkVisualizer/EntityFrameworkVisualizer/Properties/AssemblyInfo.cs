using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using EntityFrameworkVisualizer;
using EntityFrameworkVisualizer.VisualizerObjectSources;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("EntityFrameworkVisualizer")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Kirill Polishchuk")]
[assembly: AssemblyProduct("EntityFrameworkVisualizer")]
[assembly: AssemblyCopyright("Copyright © Kirill Polishchuk 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("432a9b89-5db4-4681-861c-95cb3169a0c6")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("3.0.*")]

[assembly: DebuggerVisualizer(typeof(EntitySqlVisualizer), typeof(DbQueryVisualizerObjectSource), Target = typeof(DbQuery<>), Description = "Entity SQL Visualizer")]