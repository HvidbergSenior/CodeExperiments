namespace Insight.BuildingBlocks.Domain
{
    public abstract class DomainEvent : IDomainEvent
    {
        protected DomainEvent()
        {
            Id = Guid.NewGuid();
            OccurredOn = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; }

        public DateTimeOffset OccurredOn { get; }
    }
}
