using FluentAssertions;
using Insight.BuildingBlocks.Domain;
using Insight.OutgoingDeclarations.Domain;
using Insight.Tests.End2End.TestData;
using Insight.UserAccess.Domain.User;
using Xunit;
using Xunit.Abstractions;

namespace Insight.Tests.End2End.Declarations
{
    [Collection("End2End")]
    public class OutgoingDeclarationTests
    {
        private readonly WebAppFixture webAppFixture;
        private readonly Api api;

        public OutgoingDeclarationTests(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
            api = new Api(webAppFixture);
        }


        [Fact]
        public async Task UserCanGetOutgoingDeclarationById()
        {
            //Arrange
            var token = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);

            var seededIncomingDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(2, IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(), webAppFixture, useExistingDeclarationsIfAny: false);


            var pairingsList = seededIncomingDeclarations.Select(incomingDeclaration => IncomingDeclarationIdPairing.Create(IncomingDeclarationId.Create(incomingDeclaration.IncomingDeclarationId.Value), Quantity.Create(incomingDeclaration.Quantity.Value), BatchId.Create(11111111))).ToList();

            var seededDeclarations = await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(1, Insight.OutgoingDeclarations.Domain.FilteringParameters.Empty(), webAppFixture, pairingsList);
            var seededDeclaration = seededDeclarations.First();

            var outgoingDeclarationId = seededDeclaration.OutgoingDeclarationId;
            //Act
            var outgoingDeclarationByIdResponse = await api.GetOutgoingDeclarationById(outgoingDeclarationId);

            api.Client.RemoveToken();

            //Assert
            outgoingDeclarationByIdResponse.OutgoingDeclarationByIdResponse.Should().NotBeNull();
            outgoingDeclarationByIdResponse.OutgoingDeclarationByIdResponse.GetOutgoingDeclarationIncomingDeclarationResponse.Count.Should().Be(seededIncomingDeclarations.Count());
        }

        [Fact]
        public async Task UserCanGetOutgoingDeclarationsByCustomerId()
        {
            //Arrange
            var token = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            var seededIncomingDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(2, IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(), webAppFixture);
            var pairingsList = seededIncomingDeclarations.Select(incomingDeclaration => IncomingDeclarationIdPairing.Create(IncomingDeclarationId.Create(incomingDeclaration.IncomingDeclarationId.Value), Quantity.Create(incomingDeclaration.Quantity.Value), BatchId.Empty())).ToList();

            var seededDeclarations = await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(2, Insight.OutgoingDeclarations.Domain.FilteringParameters.Empty(), webAppFixture, pairingsList);
            var seededDeclaration = seededDeclarations.First();
            var customerNumber = seededDeclaration.CustomerDetails.CustomerNumber.Value;

            //Act
            var outgoingDeclarationByCustomerIdResponse = await api.GetOutgoingDeclarationsByCustomerId(customerNumber);
            api.Client.RemoveToken();

            //Assert
            outgoingDeclarationByCustomerIdResponse.OutgoingDeclarationByCustomerIdResponse.Count().Should().Be(1);
        }

