using AutoFixture;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.FuelTransactions.Domain;
using Insight.OutgoingDeclarations.Domain;
using Insight.OutgoingDeclarations.Infrastructure;
using Marten;
using Microsoft.IdentityModel.Tokens;
using AdditionalInformation = Insight.OutgoingDeclarations.Domain.AdditionalInformation;
using Country = Insight.OutgoingDeclarations.Domain.Country;
using DateOfIssuance = Insight.OutgoingDeclarations.Domain.DateOfIssuance;
using FilteringParameters = Insight.OutgoingDeclarations.Domain.FilteringParameters;
using FuelTransactionId = Insight.OutgoingDeclarations.Domain.FuelTransactionId;
using Product = Insight.OutgoingDeclarations.Domain.Product;
using ProductionCountry = Insight.OutgoingDeclarations.Domain.ProductionCountry;
using Quantity = Insight.OutgoingDeclarations.Domain.Quantity;

namespace Insight.Tests.End2End.TestData
{
    public static class OutgoingDeclarationsTestData
    {
        public static async Task<IEnumerable<OutgoingDeclaration>> SeedWithOutgoingDeclaration(int count, FilteringParameters filteringParameters, 
            WebAppFixture fixture, List<IncomingDeclarationIdPairing> incomingDeclarationPairings, bool useExistingDeclarationsIfAny = true )
        {
            Fixture autoFixture = new();

            var sessionFactory = (ISessionFactory?)fixture.AppFactory.Services.GetService(typeof(ISessionFactory))!;
            var entityEventsPublisher =
                (IEntityEventsPublisher?)fixture.AppFactory.Services.GetService(typeof(IEntityEventsPublisher))!;
            await using var documentSession = sessionFactory.OpenSession();
            var uow = new MartenUnitOfWork(documentSession);

            var outgoingDeclarationRepository =
                new OutgoingDeclarationRepository(documentSession, entityEventsPublisher);

            var outgoingDeclarations = new List<OutgoingDeclaration>();

            var hasOutgoingDeclarations = false;
           
            if (useExistingDeclarationsIfAny)
            {
                hasOutgoingDeclarations = await outgoingDeclarationRepository.AnyAsync(CancellationToken.None);
            }

            if (!hasOutgoingDeclarations)
            {
                for (var i = 0; i < count; i++)
                {
                   autoFixture.Customizations.Add(new RandomDateTimeSequenceGenerator(minDate: DateTime.Now.AddYears(-5), maxDate: DateTime.Now));
                    var date = autoFixture.Create<DateTime>();
                    var dateAfter = date.AddDays(100);
                    
                    outgoingDeclarations.Add(OutgoingDeclaration.Create(
                        OutgoingDeclarationId.Create(autoFixture.Create<Guid>()),
                        incomingDeclarationPairings.IsNullOrEmpty() ? new List<IncomingDeclarationIdPairing>(){IncomingDeclarationIdPairing.Create(IncomingDeclarationId.Create(autoFixture.Create<Guid>()), Quantity.Create(autoFixture.Create<decimal>()), BatchId.Create(autoFixture.Create<long>()))} : incomingDeclarationPairings,
                        new List<FuelTransactionId>(){FuelTransactionId.Create(autoFixture.Create<Guid>())},
                        Country.Create(autoFixture.Create<string>()),
                        filteringParameters.Product.Value.IsNullOrEmpty()
                            ? Product.Create(autoFixture.Create<string>())
                            : Product.Create(filteringParameters.Product.Value),
                        CustomerDetails.Create(CustomerNumber.Create(autoFixture.Create<string>()), CustomerAddress.Create(autoFixture.Create<string>()), CustomerBillToName.Create(autoFixture.Create<string>()), CustomerBillToNumber.Create(autoFixture.Create<string>()), CustomerCity.Create(autoFixture.Create<string>()), CustomerDeliveryType.Create(autoFixture.Create<string>()), CustomerIndustry.Create(autoFixture.Create<string>()), filteringParameters.CustomerName.Value.IsNullOrEmpty()
                            ? CustomerName.Create(autoFixture.Create<string>())
                            : CustomerName.Create(filteringParameters.CustomerName.Value), CustomerPostCode.Create(autoFixture.Create<string>()), CustomerCountryRegion.Create(autoFixture.Create<string>())),
                        VolumeTotal.Create(autoFixture.Create<decimal>()),
                        AllocationTotal.Create(autoFixture.Create<decimal>()),
                        GhgReduction.Create(autoFixture.Create<decimal>()),
                        BfeId.Create(autoFixture.Create<string>()),
                        Density.Create(autoFixture.Create<decimal>()),
                        DatePeriod.Create(DateOnly.FromDateTime(date), DateOnly.FromDateTime(dateAfter)), 
                        CertificateId.Create(autoFixture.Create<string>()),
                        SustainabilityDeclarationNumber.Create(autoFixture.Create<string>()),
                        filteringParameters.DatePeriod.StartDate > DateOnly.MinValue ? DateOfIssuance.Create(filteringParameters.DatePeriod.StartDate) : DateOfIssuance.Create(DateOnly.FromDateTime(date)),
                        RawMaterialName.Create(autoFixture.Create<string>()),
                        RawMaterialCode.Create(autoFixture.Create<string>()),
                        ProductionCountry.Create(autoFixture.Create<string>()),
                        AdditionalInformation.Create(autoFixture.Create<string>()),
                        Mt.Create(autoFixture.Create<decimal>()),
                        Liter.Create(autoFixture.Create<decimal>()),
                        EnergyContent.Create(autoFixture.Create<decimal>()),
                        GreenhouseGasEmission.Create(autoFixture.Create<decimal>()),
                        GreenhouseGasReduction.Create(autoFixture.Create<decimal>()),
                        EmissionSavingControl.Create(autoFixture.Create<decimal>()),
                        EnergyContentControl.Create(autoFixture.Create<decimal>()),
                        FossilFuelComparatorgCO2EqPerMJ.Create(autoFixture.Create<decimal>()),
                        filteringParameters.DatePeriod.StartDate > DateOnly.MinValue ? DateOfCreation.Create(filteringParameters.DatePeriod.StartDate) : DateOfCreation.Create(DateOnly.FromDateTime(date)),
                        CustomerName.Create(autoFixture.Create<string>()),
                        DraftAllocationId.Create(autoFixture.Create<Guid>())
                    ));
                }

                await outgoingDeclarationRepository.Add(outgoingDeclarations);
                await outgoingDeclarationRepository.SaveChanges();
                await uow.Commit(CancellationToken.None);
            }
            else
            {
                outgoingDeclarations = (await outgoingDeclarationRepository.GetAllByPaging(1, count, CancellationToken.None)).ToList();
            }
            
            return outgoingDeclarations;
        }
    }
}