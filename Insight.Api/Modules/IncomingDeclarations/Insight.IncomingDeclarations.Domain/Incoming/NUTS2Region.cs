
using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class NUTS2Region : ValueObject
    {
        public string Value { get; private set; }

        private NUTS2Region()
        {
            Value = string.Empty;
        }

        private NUTS2Region(string value)
        {
            Value = value;
        }

        public static NUTS2Region Create(string value)
        {
            return new NUTS2Region(value);
        }

        public static NUTS2Region Empty()
        {
            return new NUTS2Region();
        }
    }
}
