using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class BfeId : ValueObject
    {
        public string Value { get; private set; }

        private BfeId()
        {
            Value = string.Empty;
        }

        private BfeId(string value)
        {
            Value = value;
        }

        public static BfeId Create(string company)
        {
            return new BfeId(company);
        }

        public static BfeId Empty()
        {
            return new BfeId();
        }
    }
}