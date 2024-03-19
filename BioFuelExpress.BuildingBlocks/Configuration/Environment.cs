using Microsoft.Extensions.Configuration;

namespace BioFuelExpress.BuildingBlocks.Configuration
{
    public interface IEnvironment
    {
        string EnvironmentName { get; }
        bool IsDevelopmentEnvironment();
        bool IsDemoEnvironment();
        bool IsProductionEnvironment();
    }

    public class Environment : IEnvironment
    {
        private readonly IConfiguration _configuration;

        private readonly string _EnvironmentNameLocalDevelopment = "Development";

        private readonly string _EnvironmentNameDev = "DEV";
        private readonly string _EnvironmentNameDemo = "DEMO";
        private readonly string _EnvironmentNameProd = "PROD";

        private readonly string _environmentSection = "Environment";
        private readonly string _environmentNameKey = "Name";

        public Environment(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string EnvironmentName
        {
            get
            {
                return "Fix this";
                //return _configuration.GetSection(_environmentSection).GetValue<string>(_environmentNameKey);
            }
        }

        public bool IsDevelopmentEnvironment()
        {
            return EnvironmentName.Equals(_EnvironmentNameDev, StringComparison.CurrentCultureIgnoreCase) || EnvironmentName.Equals(_EnvironmentNameLocalDevelopment, StringComparison.CurrentCultureIgnoreCase);
        }

        public bool IsDemoEnvironment()
        {

            return EnvironmentName.Equals(_EnvironmentNameDemo, StringComparison.CurrentCultureIgnoreCase);
        }

        public bool IsProductionEnvironment()
        {
            return EnvironmentName.Equals(_EnvironmentNameProd, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
