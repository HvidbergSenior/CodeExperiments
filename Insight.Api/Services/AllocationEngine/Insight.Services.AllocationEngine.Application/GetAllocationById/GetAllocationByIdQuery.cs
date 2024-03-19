using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Infrastructure;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.Services.AllocationEngine.Domain;

namespace Insight.Services.AllocationEngine.Application.GetAllocationById
{
    public sealed class GetAllocationByIdQuery : IQuery<GetAllocationByIdResponse>
    {
        public AllocationId AllocationId { get; set; }
        public SortingParameters SortingParameters { get; set; }

        private GetAllocationByIdQuery(AllocationId allocationId, SortingParameters sortingParameters)
        {
            AllocationId = allocationId;
            SortingParameters = sortingParameters;
        }

        public static GetAllocationByIdQuery Create(AllocationId allocationId, SortingParameters sortingParameters)
        {
            return new GetAllocationByIdQuery(allocationId, sortingParameters);
        }
    }

    internal class GetAllocationByIdQueryHandler : IQueryHandler<GetAllocationByIdQuery, GetAllocationByIdResponse>
    {
        private readonly IAllocationDraftRepository allocationDraftRepository;
        private readonly IIncomingDeclarationRepository incomingDeclarationRepository;

        public GetAllocationByIdQueryHandler(IAllocationDraftRepository allocationDraftRepository, IQueryBus queryBus, IIncomingDeclarationRepository incomingDeclarationRepository)
        {
            this.allocationDraftRepository = allocationDraftRepository;
            this.incomingDeclarationRepository = incomingDeclarationRepository;
        }

        public async Task<GetAllocationByIdResponse> Handle(GetAllocationByIdQuery query,
            CancellationToken cancellationToken)
        {
            var allocationDraft = await allocationDraftRepository.FindById(AllocationDraftId.Instance.Value, cancellationToken);

            if (allocationDraft == null)
            {
                allocationDraft = AllocationDraft.Create();
                await allocationDraftRepository.Add(allocationDraft, cancellationToken);
            }

            var allocation = allocationDraft?.Allocations.FirstOrDefault(p => p.Key == query.AllocationId).Value;

            if (allocation == null)
            {
                allocation = Allocation.Empty();
            }

            var guids = allocation.IncomingDeclarations.Keys.Select(key => key.Value).ToList();
            var affectedIncomingDeclarations = await incomingDeclarationRepository.GetByIdsAsync(guids, cancellationToken);

            affectedIncomingDeclarations = query.SortingParameters.IsOrderDescending ? affectedIncomingDeclarations.OrderByDescending(c => query.SortingParameters.OrderByProperty.FirstCharToUpper()) : affectedIncomingDeclarations.OrderBy(c => query.SortingParameters.OrderByProperty.FirstCharToUpper());
                
            var allocationIncomingDeclarationDtos = affectedIncomingDeclarations.Select(incomingDeclaration =>
                new AllocationIncomingDeclarationDto(
                    incomingDeclaration.Company.Value,
                    incomingDeclaration.Country.Value,
                    incomingDeclaration.Product.Value,
                    incomingDeclaration.Supplier.Value,
                    incomingDeclaration.RawMaterial.Value,
                    incomingDeclaration.PosNumber.Value,
                    incomingDeclaration.CountryOfOrigin.Value,
                    incomingDeclaration.PlaceOfDispatch.Value,
                    incomingDeclaration.DateOfDispatch.Value,
                    incomingDeclaration.Quantity.Value,
                    incomingDeclaration.GhgEmissionSaving.Value
                )).ToList();

            return MapToAllocationIdResponse(allocation, allocationIncomingDeclarationDtos);
        }

        private static GetAllocationByIdResponse MapToAllocationIdResponse(Allocation allocation, List<AllocationIncomingDeclarationDto> allocationIncomingDeclarationDtos)
        {
            return new GetAllocationByIdResponse(new AllocationByIdResponse()
                {
                    Country = allocation.FuelTransactionCountry.Value,
                    Customer = allocation.CustomerDetails.CustomerName.Value,
                    Id = allocation.AllocationId.Value,
                    Product = allocation.ProductName.Value,
                    Volume = allocation.AllocatedSum,
                    CustomerNumber = allocation.CustomerDetails.CustomerNumber.Value
                }, allocationIncomingDeclarationDtos
            );
        }
    }

    internal class GetAllocationByIdQueryAuthorizer : IAuthorizer<GetAllocationByIdQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetAllocationByIdQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetAllocationByIdQuery query,
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