        [Fact]
        public async Task UserCanGetOutgoingDeclarations()
        {
            //Arrange
            var token = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            var seededIncomingDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(2, IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(), webAppFixture);
            var pairingsList = seededIncomingDeclarations.Select(incomingDeclaration => IncomingDeclarationIdPairing.Create(IncomingDeclarationId.Create(incomingDeclaration.IncomingDeclarationId.Value), Quantity.Create(incomingDeclaration.Quantity.Value), BatchId.Empty())).ToList();

            var seededDeclarations = await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(2, Insight.OutgoingDeclarations.Domain.FilteringParameters.Empty(), webAppFixture, pairingsList);

            //Act
            var outgoingDeclarationResponse = await api.GetOutgoingDeclarations();
            api.Client.RemoveToken();

            //Assert
            outgoingDeclarationResponse.OutgoingDeclarationsResponses.Count().Should().BePositive();
        }
        [Fact]
        public async Task UserCanGetOutgoingDeclarationsByPageAndPageSize()
        {
            //Arrange
            var token = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            var filteringParameters = Any.OutgoingFilteringParameters();
            var seededIncomingDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(2, IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(), webAppFixture);
            var pairingsList = seededIncomingDeclarations.Select(incomingDeclaration => IncomingDeclarationIdPairing.Create(IncomingDeclarationId.Create(incomingDeclaration.IncomingDeclarationId.Value), Quantity.Create(incomingDeclaration.Quantity.Value), BatchId.Empty())).ToList();

            const int seededDeclarationsToFind = 10;
            const int seededDeclarationsToNotFind = 10;
            await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(seededDeclarationsToFind, filteringParameters, webAppFixture, pairingsList, false);
            await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(seededDeclarationsToNotFind, Insight.OutgoingDeclarations.Domain.FilteringParameters.Empty(), webAppFixture, pairingsList, false);

            //Act
            var declarationsByPageAndPageSize = await api.GetOutgoingDeclarationsByPageAndPageSize(filteringParameters.DatePeriod.StartDate, filteringParameters.DatePeriod.EndDate, filteringParameters.Product.Value, filteringParameters.Company.Value, filteringParameters.CustomerName.Value, 1, 1, true, "Product");
            api.Client.RemoveToken();

            //Assert
            declarationsByPageAndPageSize.OutgoingDeclarationsByPageAndPageSizeResponse.Should().NotBeNull();
        }
        [Fact]
        public async Task UserCanGetOutgoingDeclarationsByPageAndPageSizeWithFiltering()
        {
            //Arrange
            var token = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            var customerName = CustomerName.Create("GreatCustomerName");
            var product = Product.Create("GreatProduct");
            var company = Company.Create("GreatCompany");
            var dateOfCreation = DateOfCreation.Create(new DateOnly(2020, 06, 14));
            var datePeriod = DatePeriod.Create(dateOfCreation.Value, DateOnly.MaxValue);
            const string orderProperty = "DateOfCreation";

            var filteringParametersForSeededDeclarations = Insight.OutgoingDeclarations.Domain.FilteringParameters.Create(datePeriod, product, company, customerName);
            var seededIncomingDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(2, IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(), webAppFixture);
            var pairingsList = seededIncomingDeclarations.Select(incomingDeclaration => IncomingDeclarationIdPairing.Create(IncomingDeclarationId.Create(incomingDeclaration.IncomingDeclarationId.Value), Quantity.Create(incomingDeclaration.Quantity.Value), BatchId.Empty())).ToList();

            const int seededDeclarationsToFind = 10;
            const int seededDeclarationsToNotFind = 10;

            var seededDeclarationsWithFilteringProperties = await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(seededDeclarationsToFind, filteringParametersForSeededDeclarations, webAppFixture, pairingsList, false);
            var seededDeclarationsWithoutFilteringProperties = await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(seededDeclarationsToNotFind, Insight.OutgoingDeclarations.Domain.FilteringParameters.Empty(), webAppFixture, pairingsList, false);

            //Act
            var declarationsByPageAndPageSizeDatePeriod = await api.GetOutgoingDeclarationsByPageAndPageSize(filteringParametersForSeededDeclarations.DatePeriod.StartDate, filteringParametersForSeededDeclarations.DatePeriod.EndDate, filteringParametersForSeededDeclarations.Product.Value, filteringParametersForSeededDeclarations.Company.Value, filteringParametersForSeededDeclarations.CustomerName.Value, 1, seededDeclarationsToFind, true, orderProperty);
            api.Client.RemoveToken();

            //Assert
            declarationsByPageAndPageSizeDatePeriod.OutgoingDeclarationsByPageAndPageSizeResponse.Should().NotBeNull();
            declarationsByPageAndPageSizeDatePeriod.OutgoingDeclarationsByPageAndPageSizeResponse.Count.Should().Be(seededDeclarationsToFind);

        }
        [Fact]
        public async Task UserCanGetOutgoingDeclarationsByPageAndPageSizeWithFilteringDateOfCreation()
        {
            //Arrange
            var token = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);

