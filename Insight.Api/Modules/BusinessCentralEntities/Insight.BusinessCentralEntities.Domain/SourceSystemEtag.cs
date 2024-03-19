using Insight.BuildingBlocks.Domain;

namespace Insight.BusinessCentralEntities.Domain
{
    public sealed class SourcesystemEtag : ValueObject
    {
        public string Value { get; private set; }

        private SourcesystemEtag()
        {
            Value = string.Empty;
        }

        private SourcesystemEtag(string value)
        {
            Value = value;
        }

        public static SourcesystemEtag Create(string value)
        {
            return new SourcesystemEtag(value);
        }

        public static SourcesystemEtag Empty()
        {
            return new SourcesystemEtag();
        }
    }
}