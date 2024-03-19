using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationById;
using Insight.OutgoingDeclarations.Domain;

namespace Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationsByCustomerId
{
    public sealed class GetOutgoingDeclarationsByCustomerIdQuery : IQuery<GetOutgoingDeclarationsByCustomerIdResponse>
    {
        public CustomerNumber CustomerNumber { get; set; }

        private GetOutgoingDeclarationsByCustomerIdQuery(
            CustomerNumber customerNumber)
        {
            CustomerNumber = customerNumber;
        }

        public static GetOutgoingDeclarationsByCustomerIdQuery Create(
            CustomerNumber customerNumber
        )
        {
            return new GetOutgoingDeclarationsByCustomerIdQuery(
                customerNumber);
        }
    }
    internal class GetOutgoingDeclarationsByCustomerIdQueryHandler : IQueryHandler<
        GetOutgoingDeclarationsByCustomerIdQuery, GetOutgoingDeclarationsByCustomerIdResponse>
    {
        private readonly IOutgoingDeclarationRepository outgoingDeclarationRepository;

        public GetOutgoingDeclarationsByCustomerIdQueryHandler(IOutgoingDeclarationRepository outgoingDeclarationRepository)
        {
            this.outgoingDeclarationRepository = outgoingDeclarationRepository;
        }

        public async Task<GetOutgoingDeclarationsByCustomerIdResponse> Handle(GetOutgoingDeclarationsByCustomerIdQuery query, CancellationToken cancellationToken)
        {
            var outgoingDeclarations =
                await outgoingDeclarationRepository.GetByCustomerNumberAsync(
                    query.CustomerNumber.Value,
                    cancellationToken);
            
            var mappedDeclarations = MapToOutgoingDeclarationResponse(outgoingDeclarations);

            return new GetOutgoingDeclarationsByCustomerIdResponse(mappedDeclarations);
        }

        private static IEnumerable<OutgoingDeclarationsByCustomerIdResponse> MapToOutgoingDeclarationResponse(IEnumerable<OutgoingDeclaration> outgoingDeclarations)
        {
            var outgoingDeclarationResponses = new List<OutgoingDeclarationsByCustomerIdResponse>();
            
            foreach (var outgoingDeclaration in outgoingDeclarations)
            {
                outgoingDeclarationResponses.Add(new OutgoingDeclarationsByCustomerIdResponse(
                    outgoingDeclaration.Id.ToString(),
                    outgoingDeclaration.Country.Value,
                    outgoingDeclaration.Product.Value,
                    outgoingDeclaration.CustomerDetails.CustomerNumber.Value,
                    outgoingDeclaration.CustomerDetails.CustomerName.Value
                ));
            }

            return outgoingDeclarationResponses;
        }
    }

    internal class GetOutgoingDeclarationsByCustomerIdQueryAuthorizer : IAuthorizer<GetOutgoingDeclarationByIdQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetOutgoingDeclarationsByCustomerIdQueryAuthorizer(IExecutionContext executionContext)
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