using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationById;
using Insight.OutgoingDeclarations.Domain;

namespace Insight.OutgoingDeclarations.Application.GetOutgoingDeclarations
{
    public sealed class GetOutgoingDeclarationsQuery : IQuery<GetOutgoingDeclarationsResponse>
    {
        private GetOutgoingDeclarationsQuery()
        {
        }

        public static GetOutgoingDeclarationsQuery Create(
        )
        {
            return new GetOutgoingDeclarationsQuery();
        }
    }

    internal class GetOutgoingDeclarationsQueryHandler : IQueryHandler<GetOutgoingDeclarationsQuery, GetOutgoingDeclarationsResponse>
    {
        private readonly IOutgoingDeclarationRepository outgoingDeclarationRepository;

        public GetOutgoingDeclarationsQueryHandler(
            IOutgoingDeclarationRepository outgoingDeclarationRepository)
        {
            this.outgoingDeclarationRepository = outgoingDeclarationRepository;
        }

        public async Task<GetOutgoingDeclarationsResponse> Handle(GetOutgoingDeclarationsQuery query, CancellationToken cancellationToken)
        {
            var outgoingDeclarations = await outgoingDeclarationRepository.GetAllAsync(cancellationToken);
            
            var mappedDeclarations = MapToOutgoingDeclarationResponse(outgoingDeclarations);

            return new GetOutgoingDeclarationsResponse(mappedDeclarations);
        }

        private static List<OutgoingDeclarationsResponse> MapToOutgoingDeclarationResponse(IEnumerable<OutgoingDeclaration> outgoingDeclarations)
        {
            var outgoingDeclarationResponses = new List<OutgoingDeclarationsResponse>();
            
            foreach (var declaration in outgoingDeclarations)
            {
                outgoingDeclarationResponses.Add(
                    new OutgoingDeclarationsResponse(
                        declaration.OutgoingDeclarationId.Value,
                        declaration.Country.Value,
                        declaration.Product.Value,
                        declaration.CustomerDetails.CustomerNumber.Value,
                        declaration.CustomerDetails.CustomerName.Value
                    ));
            }

            return outgoingDeclarationResponses;
        }
    }

    internal class GetOutgoingDeclarationsQueryAuthorizer : IAuthorizer<GetOutgoingDeclarationByIdQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetOutgoingDeclarationsQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetOutgoingDeclarationByIdQuery query,
            CancellationToken cancellation)
        {
            if (await executionContext.GetAdminPrivileges(cancellation))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}