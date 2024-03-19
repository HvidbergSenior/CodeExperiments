using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.Services.AllocationEngine.Domain;
using Insight.Services.AllocationEngine.Service;

namespace Insight.Services.AllocationEngine.Application.GetAllocationSuggestions
{
    public sealed class GetAllocationSuggestionsQuery : IQuery<GetAllocationSuggestionsResponse>
    {
        private GetAllocationSuggestionsQuery(SuggestionRequest suggestionRequest, int page, int pageSize, bool isOrderDescending, string orderByProperty)
        {
            SuggestionRequest = suggestionRequest;
            Page = page;
            PageSize = pageSize;
            IsOrderDescending = isOrderDescending;
            OrderByProperty = orderByProperty;
        }

        public SuggestionRequest SuggestionRequest { get; }
        public int Page { get; }
        public int PageSize { get; }
        public bool IsOrderDescending { get; }
        public string OrderByProperty { get; }

        public static GetAllocationSuggestionsQuery Create(SuggestionRequest suggestionRequest, int page, int pageSize, bool isOrderDescending, string orderByProperty)
        {
            return new GetAllocationSuggestionsQuery(suggestionRequest, page, pageSize, isOrderDescending, orderByProperty);
        }
    }

    public class GetAllocationSuggestionsQueryHandler : IQueryHandler<GetAllocationSuggestionsQuery, GetAllocationSuggestionsResponse>
    {
        private readonly AllocationService allocationService;

        public GetAllocationSuggestionsQueryHandler(AllocationService allocationService)
        {
            this.allocationService = allocationService;
        }

        public async Task<GetAllocationSuggestionsResponse> Handle(GetAllocationSuggestionsQuery request, CancellationToken cancellationToken)
        {
            var suggestions = await allocationService.GetSuggestionsAsync(request.SuggestionRequest, cancellationToken);
            var suggestionResponses = suggestions.Select(c => new SuggestionResponse()
            {
                Id = c.Id,
                Company = c.Company,
                CountryOfOrigin = c.CountryOfOrigin,
                Country = c.Country,
                GHGReduction = c.GHGReduction,
                IncomingDeclarationState = c.IncomingDeclarationState,
                Period = c.Period,
                PosNumber = c.PosNumber,
                Product = c.Product,
                RawMaterial = c.RawMaterial,
                Storage = c.Storage,
                Supplier = c.Supplier,
                Volume = c.Volume,
                VolumeAvailable = c.VolumeAvailable,
                Warnings = c.Warnings
            });
            return new GetAllocationSuggestionsResponse(suggestionResponses.ToArray(), false, suggestionResponses.Count());
        }
    }

    internal class GetAllocationSuggestionsQueryAuthorizer : IAuthorizer<GetAllocationSuggestionsQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetAllocationSuggestionsQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetAllocationSuggestionsQuery query,
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
