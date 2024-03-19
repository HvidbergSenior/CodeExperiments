using System.Net;
using FluentAssertions;
using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;
using Insight.Tests.End2End.TestData;
using Insight.UserAccess.Domain.User;
using Xunit;

namespace Insight.Tests.End2End.OutgoingTransactions
{
    [Collection("End2End")]
    public class OutgoingFuelTransactionsTests
    {
        private readonly WebAppFixture webAppFixture;
        private readonly Api api;
        
        public OutgoingFuelTransactionsTests(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
            api = new Api(webAppFixture);
        }

        [Fact]
        public async Task UserCanNotGetOutgoingFuelTransactions_WithoutValidToken()
        {
            var outgoingFuelTransactionsParameters = Any.OutgoingFuelTransactionsParameters();

            var errorResult =
                await Assert.ThrowsAsync<HttpRequestException>(() =>
                    api.GetOutgoingFuelTransactions(outgoingFuelTransactionsParameters.DatePeriod.StartDate, outgoingFuelTransactionsParameters.DatePeriod.EndDate,"","","", 0, 1, true, "CustomerNumber"));
            Assert.Equal(HttpStatusCode.Unauthorized, errorResult.StatusCode);
        }

        [Fact]        
        public async Task UserCanGetOutgoingFuelTransactions_WithValidToken()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);

            var amountOfSeededTransactionsToFind = 10;
            
            var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(90));
            var dateTo = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(100));
            
            var productName = ProductName.Create("HVO DIESEL");
            var customerName = BuildingBlocks.Domain.CustomerName.Create("Number One Customer");
            var companyName = CompanyName.Create("The Danish One");
            var customerId = BuildingBlocks.Domain.CustomerId.Create(Guid.NewGuid());

            var filteringParametersForSeededToFind = Insight.FuelTransactions.Domain.FilteringParameters.Create(DatePeriod.Create(dateFrom, dateTo),
                productName, customerName, companyName);

            var seededTransactionsToFind = await FuelTransactionsTestData.SeedWithOutgoingFuelTransactions(amountOfSeededTransactionsToFind, webAppFixture, filteringParametersForSeededToFind, false, customerId.Value);

            //Act
            var outgoingFuelTransactions = await api.GetOutgoingFuelTransactions(dateFrom, dateTo,   "", "", "",0, 10, true, "CustomerNumber");
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            outgoingFuelTransactions.OutgoingFuelTransactions.Should().HaveCount(amountOfSeededTransactionsToFind);
        }

        [Fact]
        public async Task UserCanFilterOutgoingFuelTransactions()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);

            var amountOfSeededTransactionsToFind = 3;
            var amountOfSeededTransactionsNotToFind = 1;
            
            var dateFrom = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(90));
            var dateTo = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(100));
            
            var productName = ProductName.Create("HVO DIESEL");
            var customerName = BuildingBlocks.Domain.CustomerName.Create("Number One Customer");
            var companyName = CompanyName.Create("The Danish One");
            var customerId = BuildingBlocks.Domain.CustomerId.Create(Guid.NewGuid());

            var filteringParametersForSeededToFind = Insight.FuelTransactions.Domain.FilteringParameters.Create(DatePeriod.Create(dateFrom, dateTo),
                productName, customerName, companyName);

            var filteringParametersForSeededToFind2 = Insight.FuelTransactions.Domain.FilteringParameters.Create(DatePeriod.Create(dateFrom, dateTo),
                ProductName.Create("B100"), BuildingBlocks.Domain.CustomerName.Create("Evil Corp"), CompanyName.Create("The Swedish One"));

            var seededTransactionsToFind = await FuelTransactionsTestData.SeedWithOutgoingFuelTransactions(amountOfSeededTransactionsToFind, webAppFixture, filteringParametersForSeededToFind, false, customerId.Value);
            var seededTransactionsNotToFInd = await FuelTransactionsTestData.SeedWithOutgoingFuelTransactions(amountOfSeededTransactionsNotToFind, webAppFixture, filteringParametersForSeededToFind2, false, customerId.Value);
      
            //Act
            var outgoingFuelTransactionsFilterProduct = await api.GetOutgoingFuelTransactions(dateFrom, dateTo, productName.Value, "", "",0, 20, true, "CustomerNumber");
            var outgoingFuelTransactionsFilterCustomer = await api.GetOutgoingFuelTransactions(dateFrom, dateTo, "",  "", customerName.Value,0, 20, true, "CustomerNumber");
            var outgoingFuelTransactionsFilterCompany = await api.GetOutgoingFuelTransactions(dateFrom, dateTo, "", companyName.Value,"", 0, 20, true, "CustomerNumber");
            var outgoingFuelTransactionsFilterProductContains = await api.GetOutgoingFuelTransactions(dateFrom, dateTo, "hvo", "","", 0, 20, true, "CustomerNumber");
            var outgoingFuelTransactionsFilterCustomerContains = await api.GetOutgoingFuelTransactions(dateFrom, dateTo, "",  "", "numb",0, 20, true, "CustomerNumber");
            var outgoingFuelTransactionsFilterCompanyContains = await api.GetOutgoingFuelTransactions(dateFrom, dateTo, "", "danish","", 0, 20, true, "CustomerNumber");

            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            outgoingFuelTransactionsFilterProduct.OutgoingFuelTransactions.Should().NotBeEmpty();
            outgoingFuelTransactionsFilterProduct.OutgoingFuelTransactions.Should().AllSatisfy(c => c.CustomerNumber.Equals(customerId));
            outgoingFuelTransactionsFilterProduct.OutgoingFuelTransactions.Count.Should().Be(amountOfSeededTransactionsToFind);
            outgoingFuelTransactionsFilterProductContains.OutgoingFuelTransactions.Count.Should().Be(amountOfSeededTransactionsToFind);

            outgoingFuelTransactionsFilterCustomer.OutgoingFuelTransactions.Should().NotBeEmpty();
            outgoingFuelTransactionsFilterCustomer.OutgoingFuelTransactions.Should().AllSatisfy(c => c.CustomerNumber.Equals(customerId));
            outgoingFuelTransactionsFilterCustomer.OutgoingFuelTransactions.Count.Should().Be(amountOfSeededTransactionsToFind);
            outgoingFuelTransactionsFilterCustomerContains.OutgoingFuelTransactions.Count.Should().Be(amountOfSeededTransactionsToFind);

            outgoingFuelTransactionsFilterCompany.OutgoingFuelTransactions.Should().NotBeEmpty();
            outgoingFuelTransactionsFilterCompany.OutgoingFuelTransactions.Should().AllSatisfy(c => c.CustomerNumber.Equals(customerId));
            outgoingFuelTransactionsFilterCompany.OutgoingFuelTransactions.Count.Should().Be(amountOfSeededTransactionsToFind);
            outgoingFuelTransactionsFilterCompanyContains.OutgoingFuelTransactions.Count.Should().Be(amountOfSeededTransactionsToFind);

        }
    }
}
