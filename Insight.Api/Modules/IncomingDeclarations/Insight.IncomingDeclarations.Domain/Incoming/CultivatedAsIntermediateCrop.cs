using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class CultivatedAsIntermediateCrop : ValueObject
    {
        public bool Value { get; private set; }

        private CultivatedAsIntermediateCrop()
        {
            Value = false;
        }

        private CultivatedAsIntermediateCrop(bool value)
        {
            Value = value;
        }

        public static CultivatedAsIntermediateCrop Create(bool value)
        {
            return new CultivatedAsIntermediateCrop(value);
        }

        public static CultivatedAsIntermediateCrop None()
        {
            return new CultivatedAsIntermediateCrop();
        }
    }
}
