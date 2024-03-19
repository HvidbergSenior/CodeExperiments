using Insight.BuildingBlocks.Domain;

namespace Insight.Stations.Domain
{
    public sealed class StationId : ValueObject
    {
        public Guid Value { get; private set; }

        private StationId()
        {
            Value = Guid.Empty;
        }

        private StationId(Guid value)
        {
            Value = value;
        }

        public static StationId Create(Guid value)
        {
            return new StationId(value);
        }

        public static StationId Empty()
        {
            return new StationId();
        }
    }
}