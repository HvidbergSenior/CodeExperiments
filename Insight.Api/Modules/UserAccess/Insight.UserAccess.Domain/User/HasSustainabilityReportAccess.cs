using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User
{
    public sealed class HasSustainabilityReportAccess : ValueObject
    {
        public bool Value { get; private set; }

        private HasSustainabilityReportAccess()
        {
            Value = false;
        }

        private HasSustainabilityReportAccess(bool value)
        {
            Value = value;
        }

        public static HasSustainabilityReportAccess Create(bool value)
        {
            return new HasSustainabilityReportAccess(value);
        }

        public static HasSustainabilityReportAccess None()
        {
            return new HasSustainabilityReportAccess(false);
        }
    }
}
