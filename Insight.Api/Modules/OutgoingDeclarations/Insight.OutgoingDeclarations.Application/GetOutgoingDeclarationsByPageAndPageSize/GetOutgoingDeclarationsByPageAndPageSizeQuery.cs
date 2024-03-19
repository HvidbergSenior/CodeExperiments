using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationById;
using Insight.OutgoingDeclarations.Domain;

namespace Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationsByPageAndPageSize
{
    public sealed class GetOutgoingDeclarationsByPageAndPageSizeQuery : IQuery<GetOutgoingDeclarationsByPageAndPageSizeResponse>
    {
        public PaginationParameters PaginationParameters { get; set; }
        public SortingParameters SortingParameters { get; set; }
        public FilteringParameters FilteringParameters { get; set; }

        private GetOutgoingDeclarationsByPageAndPageSizeQuery(PaginationParameters paginationParameters, SortingParameters sortingParameters, FilteringParameters filteringParameters)
        {
            PaginationParameters = paginationParameters;
            SortingParameters = sortingParameters;
            FilteringParameters = filteringParameters;
        }

        public static GetOutgoingDeclarationsByPageAndPageSizeQuery Create(PaginationParameters paginationParameters, SortingParameters sortingParameters, FilteringParameters filteringParameters)
        {
            return new GetOutgoingDeclarationsByPageAndPageSizeQuery(paginationParameters, sortingParameters, filteringParameters);
        }
    }

    internal class GetOutgoingDeclarationsByPageAndPageSizeQueryHandler : IQueryHandler<GetOutgoingDeclarationsByPageAndPageSizeQuery, GetOutgoingDeclarationsByPageAndPageSizeResponse>
    {
        private readonly IOutgoingDeclarationRepository outgoingDeclarationRepository;


        public GetOutgoingDeclarationsByPageAndPageSizeQueryHandler(IOutgoingDeclarationRepository outgoingDeclarationRepository)
        {
            this.outgoingDeclarationRepository = outgoingDeclarationRepository;
        }

        public async Task<GetOutgoingDeclarationsByPageAndPageSizeResponse> Handle(GetOutgoingDeclarationsByPageAndPageSizeQuery query, CancellationToken cancellationToken)
        {
            var (outgoingDeclarationsByPageAndPageSize, totalAmountOfDeclarations, hasMore) = await outgoingDeclarationRepository.GetOutgoingDeclarationsByPageNumberAndPageSize(query.PaginationParameters.Page, query.PaginationParameters.PageSize, query.SortingParameters, query.FilteringParameters, cancellationToken);
              
            var mappedDeclarations = MapToOutgoingDeclarationResponse(outgoingDeclarationsByPageAndPageSize);

            return new GetOutgoingDeclarationsByPageAndPageSizeResponse(mappedDeclarations, hasMore, totalAmountOfDeclarations);
        }

        private static List<OutgoingDeclarationResponse> MapToOutgoingDeclarationResponse(IEnumerable<OutgoingDeclaration> outgoingDeclarations)
        {
            var outgoingDeclarationResponses = new List<OutgoingDeclarationResponse>();
            
            foreach (var declaration in outgoingDeclarations)
            {
               outgoingDeclarationResponses.Add(new OutgoingDeclarationResponse(
                    declaration.OutgoingDeclarationId.Value,
                    declaration.Country.Value,
                    declaration.Product.Value,
                    declaration.CustomerDetails.CustomerNumber.Value,
                    declaration.CustomerDetails.CustomerName.Value,
                    declaration.VolumeTotal.Value,
                    declaration.AllocationTotal.Value,
                    declaration.GhgReduction.Value,
                    declaration.FossilFuelComparatorgCO2EqPerMJ.Value,
                    declaration.IncomingDeclarationPairings.Select(p=> p.IncomingDeclarationId.Value).ToList()
                ));
            }

            return outgoingDeclarationResponses;
        }
    }

    internal class GetOutgoingDeclarationsByPageAndPageSizeQueryAuthorizer : IAuthorizer<GetOutgoingDeclarationByIdQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetOutgoingDeclarationsByPageAndPageSizeQueryAuthorizer(IExecutionContext executionContext)
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