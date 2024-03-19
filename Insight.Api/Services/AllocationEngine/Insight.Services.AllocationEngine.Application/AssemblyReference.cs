using System.Reflection;

namespace Insight.Services.AllocationEngine.Application
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
