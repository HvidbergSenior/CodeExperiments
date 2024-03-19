using System.Reflection;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Mono.Cecil;
using NetArchTest.Rules;

namespace Insight.Tests.Architecture.CommandsAndQueries
{
    public class AuthorizerTests : TestBase
    {
        private static readonly IList<Assembly> Assemblies = new List<Assembly>
        {
            typeof(WebApplication.ApplicationTarget).Assembly,
            typeof(Customers.Application.AssemblyReference).Assembly,
            typeof(UserAccess.Application.AssemblyReference).Assembly,
            typeof(FuelTransactions.Application.AssemblyReference).Assembly,
            typeof(IncomingDeclarations.Application.AssemblyReference).Assembly,
            typeof(OutgoingDeclarations.Application.AssemblyReference).Assembly,
            typeof(PdfGenerator.Application.AssemblyReference).Assembly,
            typeof(Stations.Application.AssemblyReference).Assembly,
            typeof(BusinessCentralEntities.Application.AssemblyReference).Assembly,
        };

        private static readonly IList<Type> ExcludedClasses = new List<Type>
        {
        };

        [Fact]
        public void Commands_Should_Have_Authorizers()
        {
            var result = Types.InAssemblies(Assemblies)
                .That()
                .ImplementInterface(typeof(ICommand<>))
                .Should()
                .MeetCustomRule(new AuthorizerExistsRule())
                .GetResult();

            AssertArchTestResult(result);
        }

        [Fact]
        public void Queries_Should_Have_Authorizers()
        {
            var result = Types.InAssemblies(Assemblies)
                .That()
                .ImplementInterface(typeof(IQuery<>))
                .Should()
                .MeetCustomRule(new AuthorizerExistsRule())
                .GetResult();

            AssertArchTestResult(result);
        }

        private class AuthorizerExistsRule : ICustomRule
        {
            public bool MeetsRule(TypeDefinition type)
            {
                if (ExcludedClasses.Any(c => c.Name.Equals(type.Name, StringComparison.Ordinal)))
                {
                    return true;
                }

                var authorizers = Types.InAssemblies(Assemblies).That().ImplementInterface(typeof(IAuthorizer<>))
                    .GetTypes().ToList();
                return authorizers.Any(t => t.Name.Equals(type.Name + "Authorizer", StringComparison.Ordinal));
            }
        }
    }
}