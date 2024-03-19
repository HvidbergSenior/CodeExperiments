using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class ChainOfCustodyOption : ValueObject
    {
        public string Value { get; private set; }

        private ChainOfCustodyOption()
        {
            Value = string.Empty;;
        }

        private ChainOfCustodyOption(string value)
        {
            Value = value;
        }

        public static ChainOfCustodyOption Create(string value)
        {
            return new ChainOfCustodyOption(value);
        }

        public static ChainOfCustodyOption Empty()
        {
            return new ChainOfCustodyOption();
        }
    }
}
