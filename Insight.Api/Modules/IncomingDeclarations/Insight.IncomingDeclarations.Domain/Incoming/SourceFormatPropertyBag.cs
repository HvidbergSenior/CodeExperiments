using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.Incoming
{
    public sealed class SourceFormatPropertyBag : ValueObject
    {
        public string Value { get; private set; }

        private SourceFormatPropertyBag()
        {
            Value = string.Empty;
        }

        private SourceFormatPropertyBag(string value)
        {
            Value = value;
        }

        public static SourceFormatPropertyBag Create(string value)
        {
            return new SourceFormatPropertyBag(value);
        }

        public static SourceFormatPropertyBag Empty()
        {
            return new SourceFormatPropertyBag();
        }
    }
}