            var dateOfCreationForDeclarationsToFind = DateOfCreation.Create(new DateOnly(2020, 06, 14));
            var dateOfCreationForDeclarationsNotToFind = DateOfCreation.Create(new DateOnly(2010, 06, 14));

            var datePeriodDateOfCreationForDeclarationsToFind = DatePeriod.Create(dateOfCreationForDeclarationsToFind.Value, DateOnly.MaxValue);
            var datePeriodDateOfCreationForDeclarationsNotToFind = DatePeriod.Create(dateOfCreationForDeclarationsNotToFind.Value, DateOnly.MaxValue);

            var filteringParametersForSeededDeclarationsToFind = Insight.OutgoingDeclarations.Domain.FilteringParameters.Create(datePeriodDateOfCreationForDeclarationsToFind, Product.Empty(), Company.Empty(), CustomerName.Empty());
            var filteringParametersForSeededDeclarationsNotToFind = Insight.OutgoingDeclarations.Domain.FilteringParameters.Create(datePeriodDateOfCreationForDeclarationsNotToFind, Product.Empty(), Company.Empty(), CustomerName.Empty());

            var seededIncomingDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(2, IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(), webAppFixture);
            var pairingsList = seededIncomingDeclarations.Select(incomingDeclaration => IncomingDeclarationIdPairing.Create(IncomingDeclarationId.Create(incomingDeclaration.IncomingDeclarationId.Value), Quantity.Create(incomingDeclaration.Quantity.Value), BatchId.Empty())).ToList();

            const int seededDeclarationsToFind = 3;
            const int seededDeclarationsToNotFind = 1;
            const string orderByProperty = "DateOfCreation";
            var seededDeclarationsWithFilteringProperties = await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(seededDeclarationsToFind, filteringParametersForSeededDeclarationsToFind, webAppFixture, pairingsList, false);
            var seededDeclarationsWithoutFilteringProperties = await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(seededDeclarationsToNotFind, filteringParametersForSeededDeclarationsNotToFind, webAppFixture, pairingsList, false);

            //Act
            var filterDates = DatePeriod.Create(new DateOnly(2019, 01, 01), new DateOnly(2021, 01, 01));
            var filteringParametersDatePeriod = Insight.OutgoingDeclarations.Domain.FilteringParameters.Create(filterDates, Product.Empty(), Company.Empty(), CustomerName.Empty());

            var declarationsByPageAndPageSizeDatePeriod = await api.GetOutgoingDeclarationsByPageAndPageSize(filteringParametersDatePeriod.DatePeriod.StartDate, filteringParametersDatePeriod.DatePeriod.EndDate, filteringParametersDatePeriod.Product.Value, filteringParametersDatePeriod.Company.Value, filteringParametersDatePeriod.CustomerName.Value, 1, seededDeclarationsToFind, true, orderByProperty);

            api.Client.RemoveToken();

