using System.Text;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Marten.Services;

namespace Insight.BuildingBlocks.Fakes
{
    public class InMemoryRepository<T> : IRepository<T>, IReadonlyRepository<T> where T : Entity
    {
        public IDictionary<Guid, string> Entities { get; }

        public IDictionary<Guid, T> Values => Entities.ToDictionary(c => c.Key, c => Deserialize(c.Value));

        public bool SaveChangesCalled { get; private set; }
        private JsonNetSerializer JsonSerializer { get; }

        public InMemoryRepository()
        {
            Entities = new Dictionary<Guid, string>();
            SaveChangesCalled = false;
            JsonSerializer = MartenRegistration.GetJsonNetSerializer();
        }

        public Task<T> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            Entities.TryGetValue(id, out var entity);
            if (entity == null)
                throw new NotFoundException($"Entity {id} not found.");

            return Task.FromResult(Deserialize(entity));
        }

        public Task<T?> FindById(object id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Id needs to have value");
            }

            if (id is Guid guid)
            {
                Entities.TryGetValue(guid, out var entity);
                return DeserializeNullable(entity);
            }
            return Task.FromResult((T?)null);
        }

        public Task<T> Add(T entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            Entities.Add(entity.Id, JsonSerializer.ToJson(entity));
            return Task.FromResult(entity);
        }

        public Task<IEnumerable<T>> Add(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (var entity in entities)
            {
                Entities.Add(entity.Id, JsonSerializer.ToJson(entity));
            }
            return Task.FromResult(entities);
        }

        public Task<T> Update(T entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if(!Entities.Remove(entity.Id))
            {
                throw new Exception("Unable to update entity in in-memory repo. Unable to remove key. (or not found)");
            }
            Entities.Add(entity.Id, JsonSerializer.ToJson(entity));
            return Task.FromResult(entity);
        }

        public Task Delete(T entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);

            Entities.Remove(entity.Id);
            return Task.CompletedTask;
        }
        public Task Delete(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entities);

            foreach (var entity in entities)
            {
                ArgumentNullException.ThrowIfNull(entity);
                Entities.Remove(entity.Id);
            }
            
            return Task.CompletedTask;
        }
        public Task<bool> DeleteById(Guid id, CancellationToken cancellationToken = default)
        {
            Entities.Remove(id);
            return Task.FromResult(true);
        }

        public Task SaveChanges(CancellationToken cancellationToken = default)
        {
            SaveChangesCalled = true;
            return Task.CompletedTask;
        }

        public IQueryable<T> Query()
        {
            return Entities.Values.Select(Deserialize).AsQueryable();
        }

        private Task<T?> DeserializeNullable(string? json)
        {
            if (json is null)
            {
                return Task.FromResult(default(T?));
            }
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return Task.FromResult(JsonSerializer.FromJson<T?>(ms));
        }

        protected T Deserialize(string json)
        {
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return JsonSerializer.FromJson<T>(ms);
        }

        public Task<IEnumerable<T>> GetAllByPaging(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            pageNumber = pageNumber == 0 ? 1 : pageNumber;

            var skip = pageNumber * pageSize - pageSize;

            return Task.FromResult(Entities.Values.Select(Deserialize).Skip(skip).Take(pageSize));
        }
    }
}
