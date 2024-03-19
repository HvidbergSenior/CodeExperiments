using AutoFixture;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.Customers.Domain;
using Insight.FuelTransactions.Domain;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.Services.AllocationEngine.Domain;
using Insight.Services.AllocationEngine.Infrastructure;
using Marten;
using Quantity = Insight.IncomingDeclarations.Domain.Incoming.Quantity;

namespace Insight.Tests.End2End.TestData
{
    public static class AllocationDraftsTestData
    {
        public static async Task<IEnumerable<AllocationDraft>> SeedWithAllocationDraft(AllocationDraftState allocationDraftState, 
            WebAppFixture fixture, IReadOnlyList<FuelTransactionId> fuelTransactionsIds, Guid? incomingDeclarationId1 = null, Guid? incomingDeclarationId2 = null, string customerName = "", string country = "", string productName = "", string customerNumber = "")
        {
            Fixture autoFixture = new();

            var sessionFactory = (ISessionFactory?)fixture.AppFactory.Services.GetService(typeof(ISessionFactory))!;
            var entityEventsPublisher =
                (IEntityEventsPublisher?)fixture.AppFactory.Services.GetService(typeof(IEntityEventsPublisher))!;
            await using var documentSession = sessionFactory.OpenSession();
            
            var uow = new MartenUnitOfWork(documentSession);
            
            var allocationDrafts = new List<AllocationDraft>();

            var allocationDraftRepository = new AllocationDraftRepository(documentSession, entityEventsPublisher);

            var anyAllocationDraft = await allocationDraftRepository.AnyAsync();
            
            autoFixture.Customizations.Add(new RandomDateTimeSequenceGenerator(minDate: DateTime.Now.AddYears(-5),
                maxDate: DateTime.Now));
            var date = autoFixture.Create<DateTime>();
            var earlierDate = date.AddYears(-1);
           
            if (!anyAllocationDraft)
            {
                var allocationDraft = AllocationDraft.Create();
                allocationDraft.Unlock();

                var customerDetails = CustomerDetails.Create(
                    BuildingBlocks.Domain.CustomerNumber.Create(customerNumber),
                    CustomerAddress.Create("Address1"),
                    CustomerBillToName.Create("Cbn1"),
                    CustomerBillToNumber.Create("Cb1"),
                    CustomerCity.Create("City1"),
                    CustomerDeliveryType.Create("Cdtype1"),
                    CustomerIndustry.Create("Industry1"),
                    BuildingBlocks.Domain.CustomerName.Create(customerName),
                    CustomerPostCode.Create("CPC1"),
                    CustomerCountryRegion.Create("Country1"));

                var allocationSpecified = Allocation.Create(customerDetails,
                    FuelTransactionCountry.Create(country), ProductName.Create(productName),
                    DateOnly.FromDateTime(earlierDate),
                    DateOnly.FromDateTime(date));

                var incomingDeclarationIdAndQuantityCollection = new IncomingDeclarationIdAndQuantityCollection
                {
                    {
                        IncomingDeclarationId.Create(incomingDeclarationId1 ?? autoFixture.Create<Guid>()),
                        Quantity.Create(autoFixture.Create<decimal>())
                    },
                    {
                        IncomingDeclarationId.Create(incomingDeclarationId2 ?? autoFixture.Create<Guid>()),
                        Quantity.Create(autoFixture.Create<decimal>())
                    }
                };

                allocationSpecified.SetTransactionIds(fuelTransactionsIds.ToArray());
                allocationSpecified.SetAllocations(incomingDeclarationIdAndQuantityCollection);

                allocationDraft.AddAllocation(allocationSpecified);

                if (allocationDraftState == AllocationDraftState.Locked)
                {
                    allocationDraft.Lock();
                }
                else
                {
                    allocationDraft.Unlock();
                }

                allocationDrafts.Add(allocationDraft);

                await allocationDraftRepository.Add(allocationDraft);
                await allocationDraftRepository.SaveChanges();
                await uow.Commit(CancellationToken.None);
            }
            else
            {
                allocationDrafts = (await allocationDraftRepository.GetAllByPaging(1, 1, CancellationToken.None)).ToList();
                
                allocationDrafts.First().Unlock();

                var customerDetails = CustomerDetails.Create(
                    BuildingBlocks.Domain.CustomerNumber.Create(customerNumber),
                    CustomerAddress.Create("Address1"),
                    CustomerBillToName.Create("Cbn1"),
                    CustomerBillToNumber.Create("Cb1"),
                    CustomerCity.Create("City1"),
                    CustomerDeliveryType.Create("Cdtype1"),
                    CustomerIndustry.Create("Industry1"),
                    BuildingBlocks.Domain.CustomerName.Create(customerName),
                    CustomerPostCode.Create("CPC1"),
                    CustomerCountryRegion.Create("Country1"));

                var allocationSpecified = Allocation.Create(customerDetails,
                    FuelTransactionCountry.Create(country), ProductName.Create(productName),
                    DateOnly.FromDateTime(earlierDate),
                    DateOnly.FromDateTime(date));

                var incomingDeclarationIdAndQuantityCollection = new IncomingDeclarationIdAndQuantityCollection
                {
                    {
                        IncomingDeclarationId.Create(incomingDeclarationId1 ?? autoFixture.Create<Guid>()),
                        Quantity.Create(autoFixture.Create<decimal>())
                    },
                    {
                        IncomingDeclarationId.Create(incomingDeclarationId2 ?? autoFixture.Create<Guid>()),
                        Quantity.Create(autoFixture.Create<decimal>())
                    }
                };

                allocationSpecified.SetTransactionIds(fuelTransactionsIds.ToArray());
                allocationSpecified.SetAllocations(incomingDeclarationIdAndQuantityCollection);

                allocationDrafts.First().AddAllocation(allocationSpecified);
                
                if (allocationDraftState == AllocationDraftState.Locked)
                {
                    allocationDrafts.First().Lock();
                }
                else
                {
                    allocationDrafts.First().Unlock();
                }
                
                await allocationDraftRepository.Update(allocationDrafts.First());
                await allocationDraftRepository.SaveChanges();
                await uow.Commit(CancellationToken.None);
            }

            return allocationDrafts;
        }

        private static void CreateAllocations(int i, Fixture autoFixture, DateTime date, DateTime earlierDate, Guid incomingDeclarationId)
        {
          

        }
    }
}