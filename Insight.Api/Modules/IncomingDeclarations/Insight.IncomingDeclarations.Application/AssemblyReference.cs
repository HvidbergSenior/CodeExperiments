using System.Reflection;

namespace Insight.IncomingDeclarations.Application
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
