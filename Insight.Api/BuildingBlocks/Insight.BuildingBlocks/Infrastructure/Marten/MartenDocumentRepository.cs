using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Exceptions;
using Marten;
using Marten.Pagination;

namespace Insight.BuildingBlocks.Infrastructure.Marten
{
    public class MartenDocumentRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly IDocumentSession _documentSession;
        private readonly IEntityEventsPublisher _aggregateEventsPublisher;

        public MartenDocumentRepository(IDocumentSession documentSession,
            IEntityEventsPublisher aggregateEventsPublisher)
        {
            _documentSession = documentSession;
            _aggregateEventsPublisher = aggregateEventsPublisher;
        }

        public Task<TEntity> Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Updated = DateTimeOffset.Now;
            _documentSession.Insert(entity);
            _aggregateEventsPublisher.TryEnqueueEventsFrom(entity, out _);
            return Task.FromResult(entity);
        }

        public Task<IEnumerable<TEntity>> Add(IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            var utcNow = DateTimeOffset.UtcNow;

            foreach (var entity in entities)
            {
                entity.Updated = utcNow;
                _documentSession.Insert(entity);
                _aggregateEventsPublisher.TryEnqueueEventsFrom(entity, out _);
            }

            return Task.FromResult(entities);
        }

        public Task Delete(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _documentSession.Delete(entity);
            _aggregateEventsPublisher.TryEnqueueEventsFrom(entity, out _);

            return Task.CompletedTask;
        }
        public Task Delete(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            
            foreach (var entity in entities)
            {
                _documentSession.Delete(entity);
                _aggregateEventsPublisher.TryEnqueueEventsFrom(entity, out _);
            }

            return Task.CompletedTask;
        }
        public Task<bool> DeleteById(Guid id, CancellationToken cancellationToken = default)
        {
            _documentSession.Delete<TEntity>(id);
            return Task.FromResult(true);
        }

        public Task<TEntity?> FindById(object id, CancellationToken cancellationToken = default)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Id needs to have value");
            }

            return id switch
            {
                Guid guid => _documentSession.LoadAsync<TEntity>(guid, cancellationToken),
                long l => _documentSession.LoadAsync<TEntity>(l, cancellationToken),
                int i => _documentSession.LoadAsync<TEntity>(i, cancellationToken),
                string s => _documentSession.LoadAsync<TEntity>(s, cancellationToken),
                _ => throw new ArgumentException("Id was of a type not supported.", nameof(id))
            };
        }

        public async Task<IEnumerable<TEntity>> GetAllByPaging(int pageNumber, int pageSize,
            CancellationToken cancellationToken = default)
        {   
            return await Query().ToPagedListAsync(pageNumber == 0 ? 1 : pageNumber, pageSize, cancellationToken);
        }


        public async Task<TEntity> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await FindById(id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException($"Entity {id} not found.");
            }

            return entity;
        }

        public IQueryable<TEntity> Query()
        {
            return _documentSession.Query<TEntity>();
        }

        public Task SaveChanges(CancellationToken cancellationToken = default)
        {
            return _aggregateEventsPublisher.Publish(cancellationToken);
        }

        public Task<TEntity> Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Updated = DateTimeOffset.UtcNow;
            _documentSession.Update(entity);
            _aggregateEventsPublisher.TryEnqueueEventsFrom(entity, out _);

            return Task.FromResult(entity);
        }
    }
}