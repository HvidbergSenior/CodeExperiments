using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.FuelTransactions.Domain;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.OutgoingDeclarations.Domain;
using Insight.Services.AllocationEngine.Domain;
using AdditionalInformation = Insight.OutgoingDeclarations.Domain.AdditionalInformation;
using Country = Insight.OutgoingDeclarations.Domain.Country;
using DateOfIssuance = Insight.OutgoingDeclarations.Domain.DateOfIssuance;
using DatePeriod = Insight.BuildingBlocks.Domain.DatePeriod;
using FuelTransactionId = Insight.OutgoingDeclarations.Domain.FuelTransactionId;
using IncomingDeclarationId = Insight.OutgoingDeclarations.Domain.IncomingDeclarationId;
using Product = Insight.OutgoingDeclarations.Domain.Product;
using ProductionCountry = Insight.OutgoingDeclarations.Domain.ProductionCountry;
using Quantity = Insight.OutgoingDeclarations.Domain.Quantity;

namespace Insight.Services.AllocationEngine.Application.PublishAllocations
{
    public sealed class PublishAllocationsCommand : ICommand<ICommandResponse>
    {
        private PublishAllocationsCommand()
        {
        }


        public static PublishAllocationsCommand Create(
           )
        {
            return new PublishAllocationsCommand();
        }
    }

    internal class PublishAllocationsCommandHandler : ICommandHandler<PublishAllocationsCommand, ICommandResponse>
    {
        private readonly IAllocationDraftRepository allocationDraftRepository;
        private readonly IIncomingDeclarationRepository incomingDeclarationRepository;
        private readonly IOutgoingDeclarationRepository outgoingDeclarationRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ISequenceBatchIdGenerator sequenceBatchIdGenerator;

        public PublishAllocationsCommandHandler(IAllocationDraftRepository allocationDraftRepository, IUnitOfWork unitOfWork, IIncomingDeclarationRepository incomingDeclarationRepository, IOutgoingDeclarationRepository outgoingDeclarationRepository, ISequenceBatchIdGenerator sequenceBatchIdGenerator)
        {
            this.allocationDraftRepository = allocationDraftRepository;
            this.unitOfWork = unitOfWork;
            this.incomingDeclarationRepository = incomingDeclarationRepository;
            this.outgoingDeclarationRepository = outgoingDeclarationRepository;
            this.sequenceBatchIdGenerator = sequenceBatchIdGenerator;
        }

