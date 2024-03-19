using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Infrastructure;
using Insight.Customers.Domain.Repositories;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.OutgoingDeclarations.Domain;
using Insight.Services.AllocationEngine.Domain;
using Company = Insight.OutgoingDeclarations.Domain.Company;
using FilteringParameters = Insight.OutgoingDeclarations.Domain.FilteringParameters;
using IncomingDeclarationId = Insight.IncomingDeclarations.Domain.Incoming.IncomingDeclarationId;
using PaginationParameters = Insight.OutgoingDeclarations.Domain.PaginationParameters;
using Product = Insight.IncomingDeclarations.Domain.Incoming.Product;
using SortingParameters = Insight.OutgoingDeclarations.Domain.SortingParameters;

namespace Insight.Services.AllocationEngine.Application.GetAllocations
{
    public sealed class GetAllocationsQuery : IQuery<GetAllocationsResponse>
    {
        public FilteringParameters FilteringParameters { get; }
        public SortingParameters SortingParameters { get; }
        public PaginationParameters PaginationParameters { get; }

        private GetAllocationsQuery(FilteringParameters filterParameters, SortingParameters sortingParameters, PaginationParameters paginationParameters)
        {
            FilteringParameters = filterParameters;
            SortingParameters = sortingParameters;
            PaginationParameters = paginationParameters;
        }
     
        public static GetAllocationsQuery Create(FilteringParameters filteringParameters, SortingParameters sortingParameters, PaginationParameters paginationParameters)
        {
            return new GetAllocationsQuery(filteringParameters, sortingParameters, paginationParameters);
        }
    }

    internal class GetAllocationsQueryHandler : IQueryHandler<GetAllocationsQuery, GetAllocationsResponse>
    {
        private readonly IAllocationDraftRepository allocationDraftRepository;
        private readonly IIncomingDeclarationRepository incomingDeclarationRepository;

        public GetAllocationsQueryHandler(IAllocationDraftRepository allocationDraftRepository, IIncomingDeclarationRepository incomingDeclarationRepository)
        {
            this.allocationDraftRepository = allocationDraftRepository;
            this.incomingDeclarationRepository = incomingDeclarationRepository;
        }

        public async Task<GetAllocationsResponse> Handle(GetAllocationsQuery query, CancellationToken cancellationToken)
        {
            var allocationDraft = await allocationDraftRepository.FindById(AllocationDraftId.Instance.Value, cancellationToken);
            
            if (allocationDraft == null)
            {
                allocationDraft = AllocationDraft.Create();
                await allocationDraftRepository.Add(allocationDraft, cancellationToken);
            }
            var affectedAllocations =  FilterAffectedAllocations(query, allocationDraft.Allocations);

            var incomingDeclarationIds = affectedAllocations.SelectMany(c => c.Value.IncomingDeclarations.Select(o=> o.Key.Value)).ToArray();
            
            IEnumerable<IncomingDeclaration> affectedIncomingDeclarations = await incomingDeclarationRepository.GetByIdsAsync(incomingDeclarationIds, cancellationToken);

            affectedIncomingDeclarations = FilterIncomingByCompany(query, affectedIncomingDeclarations);
         
            var incomingDecDict = affectedIncomingDeclarations.ToDictionary(c => c.IncomingDeclarationId, c => c);

            var responseList = new List<AllocationResponse>();

            foreach(var allocation in affectedAllocations)
            {
                CreateAllocationResponse(allocation, incomingDecDict, responseList);
            }

            var pagesToSkip = query.PaginationParameters.Page <= 0 ? 1 : query.PaginationParameters.Page;
            var pageSize = query.PaginationParameters.PageSize;
            var skip = pagesToSkip * pageSize - pageSize;

            var pagedResponseList = responseList.Skip(skip).Take(pageSize).ToList();

            // Responselist may contain more than the requested pagesize due to one-to-many relationship between allocations and incoming declarations
            var hasMoreAllocations = incomingDeclarationIds.Length - (query.PaginationParameters.Page <= 0 ? 1 : query.PaginationParameters.Page * query.PaginationParameters.PageSize) > 0;

            return new GetAllocationsResponse(pagedResponseList, hasMoreAllocations, incomingDeclarationIds.Length, allocationDraft.IsLocked);
        }

