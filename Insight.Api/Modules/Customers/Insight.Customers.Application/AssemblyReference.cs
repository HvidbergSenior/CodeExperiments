using System.Reflection;

namespace Insight.Customers.Application
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