            //Assert
            declarationsByPageAndPageSizeDatePeriod.OutgoingDeclarationsByPageAndPageSizeResponse.Should().NotBeNull();
            declarationsByPageAndPageSizeDatePeriod.OutgoingDeclarationsByPageAndPageSizeResponse.Count.Should().Be(seededDeclarationsToFind);

        }
        [Fact]
        public async Task UserCanGetOutgoingDeclarationsByPageAndPageSizeWithFilteringProduct()
        {
            //Arrange
            var token = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            var product = Product.Create("GreatProduct");

            var filteringParametersForSeededDeclarations = Insight.OutgoingDeclarations.Domain.FilteringParameters.Create(DatePeriod.Empty(), product, Company.Empty(), CustomerName.Empty());
            var seededIncomingDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(2, IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(), webAppFixture);
            var pairingsList = seededIncomingDeclarations.Select(incomingDeclaration => IncomingDeclarationIdPairing.Create(IncomingDeclarationId.Create(incomingDeclaration.IncomingDeclarationId.Value), Quantity.Create(incomingDeclaration.Quantity.Value), BatchId.Empty())).ToList();

            const int seededDeclarationsToFind = 3;
            const int seededDeclarationsToNotFind = 1;

            var seededDeclarationsWithFilteringProperties = await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(seededDeclarationsToFind, filteringParametersForSeededDeclarations, webAppFixture, pairingsList, false);
            var seededDeclarationsWithoutFilteringProperties = await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(seededDeclarationsToNotFind, Insight.OutgoingDeclarations.Domain.FilteringParameters.Empty(), webAppFixture, pairingsList, false);

            //Act
            var declarationsByPageAndPageSizeProduct = await api.GetOutgoingDeclarationsByPageAndPageSize(filteringParametersForSeededDeclarations.DatePeriod.StartDate, filteringParametersForSeededDeclarations.DatePeriod.EndDate, "reatp", filteringParametersForSeededDeclarations.Company.Value, filteringParametersForSeededDeclarations.CustomerName.Value, 1, seededDeclarationsToFind, true, "Product");

            api.Client.RemoveToken();

            //Assert
            declarationsByPageAndPageSizeProduct.OutgoingDeclarationsByPageAndPageSizeResponse.Should().NotBeNull();
            declarationsByPageAndPageSizeProduct.OutgoingDeclarationsByPageAndPageSizeResponse.Count.Should().Be(seededDeclarationsToFind);
            declarationsByPageAndPageSizeProduct.OutgoingDeclarationsByPageAndPageSizeResponse[0].IncomingDeclarationIds.Count.Should().Be(2);

        }

        [Fact]
        public async Task UserCanGetOutgoingDeclarationsByPageAndPageSizeWithFilteringCustomerName()
        {
            //Arrange
            var token = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            var customerName = CustomerName.Create("GreatCustomerName");
            var customerName2 = CustomerName.Create("GreatCustomerName2");

            var seededIncomingDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(2, IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(), webAppFixture);
            var pairingsList = seededIncomingDeclarations.Select(incomingDeclaration => IncomingDeclarationIdPairing.Create(IncomingDeclarationId.Create(incomingDeclaration.IncomingDeclarationId.Value), Quantity.Create(incomingDeclaration.Quantity.Value), BatchId.Empty())).ToList();

            var filteringParametersForSeededDeclarations = Insight.OutgoingDeclarations.Domain.FilteringParameters.Create(DatePeriod.Empty(), Product.Empty(), Company.Empty(), customerName);
            var filteringParametersForSeededDeclarations2 = Insight.OutgoingDeclarations.Domain.FilteringParameters.Create(DatePeriod.Empty(), Product.Empty(), Company.Empty(), customerName2);

            const int seededDeclarationsToFind = 3;
            const int seededDeclarationsToNotFind = 1;

            var seededDeclarationsWithFilteringProperties = await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(seededDeclarationsToFind, filteringParametersForSeededDeclarations, webAppFixture, pairingsList, false);
            var seededDeclarationsWithFilteringProperties2 = await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(seededDeclarationsToFind, filteringParametersForSeededDeclarations2, webAppFixture, pairingsList, false);

            var seededDeclarationsWithoutFilteringProperties = await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(seededDeclarationsToNotFind, Insight.OutgoingDeclarations.Domain.FilteringParameters.Empty(), webAppFixture, pairingsList, false);

            //Act
            var declarationsByPageAndPageSizeSupplier = await api.GetOutgoingDeclarationsByPageAndPageSize(filteringParametersForSeededDeclarations.DatePeriod.StartDate, filteringParametersForSeededDeclarations.DatePeriod.EndDate, filteringParametersForSeededDeclarations.Product.Value, filteringParametersForSeededDeclarations.Company.Value, "ustom", 1, seededDeclarationsToFind, false, "CustomerName");

            api.Client.RemoveToken();

            //Assert
            declarationsByPageAndPageSizeSupplier.OutgoingDeclarationsByPageAndPageSizeResponse.Should().NotBeNull();
            declarationsByPageAndPageSizeSupplier.OutgoingDeclarationsByPageAndPageSizeResponse.Count.Should().Be(seededDeclarationsToFind);
            declarationsByPageAndPageSizeSupplier.OutgoingDeclarationsByPageAndPageSizeResponse[0].IncomingDeclarationIds.Count.Should().Be(2);
        }
    }
}