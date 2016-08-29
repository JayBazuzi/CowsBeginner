using System.Reflection;
using System.Runtime.InteropServices;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;

[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.0.*")]
[assembly: UseReporter(typeof(DiffReporter))]
[assembly: UseApprovalSubdirectory("_approvals")]