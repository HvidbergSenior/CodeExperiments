using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Exceptions;
using MediatR;

namespace Insight.BuildingBlocks.PipelineBehaviour
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IAuthorizer<TRequest>> _authorizers;

        public AuthorizationBehaviour(IEnumerable<IAuthorizer<TRequest>> authorizers)
        {
            _authorizers = authorizers;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            foreach (var authorizer in _authorizers)
            {
                var result = await authorizer.Authorize(request, cancellationToken);
                if (!result.IsAuthorized)
                {
                    throw new UnauthorizedException(result.FailureMessage);
                }
            }
            return await next();
        }

    }
}
