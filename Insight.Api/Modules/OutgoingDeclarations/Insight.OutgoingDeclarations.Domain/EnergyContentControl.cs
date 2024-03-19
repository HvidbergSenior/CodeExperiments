using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class EnergyContentControl : ValueObject
    {
        public decimal Value { get; private set; }

        private EnergyContentControl()
        {
            Value = default;
        }

        private EnergyContentControl(decimal value)
        {
            Value = value;
        }

        public static EnergyContentControl Create(decimal company)
        {
            return new EnergyContentControl(company);
        }

        public static EnergyContentControl Empty()
        {
            return new EnergyContentControl();
        }
    }
}