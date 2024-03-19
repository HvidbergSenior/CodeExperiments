using AutoFixture;
using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Infrastructure.Incoming;
using Marten;
using Microsoft.IdentityModel.Tokens;
using Quantity = Insight.IncomingDeclarations.Domain.Incoming.Quantity;

namespace Insight.Tests.End2End.TestData
{
    public static class IncomingDeclarationsTestData
    {
        public static async Task<IEnumerable<IncomingDeclaration>> SeedWithIncomingDeclaration(int count, FilteringParameters filteringParameters, 
            WebAppFixture fixture, IncomingDeclarationState incomingDeclarationState = IncomingDeclarationState.New, bool useExistingDeclarationsIfAny = true, string placeOfDispatch = "", string country = "")
        {
            Fixture autoFixture = new();

            var sessionFactory = (ISessionFactory?)fixture.AppFactory.Services.GetService(typeof(ISessionFactory))!;
            var entityEventsPublisher =
                (IEntityEventsPublisher?)fixture.AppFactory.Services.GetService(typeof(IEntityEventsPublisher))!;
            await using var documentSession = sessionFactory.OpenSession();
            var uow = new MartenUnitOfWork(documentSession);

            var incomingDeclarationRepository =
                new IncomingDeclarationRepository(documentSession, entityEventsPublisher);

            var incomingDeclarations = new List<IncomingDeclaration>();

            var hasIncomingDeclarations = false;

            if (useExistingDeclarationsIfAny)
            {
                hasIncomingDeclarations = await incomingDeclarationRepository.AnyAsync(CancellationToken.None);
            }

            if (!hasIncomingDeclarations)
            {
                for (var i = 0; i < count; i++)
                {
                    autoFixture.Customizations.Add(new RandomDateTimeSequenceGenerator(minDate: DateTime.Now.AddYears(-5), maxDate: DateTime.Now));
                    var date = autoFixture.Create<DateTime>();

                    incomingDeclarations.Add(IncomingDeclaration.Create(
                        IncomingDeclarationId.Create(autoFixture.Create<Guid>()),
                        filteringParameters.Company.Value.IsNullOrEmpty()? Company.Create(autoFixture.Create<string>()) : Company.Create(filteringParameters.Company.Value),
                        country.IsNullOrEmpty() ? Country.Create(autoFixture.Create<string>()) : Country.Create(country),
                        filteringParameters.Product.Value.IsNullOrEmpty()? Product.Create(autoFixture.Create<string>()) : Product.Create(filteringParameters.Product.Value),
                        filteringParameters.DatePeriod.StartDate > DateOnly.MinValue ? DateOfDispatch.Create(filteringParameters.DatePeriod.StartDate) : DateOfDispatch.Create(DateOnly.FromDateTime(date)),
                        filteringParameters.Supplier.Value.IsNullOrEmpty()? Supplier.Create(autoFixture.Create<string>()) : Supplier.Create(filteringParameters.Supplier.Value),
                        CertificationSystem.Create(autoFixture.Create<string>()),
                        SupplierCertificateNumber.Create(autoFixture.Create<string>()),
                        PosNumber.Create(autoFixture.Create<string>()),
                        DateOfIssuance.Create(DateOnly.FromDateTime(date)),
                        placeOfDispatch.IsNullOrEmpty() ? PlaceOfDispatch.Create(autoFixture.Create<string>()) : PlaceOfDispatch.Create(placeOfDispatch),
                        ProductionCountry.Create(autoFixture.Create<string>()),
                        DateOfInstallation.Create(autoFixture.Create<string>()),
                        TypeOfProduct.Create(autoFixture.Create<string>()),
                        RawMaterial.Create(autoFixture.Create<string>()),
                        AdditionalInformation.Create(autoFixture.Create<string>()),
                        CountryOfOrigin.Create(autoFixture.Create<string>()),
                        Quantity.Create(autoFixture.Create<decimal>()),
                        UnitOfMeasurement.Litres,
                        EnergyContentMJ.Create(autoFixture.Create<decimal>()),
                        EnergyQuantityGJ.Create(autoFixture.Create<decimal>()),
                        ComplianceWithSustainabilityCriteria.Create(autoFixture.Create<bool>()),
                        ComplianceWithEuRedMaterialCriteria.Create(autoFixture.Create<bool>()),
                        ComplianceWithIsccMaterialCriteria.Create(autoFixture.Create<bool>()),
                        ChainOfCustodyOption.Create(autoFixture.Create<string>()),
                        CultivatedAsIntermediateCrop.Create(autoFixture.Create<bool>()),
                        FulfillsMeasuresForLowILUCRiskFeedstocks.Create(autoFixture.Create<bool>()),
                        MeetsDefinitionOfWasteOrResidue.Create(autoFixture.Create<bool>()),
                        NUTS2Region.Create(autoFixture.Create<string>()),
                        TotalDefaultValueAccordingToRED2.Create(autoFixture.Create<bool>()),
                        GHGEec.Create(autoFixture.Create<decimal>()),
                        GHGEl.Create(autoFixture.Create<decimal>()),
                        GHGEp.Create(autoFixture.Create<decimal>()),
                        GHGEtd.Create(autoFixture.Create<decimal>()),
                        GHGEu.Create(autoFixture.Create<decimal>()),
                        GHGEsca.Create(autoFixture.Create<decimal>()),
                        GHGEccs.Create(autoFixture.Create<decimal>()),
                        GHGEccr.Create(autoFixture.Create<decimal>()),
                        GHGEee.Create(autoFixture.Create<decimal>()),
                        GHGgCO2eqPerMJ.Create(autoFixture.Create<decimal>()),
                        FossilFuelComparatorgCO2eqPerMJ.Create(autoFixture.Create<decimal>()),
                        GHGEmissionSaving.Create(autoFixture.Create<decimal>()),
                        DeclarationRowNumber.Create(autoFixture.Create<int>()),
                        IncomingDeclarationUploadId.Create(autoFixture.Create<Guid>()),
                        incomingDeclarationState != IncomingDeclarationState.New ? incomingDeclarationState : IncomingDeclarationState.New,
                        SourceFormatPropertyBag.Create(autoFixture.Create<string>())
                    ));
                }

                await incomingDeclarationRepository.Add(incomingDeclarations);
                await incomingDeclarationRepository.SaveChanges();
                await uow.Commit(CancellationToken.None);
            }
            else
            {
                incomingDeclarations = (await incomingDeclarationRepository.GetAllByPaging(1, count, CancellationToken.None)).ToList();
            }
            
            return incomingDeclarations;
        }
    }
}