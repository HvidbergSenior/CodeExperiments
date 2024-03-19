using System.Reflection;

namespace Insight.PdfGenerator.Application
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
    }
}
