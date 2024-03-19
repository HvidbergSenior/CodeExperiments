using System.Reflection;
using Insight.WebApplication;
using NetArchTest.Rules;

namespace Insight.Tests.Architecture
{
    public abstract class TestBase
    {
        protected static Assembly Insight => typeof(ApplicationTarget).Assembly;

        public const string WebapplicationNamespace = "Insight.WebApplication";

        public const string DeclarationsNamespace = "Insight.Modules.Declarations";
        
        public const string CustomerNamespace = "Insight.Modules.Customer";

        public const string UserAccessNamespace = "Insight.Modules.UserAccess";
        public const string CertificateNamespace = "Insight.Modules.Certificates";


        protected static void AssertAreImmutable(IEnumerable<Type> types)
        {
            IList<Type> failingTypes = new List<Type>();
            foreach (var type in types)
            {
                if (type.GetFields().Any(x => !x.IsInitOnly) || type.GetProperties().Any(x => x.CanWrite))
                {
                    failingTypes.Add(type);
                    break;
                }
            }

            AssertFailingTypes(failingTypes);
        }

        protected static void AssertFailingTypes(IEnumerable<Type> types)
        {
            if (types == null)
            {
                Assert.True(1 == 1);
            }
            else
            {
                Assert.Empty(types);
            }
        }

        protected static void AssertArchTestResult(TestResult result)
        {
            AssertFailingTypes(result.FailingTypes);
        }
    }
}