        private static IEnumerable<IncomingDeclaration> FilterIncomingByCompany(GetAllocationsQuery query, IEnumerable<IncomingDeclaration> affectedIncomingDeclarations)
        {
            if(!query.FilteringParameters.Company.Value.ToUpperInvariant().Equals(Company.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                affectedIncomingDeclarations = affectedIncomingDeclarations.Where(d => d.Company.Value.ToUpperInvariant().Contains(query.FilteringParameters.Company.Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
            }

            return affectedIncomingDeclarations;
        }

        private static void CreateAllocationResponse(KeyValuePair<AllocationId, Allocation> allocation, Dictionary<IncomingDeclarationId, IncomingDeclaration> incomingDecDict, List<AllocationResponse> responseList)
        {
            foreach (var incomingDeclaration in allocation.Value.IncomingDeclarations)
            {
                if (!incomingDecDict.ContainsKey(incomingDeclaration.Key))
                {
                    continue;
                }
                    
                var responseEntry = new AllocationResponse()
                {
                    PosNumber = incomingDecDict[incomingDeclaration.Key].PosNumber.Value,
                    CertificationSystem = incomingDecDict[incomingDeclaration.Key].CertificationSystem.Value,
                    Company = incomingDecDict[incomingDeclaration.Key].Company.Value,
                    Country = incomingDecDict[incomingDeclaration.Key].Country.Value,
                    CountryOfOrigin = incomingDecDict[incomingDeclaration.Key].CountryOfOrigin.Value,
                    Customer = allocation.Value.CustomerDetails.CustomerName.Value,
                    GHGReduction = incomingDecDict[incomingDeclaration.Key].GhgEmissionSaving.Value,
                    Id = allocation.Value.AllocationId.Value,
                    Product = incomingDecDict[incomingDeclaration.Key].Product.Value,
                    RawMaterial = incomingDecDict[incomingDeclaration.Key].RawMaterial.Value,
                    Storage = incomingDecDict[incomingDeclaration.Key].PlaceOfDispatch.Value,
                    Volume = allocation.Value.IncomingDeclarations[incomingDeclaration.Key].Value,
                    Warnings = allocation.Value.Warnings,
                    FossilFuelComparatorgCO2EqPerMJ = incomingDecDict[incomingDeclaration.Key]
                        .FossilFuelComparatorgCO2EqPerMJ.Value,
                    CustomerNumber = allocation.Value.CustomerDetails.CustomerNumber.Value
                };
                    
                responseList.Add(responseEntry);
            }
        }

        private static IEnumerable<KeyValuePair<AllocationId, Allocation>> FilterAffectedAllocations(GetAllocationsQuery query, IEnumerable<KeyValuePair<AllocationId, Allocation>> affectedAllocations)
        {
            if (!query.FilteringParameters.Product.Value.ToUpperInvariant().Equals(Product.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                affectedAllocations = affectedAllocations.Where(d => d.Value.ProductName.Value.ToUpperInvariant().Contains(query.FilteringParameters.Product.Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
            }
           
            if(!query.FilteringParameters.CustomerName.Value.ToUpperInvariant().Equals(CustomerName.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                affectedAllocations = affectedAllocations.Where(d => d.Value.CustomerDetails.CustomerName.Value.ToUpperInvariant().Contains(query.FilteringParameters.CustomerName.Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));
            }

            affectedAllocations = query.SortingParameters.IsOrderDescending ? affectedAllocations.OrderByDescending(c => query.SortingParameters.OrderByProperty.FirstCharToUpper()) : affectedAllocations.OrderBy(c => query.SortingParameters.OrderByProperty.FirstCharToUpper());
            return affectedAllocations;
        }

        internal class GetAllocationsQueryAuthorizer : IAuthorizer<GetAllocationsQuery>
        {
            private readonly IExecutionContext executionContext;

            public GetAllocationsQueryAuthorizer(IExecutionContext executionContext)
            {   
                this.executionContext = executionContext;
            }

            public async Task<AuthorizationResult> Authorize(GetAllocationsQuery query,
                CancellationToken cancellation)
            {
                if(await executionContext.GetAdminPrivileges(cancellation))
                {
                    return AuthorizationResult.Succeed();
                }

                return AuthorizationResult.Fail();
            }
        }
    }
}