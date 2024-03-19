using Microsoft.Extensions.Configuration;

namespace Insight.BuildingBlocks.Infrastructure.Environment
{
    public interface IEnvironment
    {
        string EnvironmentName { get; }
        bool IsDevelopmentEnvironment();
        bool IsDemoEnvironment();
        bool IsProductionEnvironment();
        bool IsTestEnvironment();
    }

    public class Environment : IEnvironment
    {
        private readonly IConfiguration _configuration;

        private readonly string environmentNameLocalDevelopment = "Development";

        private const string EnvironmentNameDev = "DEV";
        private const string EnvironmentNameDemo = "DEMO";
        private const string EnvironmentNameProd = "PROD";
        private const string EnvironmentNameTest = "Test";

        private const string EnvironmentSection = "Environment";
        private const string EnvironmentNameKey = "Name";

        public Environment(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string EnvironmentName => _configuration.GetSection(EnvironmentSection).GetValue<string>(EnvironmentNameKey) ?? "";

        public bool IsDevelopmentEnvironment()
        {
            return EnvironmentName.Equals(EnvironmentNameDev, StringComparison.OrdinalIgnoreCase) ||
                   EnvironmentName.Equals(environmentNameLocalDevelopment, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsDemoEnvironment()
        {
            return EnvironmentName.Equals(EnvironmentNameDemo, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsProductionEnvironment()
        {
            return EnvironmentName.Equals(EnvironmentNameProd, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsTestEnvironment()
        {
            return EnvironmentName.Equals(EnvironmentNameTest, StringComparison.OrdinalIgnoreCase);
        }
    }
}