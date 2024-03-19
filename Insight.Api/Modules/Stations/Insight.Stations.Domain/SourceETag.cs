using Insight.BuildingBlocks.Domain;

namespace Insight.Stations.Domain
{
    public sealed class SourceETag : ValueObject
    {
        public string Value { get; private set; }

        private SourceETag()
        {
            Value = string.Empty;
        }

        private SourceETag(string value)
        {
            Value = value;
        }

        public static SourceETag Create(string value)
        {
            return new SourceETag(value);
        }

        public static SourceETag Empty()
        {
            return new SourceETag();
        }
    }
}