using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class GHGEmissionSaving : ValueObject
    {
        public decimal Value { get; private set; }

        private GHGEmissionSaving()
        {
            Value = decimal.Zero;
        }

        private GHGEmissionSaving(decimal value)
        {
            Value = value;
        }

        public static GHGEmissionSaving Create(decimal value)
        {
            return new GHGEmissionSaving(value);
        }

        public static GHGEmissionSaving Empty()
        {
            return new GHGEmissionSaving();
        }
    }
}
