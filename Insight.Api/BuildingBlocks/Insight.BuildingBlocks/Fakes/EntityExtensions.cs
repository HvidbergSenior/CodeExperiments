using Insight.BuildingBlocks.Domain;

namespace Insight.BuildingBlocks.Fakes
{
    public static class EntityExtensions
    {
        public static T? PublishedEvent<T>(this Entity entity) where T : class, IDomainEvent
        {
            return entity.DomainEvents.LastOrDefault(e => e.GetType() == typeof(T)) as T;
        }
    }
}