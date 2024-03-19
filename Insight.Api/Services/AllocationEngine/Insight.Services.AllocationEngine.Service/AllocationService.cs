using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.Customers.Domain;
using Insight.Customers.Domain.Repositories;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Domain.Allocations;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.Services.AllocationEngine.Domain;
using Marten;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quantity = Insight.IncomingDeclarations.Domain.Incoming.Quantity;

namespace Insight.Services.AllocationEngine.Service
{
    public class AllocationService
    {
        private readonly IIncomingDeclarationRepository incomingDeclarationRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IAllocationDraftRepository allocationDraftRepository;
        private readonly IFuelTransactionsAllocationRepository fuelTransactionsAllocationRepository;
        private readonly IIncomingDeclarationAllocationRepository incomingDeclarationAllocationRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AllocationService> logger;

        public AllocationService(IIncomingDeclarationRepository incomingDeclarationRepository, ICustomerRepository customerRepository, IAllocationDraftRepository allocationDraftRepository, IFuelTransactionsAllocationRepository fuelTransactionsAllocationRepository, IIncomingDeclarationAllocationRepository incomingDeclarationAllocationRepository, IUnitOfWork unitOfWork, ILogger<AllocationService> logger)
        {
            this.incomingDeclarationRepository = incomingDeclarationRepository;
            this.customerRepository = customerRepository;
            this.allocationDraftRepository = allocationDraftRepository;
            this.fuelTransactionsAllocationRepository = fuelTransactionsAllocationRepository;
            this.incomingDeclarationAllocationRepository = incomingDeclarationAllocationRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<IEnumerable<Suggestion>> GetSuggestionsAsync(SuggestionRequest suggestionRequest, CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetById(suggestionRequest.CustomerId.Value, cancellationToken);
            if (customer == null)
            {
                throw new NotFoundException("Customer not found");
            }

            return (await incomingDeclarationRepository.GetAllByProductCountryDispatchLocationDispatchDateAndCapacityAsync(suggestionRequest.Product, suggestionRequest.Country, suggestionRequest.PlaceOfDispatch, suggestionRequest.StartDate, suggestionRequest.EndDate, suggestionRequest.IsOrderDescending, suggestionRequest.OrderByProperty, cancellationToken))
                 .Select(c => new Suggestion()
                 {
                     Id = c.IncomingDeclarationId.Value,
                     PosNumber = c.PosNumber.Value,
                     CountryOfOrigin = c.CountryOfOrigin.Value,
                     Country = c.Country.Value,
                     IncomingDeclarationState = c.IncomingDeclarationState,
                     Period = c.DateOfDispatch.Value,
                     Product = c.Product.Value,
                     RawMaterial = c.RawMaterial.Value,
                     GHGReduction = c.GhgEmissionSaving.Value,
                     Company = c.Company.Value,
                     Storage = c.PlaceOfDispatch.Value,
                     Supplier = c.Supplier.Value,
                     Volume = c.Quantity.Value,
                     VolumeAvailable = c.RemainingVolume,
                     Warnings = GetWarnings(c, customer)
                 }).OrderByDescending(c => c.GHGReduction).ThenBy(c => c.VolumeAvailable);
        }

        public async Task ManuallyAllocate(ManualAllocationAssignments manualAllocationAssignments, CancellationToken cancellationToken)
        {
            var allocationDraft = await allocationDraftRepository.FindById(AllocationDraftId.Instance.Value, cancellationToken);

            if (allocationDraft == null)
            {
                allocationDraft = AllocationDraft.Create();
                await allocationDraftRepository.Add(allocationDraft, cancellationToken);
                await allocationDraftRepository.SaveChanges(cancellationToken);
                await unitOfWork.Commit(cancellationToken);
            }

            if (allocationDraft.AllocationDraftState == AllocationDraftState.Locked)
            {
                throw new BusinessException("Draft is locked"); // Todo: Add test.
            }

            var customer = await customerRepository.GetById(manualAllocationAssignments.FuelTransactionsBatch.CustomerId.Value, cancellationToken);
            
            if (customer == null)
            {
                throw new BusinessException($"Customer {manualAllocationAssignments.FuelTransactionsBatch.CustomerId} not found");
            }

            var affectedIncomingDeclarations = (await incomingDeclarationRepository.GetByIdsAsync(manualAllocationAssignments.AllocationAssignments.Select(c => c.IncomingDeclarationId.Value), cancellationToken)).ToDictionary(c => c.IncomingDeclarationId, c => c);

            var draftAllocationRowResponse = await fuelTransactionsAllocationRepository.DraftAllocateFuelTransactionsPartiallyAsync(DraftAllocationId.Create(allocationDraft.TemporaryAllocationId.Value),
                manualAllocationAssignments.FuelTransactionsBatch.CustomerId,
                manualAllocationAssignments.FuelTransactionsBatch.StartDate,
                manualAllocationAssignments.FuelTransactionsBatch.EndDate,
                manualAllocationAssignments.FuelTransactionsBatch.ProductNumber,
                manualAllocationAssignments.FuelTransactionsBatch.LocationId, 
                FuelTransactions.Domain.Quantity.Create(manualAllocationAssignments.AllocationAssignments.Sum(c => c.Volume.Value)), cancellationToken);

            if (draftAllocationRowResponse.Quantity.Value > manualAllocationAssignments.AllocationAssignments.Sum(c => c.Volume.Value))
            {
                await RollBackAsync(manualAllocationAssignments, allocationDraft, draftAllocationRowResponse, cancellationToken);

                throw new BusinessException($"Volume of affected FuelTransactions {draftAllocationRowResponse.Quantity.Value} does not match the sum of IncomingDeclarations Volume {manualAllocationAssignments.AllocationAssignments.Sum(c => c.Volume.Value)}");
            }

            var allocationDict = new IncomingDeclarationIdAndQuantityCollection();

            var allocation = Allocation.Create(customer.CustomerDetails,
                                               manualAllocationAssignments.FuelTransactionsBatch.Country,
                                               manualAllocationAssignments.FuelTransactionsBatch.ProductName,
                                               manualAllocationAssignments.FuelTransactionsBatch.StartDate,
                                               manualAllocationAssignments.FuelTransactionsBatch.EndDate);

            foreach (var assignment in manualAllocationAssignments.AllocationAssignments)
            {
                var allocationUpdatedSuccess = affectedIncomingDeclarations[assignment.IncomingDeclarationId].AddAllocation(allocation.AllocationId.Value, assignment.Volume.Value);
                if (!allocationUpdatedSuccess)
                {
                    await RollBackAsync(manualAllocationAssignments, allocationDraft, draftAllocationRowResponse, cancellationToken);
                    throw new BusinessException("Allocated volume cannot exceed available volume");
                }
                allocationDict.Add(assignment.IncomingDeclarationId, assignment.Volume);
            }


            allocation.SetAllocations(allocationDict);
            allocation.SetTransactionIds(draftAllocationRowResponse.FuelTransactionIds);

            var success = allocationDraft.AddAllocation(allocation);
            if (!success)
            {
                await RollBackAsync(manualAllocationAssignments, allocationDraft, draftAllocationRowResponse, cancellationToken);

                throw new BusinessException("Allocation failed");
            }

            foreach (var incomingDeclaration in affectedIncomingDeclarations.Values)
            {
                await incomingDeclarationRepository.Update(incomingDeclaration, cancellationToken);
            }
            await incomingDeclarationRepository.SaveChanges(cancellationToken);

            await allocationDraftRepository.Update(allocationDraft, cancellationToken);
            await allocationDraftRepository.SaveChanges(cancellationToken);

            await unitOfWork.Commit(cancellationToken); // TODO: Check if patches work without calling savechanges on Idocumentsession.
        }

        private async Task RollBackAsync(ManualAllocationAssignments manualAllocationAssignments, AllocationDraft allocationDraft, DraftAllocationRowResponse draftAllocationRowResponse, CancellationToken cancellationToken)
        {
            await fuelTransactionsAllocationRepository.RollBackDraftAllocationAsync(DraftAllocationId.Create(allocationDraft.TemporaryAllocationId.Value),
                            manualAllocationAssignments.FuelTransactionsBatch.CustomerId,
                            manualAllocationAssignments.FuelTransactionsBatch.StartDate,
                            manualAllocationAssignments.FuelTransactionsBatch.EndDate,
                            manualAllocationAssignments.FuelTransactionsBatch.ProductNumber,
                            manualAllocationAssignments.FuelTransactionsBatch.LocationId,
                            draftAllocationRowResponse.FuelTransactionIds, cancellationToken);
        }

        public async Task ClearDraftAsync(CancellationToken cancellationToken)
        {
            var allocationDraft = await allocationDraftRepository.FindById(AllocationDraftId.Instance.Value, cancellationToken);
            if (allocationDraft == null)
            {
                return;
            }

            if (!allocationDraft.AllowClear)
            {
                throw new BusinessException("Draft is locked!");
            }

            var incomingDeclarationsToClear = new Dictionary<IncomingDeclarationId, List<Guid>>();
            foreach(var allocation in allocationDraft.Allocations)
            {
                foreach(var assignment in allocation.Value.IncomingDeclarations)
                {
                    incomingDeclarationsToClear.TryAdd(assignment.Key, new List<Guid>());

                    incomingDeclarationsToClear[assignment.Key].Add(allocation.Key.Value);
                }
            }
            await incomingDeclarationAllocationRepository.ClearDraftAllocationAsync(incomingDeclarationsToClear, cancellationToken);

            await fuelTransactionsAllocationRepository.ClearAllocationDraftIdAsync(DraftAllocationId.Create(allocationDraft.TemporaryAllocationId.Value), cancellationToken);
            await allocationDraftRepository.Delete(allocationDraft, cancellationToken);
            await unitOfWork.Commit(cancellationToken);
        }

        public async Task AutoAllocate(DateOnly startDate, DateOnly endDate, FilterProductName filterProduct, FilterCompanyName filterCompany, FilterCustomerName filterCustomer, CancellationToken cancellationToken)
        {
            var allocationDraft = await allocationDraftRepository.FindById(AllocationDraftId.Instance.Value, cancellationToken);

            if (allocationDraft == null)
            {
                allocationDraft = AllocationDraft.Create();
                await allocationDraftRepository.Add(allocationDraft, cancellationToken);
                await allocationDraftRepository.SaveChanges(cancellationToken);
                await unitOfWork.Commit(cancellationToken);
            }

            if (allocationDraft.IsLocked)
            {
                throw new BusinessException("Allocation Draft is locked");
            }

            var validIncomingDeclarations = await incomingDeclarationRepository.GetAllByDateRangeAndFilterAsync(startDate, endDate, filterProduct.Value, filterCompany.Value, cancellationToken);

            IEnumerable<MissingAllocationRowResponse> stuffToAllocate = await fuelTransactionsAllocationRepository.GetMissingAllocationForPeriodAndFiltersAsync(startDate, endDate, filterProduct.Value, filterCompany.Value, filterCustomer.Value, cancellationToken);

            var customersInPlay = (await customerRepository.GetCustomersByIdsAsync(stuffToAllocate.Select(c => CustomerId.Create(c.FuelTransactionCustomerId.Value)), cancellationToken)).ToDictionary(c => c.Id, c => c);

            var allocationCollection = new List<Allocation>();
            
            // Customers with highest CO2 target are allocated first, then highest quantity.
            foreach (var batchToAllocate in stuffToAllocate
                .OrderBy(c => customersInPlay.OrderByDescending(o => o.Value.CO2Target.Value).Select(o => o.Key).ToList().IndexOf(c.FuelTransactionCustomerId.Value))
                .ThenBy(c => c.Quantity.Value))
            {
                if (!customersInPlay.ContainsKey(batchToAllocate.FuelTransactionCustomerId.Value))
                {
                    logger.LogWarning("Customer {CustomerId} not found", batchToAllocate.FuelTransactionCustomerId.Value);
                    continue;
                }
                var alloc = Allocation.Create(customersInPlay[batchToAllocate.FuelTransactionCustomerId.Value].CustomerDetails, batchToAllocate.FuelTransactionCountry, batchToAllocate.ProductName, startDate, endDate);
                var allWarnings = new List<string>();
                IncomingDeclarationIdAndQuantityCollection allocations = new();
                decimal remainingVolumeToAllocate = batchToAllocate.Quantity.Value;

                var potentialDeclarations = validIncomingDeclarations.Where(c => c.RemainingVolume > 0 && c.Product.Value == batchToAllocate.ProductName.Value && (batchToAllocate.Location.Value == "EXTERNAL" || batchToAllocate.LocationId.Value == $"{c.Country.Value}:{c.PlaceOfDispatch.Value}"));
                var rankedResults = potentialDeclarations
                    .Select(c => new { IncomingDeclaration = c, Warnings = GetWarnings(c, customersInPlay[batchToAllocate.FuelTransactionCustomerId.Value]) })
                    .OrderBy(c => c.Warnings.Length)
                    .ThenByDescending(c => c.IncomingDeclaration.RemainingVolume)
                    .Select(c => c.IncomingDeclaration);

                foreach (var incomingDeclaration in rankedResults)
                {
                    if (incomingDeclaration.RemainingVolume >= remainingVolumeToAllocate)
                    {
                        var warnings = GetWarnings(incomingDeclaration, customersInPlay[batchToAllocate.FuelTransactionCustomerId.Value]);
                        if (warnings.Any())
                        {
                            allWarnings.AddRange(warnings);
                        }
                        // Enough to cover it all in one.
                        allocations.Add(incomingDeclaration.IncomingDeclarationId, Quantity.Create(batchToAllocate.Quantity.Value));
                        incomingDeclaration.Allocations.Value.Add(alloc.AllocationId.Value, batchToAllocate.Quantity.Value);
                        remainingVolumeToAllocate = 0;
                    }
                    else
                    {
                        var warnings = GetWarnings(incomingDeclaration, customersInPlay[batchToAllocate.FuelTransactionCustomerId.Value]);
                        if (warnings.Any())
                        {
                            allWarnings.AddRange(warnings);
                        }
                        allocations.Add(incomingDeclaration.IncomingDeclarationId, Quantity.Create(incomingDeclaration.RemainingVolume));
                        remainingVolumeToAllocate -= incomingDeclaration.RemainingVolume;
                        incomingDeclaration.Allocations.Value.Add(alloc.AllocationId.Value, incomingDeclaration.RemainingVolume);
                    }
                    await incomingDeclarationRepository.Update(incomingDeclaration, cancellationToken);
                    if (remainingVolumeToAllocate == 0)
                    {
                        break;
                    }
                }

                if (batchToAllocate.Quantity.Value == remainingVolumeToAllocate)
                {
                    logger.LogWarning("Unable to allocate {AllocationBatch}", JsonConvert.SerializeObject(batchToAllocate));
                }
                else
                {
                    DraftAllocationRowResponse fuelTransactionIds;

                    if (remainingVolumeToAllocate > 0)
                    {
                        logger.LogWarning("Unable to fully allocate {AllocationBatch}, Remaining: {RemainingVolumeToAllocate}", JsonConvert.SerializeObject(batchToAllocate), remainingVolumeToAllocate);


                        fuelTransactionIds = await fuelTransactionsAllocationRepository.DraftAllocateFuelTransactionsPartiallyAsync(DraftAllocationId.Create(allocationDraft.TemporaryAllocationId.Value),
                                                                batchToAllocate.FuelTransactionCustomerId,
                                                                startDate,
                                                                endDate,
                                                                batchToAllocate.ProductNumber,
                                                                batchToAllocate.LocationId,
                                                                FuelTransactions.Domain.Quantity.Create(batchToAllocate.Quantity.Value - remainingVolumeToAllocate),
                                                                cancellationToken);

                    }
                    else
                    {
                        fuelTransactionIds = await fuelTransactionsAllocationRepository
                        .DraftAllocateFuelTransactionsAsync(DraftAllocationId.Create(allocationDraft.TemporaryAllocationId.Value),
                                                                batchToAllocate.FuelTransactionCustomerId,
                                                                startDate,
                                                                endDate,
                                                                batchToAllocate.ProductNumber,
                                                                batchToAllocate.LocationId,
                                                                cancellationToken);
                    }

                    alloc.SetAllocations(allocations);
                    alloc.SetTransactionIds(fuelTransactionIds.FuelTransactionIds);
                    alloc.SetWarnings(allWarnings.ToArray());

                    // Log the allocation
                    allocationCollection.Add(alloc);
                }
            }

            allocationCollection.ForEach(c => allocationDraft.AddAllocation(c));
            await incomingDeclarationRepository.SaveChanges(cancellationToken);
            await allocationDraftRepository.Update(allocationDraft, cancellationToken);
            await allocationDraftRepository.SaveChanges(cancellationToken);
            await unitOfWork.Commit(cancellationToken);
        }

        private static string[] GetWarnings(IncomingDeclaration incomingDeclaration, Customer customer)
        {
            ArgumentNullException.ThrowIfNull(incomingDeclaration, nameof(incomingDeclaration));
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));

            var warnings = new List<string>();

            if (customer.CO2Target.Value != 0)
            {
                if (incomingDeclaration.GhgEmissionSaving.Value < customer.CO2Target.Value)
                {
                    warnings.Add($"Customer has a higher CO₂ Target ({customer.CO2Target.Value}");
                }
            }

            if (customer.AllowedRawMaterials.Value.Count != 0)
            {
                if (customer.AllowedRawMaterials.Value.Where(c => c.Value).Any()) // Positve list found.
                {
                    var allowed = customer.AllowedRawMaterials.Value.Where(c => c.Value).Select(c => c.Key).ToArray();
                    if (!incomingDeclaration.RawMaterial.Value.In(allowed))
                    {
                        warnings.Add($"Raw material not allowed. Allowed: {string.Join(",", allowed)}");
                    }
                }
                else
                {
                    var notAllowed = customer.AllowedRawMaterials.Value.Where(c => c.Value).Select(c => c.Key).ToArray();
                    if (incomingDeclaration.RawMaterial.Value.In(notAllowed))
                    {
                        warnings.Add($"Raw material not allowed");
                    }
                }
            }

            return warnings.ToArray();
        }
    }


}
