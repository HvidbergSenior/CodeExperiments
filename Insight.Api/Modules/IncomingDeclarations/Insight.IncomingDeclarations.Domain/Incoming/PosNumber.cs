using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class PosNumber : ValueObject
    {
        public string Value { get; private set; }

        private PosNumber()
        {
            Value = string.Empty;
        }

        private PosNumber(string value)
        {
            Value = value;
        }

        public static PosNumber Create(string value)
        {
            return new PosNumber(value);
        }

        public static PosNumber Empty()
        {
            return new PosNumber();
        }
    }
}
