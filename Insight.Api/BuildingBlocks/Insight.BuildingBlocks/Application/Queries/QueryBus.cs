using MediatR;
namespace Insight.BuildingBlocks.Application.Queries
{
    public class QueryBus : IQueryBus
    {
        private readonly IMediator mediator;

        public QueryBus(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<TResponse> Send<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery<TResponse>
        {
            return mediator.Send(query, cancellationToken);
        }
    }
}
