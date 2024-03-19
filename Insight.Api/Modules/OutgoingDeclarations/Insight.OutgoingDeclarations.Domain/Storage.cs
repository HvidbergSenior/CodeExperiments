using Insight.BuildingBlocks.Domain;

namespace Insight.OutgoingDeclarations.Domain
{
    public sealed class Storage : ValueObject
    {
        public string Value { get; private set; }

        private Storage()
        {
            Value = string.Empty;;
        }

        private Storage(string value)
        {
            Value = value;
        }

        public static Storage Create(string value)
        {
            return new Storage(value);
        }

        public static Storage Empty()
        {
            return new Storage();
        }
    }
}