        public async Task<ICommandResponse> Handle(PublishAllocationsCommand request,
            CancellationToken cancellationToken)
        {
            var allocationDraft = await allocationDraftRepository.FindById(AllocationDraftId.Instance.Value, cancellationToken);
            
            if (allocationDraft == null)
            {
                allocationDraft = AllocationDraft.Create();
                await allocationDraftRepository.Add(allocationDraft, cancellationToken);
            }

            if(!allocationDraft.IsLocked)
            {
                throw new BusinessException("Draft must be locked before being published");
            }

            var incomingDeclarationIds = allocationDraft.Allocations.SelectMany(c => c.Value.IncomingDeclarations.Select(o=> o.Key.Value)).ToArray();

            var affectedIncomingDeclarations = await incomingDeclarationRepository.GetByIdsAsync(incomingDeclarationIds, cancellationToken);
         
            var incomingDecDict = affectedIncomingDeclarations.ToDictionary(c => c.IncomingDeclarationId, c => c);

            var allocationsGroupedBy = allocationDraft.Allocations.Values.GroupBy(c => new { Customer = c.CustomerDetails.CustomerName.Value, Country = c.FuelTransactionCountry.Value, Product = c.ProductName.Value});
            
            var outgoingDeclarations = new List<OutgoingDeclaration>();

            foreach (var grouping in allocationsGroupedBy)
            {
                var fuelTransactionIdsOnDeclarationsOnAllocation = grouping.SelectMany(allocation => allocation.FuelTransactionIds.Select(fuelTransactionId => FuelTransactionId.Create(fuelTransactionId.Value))).ToList();
                var incomingDeclarationIdsOnAllocation = grouping.SelectMany(allocation => allocation.IncomingDeclarations.Keys.Where(incomingDeclarationId => incomingDecDict.ContainsKey(incomingDeclarationId))).ToList();

                decimal volumeBaselineSum = 0;
                decimal ghgBaselineSum = 0;
                
                foreach (var incomingId in incomingDeclarationIdsOnAllocation)
                {
                    var volume = incomingDecDict[incomingId].Quantity.Value;
                    var ghgReduction = incomingDecDict[incomingId].GhgEmissionSaving.Value;
                    var volumeMultiplier = incomingDecDict[incomingId].FossilFuelComparatorgCO2EqPerMJ.Value;

                    var volumeBaseline = (volume * 34 * volumeMultiplier) / 1000000;
                    var ghgBaseline = volumeBaseline * ghgReduction * 100;
                    volumeBaselineSum += volumeBaseline;
                    ghgBaselineSum += ghgBaseline;
                }

                var ghgWeighted = (volumeBaselineSum == 0 ? 0 : ghgBaselineSum / volumeBaselineSum) / 100;

                var pairingsList = new List<IncomingDeclarationIdPairing>();
                foreach (var incomingDeclaration in affectedIncomingDeclarations)
                {
                    var batchId = BatchId.Create(await sequenceBatchIdGenerator.GetNextBatchId(cancellationToken));
                    pairingsList.Add(IncomingDeclarationIdPairing.Create(
                        IncomingDeclarationId.Create(incomingDeclaration.IncomingDeclarationId.Value),
                        Quantity.Create(incomingDeclaration.Quantity.Value),
                        batchId));
                }

                outgoingDeclarations.Add(CreateOutgoingDeclaration(pairingsList, fuelTransactionIdsOnDeclarationsOnAllocation, grouping.Key.Product, grouping.ToList().First().AllocatedSum, grouping.Key.Country, grouping.ToList().First().CustomerDetails, ghgWeighted, allocationDraft.AllocationDraftId, DatePeriod.Create(grouping.ToList().First().StartDate, grouping.ToList().First().EndDate)));
            }

            await outgoingDeclarationRepository.Add(outgoingDeclarations, cancellationToken);
            await outgoingDeclarationRepository.SaveChanges(cancellationToken);
            await allocationDraftRepository.Delete(allocationDraft, cancellationToken);
            
            await unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }

        private static OutgoingDeclaration CreateOutgoingDeclaration(
            IReadOnlyList<IncomingDeclarationIdPairing> incomingDeclarationsPairings,
            IReadOnlyList<FuelTransactionId> fuelTransactionIds, string productName, decimal allocatedSum,
            string country, CustomerDetails customerDetails, decimal ghgWeighted,
            AllocationDraftId draftAllocationDraftId, DatePeriod datePeriod)
        {
            return OutgoingDeclaration.Create(
                OutgoingDeclarationId.Create(
                    Guid.NewGuid()),
                incomingDeclarationsPairings,
                fuelTransactionIds,
                Country.Create(country),
                Product.Create(productName),
                customerDetails,
                VolumeTotal.Create(allocatedSum),
                AllocationTotal.Create(allocatedSum),
                GhgReduction.Create(ghgWeighted),
                BfeId.Create("mangler"),
                Density.Create(33),
                datePeriod,
                CertificateId.Create("Mangler"),
                SustainabilityDeclarationNumber.Create("Mangler"),
                DateOfIssuance.Create(DateOnly.MaxValue),
                RawMaterialName.Create("Mangler"),
                RawMaterialCode.Create("Manger"),
                ProductionCountry.Create("Mangler"),
                AdditionalInformation.Create("Mangler"),
                Mt.Create(33),
                Liter.Create(33),
                EnergyContent.Create(33),
                GreenhouseGasEmission.Create(44),
                GreenhouseGasReduction.Create(44),
                EmissionSavingControl.Create(44),
                EnergyContentControl.Create(44),
                FossilFuelComparatorgCO2EqPerMJ.Create(333),
                DateOfCreation.Create(DateOnly.FromDateTime(DateTime.Now)),
                CustomerName.Create(customerDetails.CustomerName.Value),
                DraftAllocationId.Create(draftAllocationDraftId.Value)
            );
        }
    }
}