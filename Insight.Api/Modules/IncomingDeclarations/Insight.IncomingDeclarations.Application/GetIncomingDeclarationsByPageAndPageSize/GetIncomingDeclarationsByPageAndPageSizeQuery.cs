using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.IncomingDeclarations.Application.GetIncomingDeclarationsByPageAndPageSize
{
    public sealed class GetIncomingDeclarationsByPageAndPageSizeQuery : IQuery<GetIncomingDeclarationsByPageAndPageSizeResponse>
    {
        public PaginationParameters PaginationParameters { get; set; }
        public SortingParameters SortingParameters { get; set; }
        public FilteringParameters FilteringParameters { get; set; }
        public IEnumerable<IncomingDeclarationState> IncomingDeclarationStates { get; set; }

        private GetIncomingDeclarationsByPageAndPageSizeQuery(PaginationParameters paginationParameters, SortingParameters sortingParameters,FilteringParameters filteringParameters,
            IEnumerable<IncomingDeclarationState> incomingDeclarationStates)
        {
            PaginationParameters = paginationParameters;
            SortingParameters = sortingParameters;
            FilteringParameters = filteringParameters;
            IncomingDeclarationStates = incomingDeclarationStates;
        }

        public static GetIncomingDeclarationsByPageAndPageSizeQuery Create(
            PaginationParameters paginationParameters, SortingParameters sortingParameters,
            FilteringParameters filteringParameters,
            IEnumerable<IncomingDeclarationState> incomingDeclarationStates)
        {
            return new GetIncomingDeclarationsByPageAndPageSizeQuery(paginationParameters, sortingParameters,filteringParameters,
                incomingDeclarationStates);
        }
    }
    internal class GetIncomingDeclarationsByPageAndPageSizeQueryHandler : IQueryHandler<GetIncomingDeclarationsByPageAndPageSizeQuery, GetIncomingDeclarationsByPageAndPageSizeResponse>
    {
        private readonly IIncomingDeclarationRepository incomingDeclarationRepository;

        public GetIncomingDeclarationsByPageAndPageSizeQueryHandler(IIncomingDeclarationRepository incomingDeclarationRepository)
        {
            this.incomingDeclarationRepository = incomingDeclarationRepository;
        }

        public async Task<GetIncomingDeclarationsByPageAndPageSizeResponse> Handle(GetIncomingDeclarationsByPageAndPageSizeQuery query, CancellationToken cancellationToken)
        {
            var (incomingDeclarationsByPageAndPageSize, totalAmountOfDeclarations, hasMore) = await incomingDeclarationRepository.GetIncomingDeclarationsByPageNumberAndPageSize(query.PaginationParameters.Page, query.PaginationParameters.PageSize, query.SortingParameters, query.FilteringParameters, query.IncomingDeclarationStates, cancellationToken);

            var mappedDeclarations = MapToIncomingDeclarationResponses(incomingDeclarationsByPageAndPageSize);

            return new GetIncomingDeclarationsByPageAndPageSizeResponse(mappedDeclarations, hasMore, totalAmountOfDeclarations);
        }

        private static List<IncomingDeclarationResponse> MapToIncomingDeclarationResponses(IEnumerable<IncomingDeclaration> incomingDeclarations)
        {
            var incomingDeclarationResponses = new List<IncomingDeclarationResponse>();

            foreach (var declaration in incomingDeclarations)
            {
                incomingDeclarationResponses.Add(
                    new IncomingDeclarationResponse(
                        declaration.Id.ToString(),
                        declaration.Company.Value,
                        declaration.Country.Value,
                        declaration.Product.Value,
                        declaration.Supplier.Value,
                        declaration.RawMaterial.Value,
                        declaration.CountryOfOrigin.Value,
                        declaration.PosNumber.Value,
                        declaration.IncomingDeclarationState,
                        declaration.PlaceOfDispatch.Value,
                        declaration.DateOfDispatch.Value,
                        declaration.Quantity.Value,
                        declaration.GhgEmissionSaving.Value,
                        declaration.RemainingVolume
                        ));
            }

            return incomingDeclarationResponses;
        }
    }

    internal class GetIncomingDeclarationsByPageAndPageSizeQueryAuthorizer : IAuthorizer<GetIncomingDeclarationsByPageAndPageSizeQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetIncomingDeclarationsByPageAndPageSizeQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetIncomingDeclarationsByPageAndPageSizeQuery query,
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