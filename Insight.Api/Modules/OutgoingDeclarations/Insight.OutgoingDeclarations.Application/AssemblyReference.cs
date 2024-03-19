using System.Reflection;

namespace Insight.OutgoingDeclarations.Application
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
