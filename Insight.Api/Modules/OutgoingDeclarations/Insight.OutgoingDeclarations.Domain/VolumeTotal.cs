using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class VolumeTotal : ValueObject
    {
        public decimal Value { get; private set; }

        private VolumeTotal()
        {
            Value = default;
        }

        private VolumeTotal(decimal value)
        {
            Value = value;
        }

        public static VolumeTotal Create(decimal value)
        {
            return new VolumeTotal(value);
        }

        public static VolumeTotal Empty()
        {
            return new VolumeTotal();
        }
    }
}