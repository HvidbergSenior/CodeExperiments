using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class DateOfInstallation : ValueObject
    {
        public string Value { get; private set; }

        private DateOfInstallation()
        {
            Value = string.Empty;
        }

        private DateOfInstallation(string value)
        {
            Value = value;
        }

        public static DateOfInstallation Create(string value)
        {
            return new DateOfInstallation(value);
        }

        public static DateOfInstallation Empty()
        {
            return new DateOfInstallation();
        }
    }
}
