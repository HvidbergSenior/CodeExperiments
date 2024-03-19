using AutoFixture;
using FluentAssertions;
using Insight.BuildingBlocks.Domain;
using Insight.OutgoingDeclarations.Domain;
using Insight.UserAccess.Application.RegisterUser;
using Insight.UserAccess.Domain.User;
using Insight.FuelTransactions.Domain;
using Insight.Tests.End2End.TestData;
using Xunit;
using FilteringParameters = Insight.FuelTransactions.Domain.FilteringParameters;
using Quantity = Insight.OutgoingDeclarations.Domain.Quantity;

namespace Insight.Tests.End2End.SustainabilityReports;

[Collection("End2End")]
public class SustainabilityReportTests
{
    private readonly WebAppFixture webAppFixture;
    private readonly Api api;
    private readonly Fixture any = new();

    public SustainabilityReportTests(WebAppFixture webAppFixture)
    {
        this.webAppFixture = webAppFixture;
        api = new Api(webAppFixture);
    }

    [Fact]
    public async Task SustainabilityReport_IsCalled()
    {
        //int dayStartGen, int dayEndGen, int dayStartQuery, int dayEndQuery, ProductNameEnumeration[] productNamesGen, ProductNameEnumeration[] productNamesQuery, int transactionsPerProduct, CustomerPermission customerPermissionUser, bool userHasOwnCustomerId, bool askForRandomCustomerId, bool shouldCastException, int expectedSeries, bool expectedSomeData, UserAccess.Domain.User.UserRole userRole = UserAccess.Domain.User.UserRole.User
        //    [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel}, new[]{ProductNameEnumeration.Diesel}, 10, CustomerPermission.FuelConsumption, false, false, false, 1, true)]

        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

        //Arrange
        var dateStartGen = DateOnly.FromDateTime(new DateTime(2010, 1, 5));
        var dateEndGen = DateOnly.FromDateTime(new DateTime(2024, 1, 20));
        var dateStartQuery = DateOnly.FromDateTime(new DateTime(2010, 1, 4));
        var dateEndQuery = DateOnly.FromDateTime(new DateTime(2024, 1, 21));
        var datePeriod = DatePeriod.Create(dateStartGen, dateEndGen);
        var tokenCommonAdmin = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
        api.Client.SetToken(tokenCommonAdmin.AccessToken);
        var seededCustomer = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
            customerNumber: any.Create<string>(), customerBillToNumber: "",
            useExistingDeclarationsIfAny: false);
        var customerId = seededCustomer.FirstOrDefault()!.CustomerId.Value;
        var customerName = CustomerName.Create("FuelTransaction_Customer");
        var productNamesQueryDto = new[] { ProductNameEnumeration.Diesel, ProductNameEnumeration.Hvo100 }
            .ToArray();
        foreach (var productNameGen in new[] { ProductNameEnumeration.Diesel })
        {
            var filteringParameters =
                FilteringParameters.Create(datePeriod,
                    ProductName.Create(
                        ProductNameEnumerationExtensions.ProductNameEnumerationToTranslatedString(productNameGen)),
                    customerName, CompanyName.Empty());
            await FuelTransactionsTestData.SeedWithOutgoingFuelTransactions(10, webAppFixture, filteringParameters,
                false, customerId);
        }

        var incomingDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(8,
            IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(), webAppFixture);


        var pairings = new List<IncomingDeclarationIdPairing>()
        {
            IncomingDeclarationIdPairing.Create(
                IncomingDeclarationId.Create(incomingDeclarations.First().IncomingDeclarationId.Value),
                Quantity.Create(1000),
                BatchId.Create(333333333)),
            IncomingDeclarationIdPairing.Create(
                IncomingDeclarationId.Create(incomingDeclarations.Last().IncomingDeclarationId.Value),
                Quantity.Create(3000),
                BatchId.Create(22222222))
        };
        var outgoingFilteringParameters = Insight.OutgoingDeclarations.Domain.FilteringParameters.Create(
            DatePeriod.Create(dateStartGen, dateEndGen), Product.Create("DIESEL"),
            Company.Create("CompanyName"), CustomerName.Create("CustomerName"));
        var outgoingFilteringParameters2 = Insight.OutgoingDeclarations.Domain.FilteringParameters.Create(
            DatePeriod.Create(dateStartGen, dateEndGen), Product.Create("Hvo100"),
            Company.Create("CompanyName"), CustomerName.Create("CustomerName"));
        var outgoingDeclarations =
            await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(2, outgoingFilteringParameters,
                webAppFixture, pairings, false);
        var outgoingDeclarations2 =
            await OutgoingDeclarationsTestData.SeedWithOutgoingDeclaration(2, outgoingFilteringParameters2,
                webAppFixture, pairings, false);


        var customerPermissions = new List<RegisterUserCustomerPermissionDto>()
        {
            new()
            {
                CustomerId = customerId,
                CustomerNumber = any.Create<string>(),
                CustomerName = customerName.Value,
                Permissions = new List<CustomerPermission>()
                {
                    CustomerPermission.Admin
                }
            }
        };
        var (userName, password) = await api.RegisterUser(customerPermissions,
            role: UserRole.Admin);
        var token = await api.LoginUser(userName, password);

        // Set the token on the client. (Remember to remove it again!)
        api.Client.SetToken(token.AccessToken);

        //Act

        await api.GetSustainabilityReport(dateStartQuery, dateEndQuery,
            productNamesQueryDto, new Guid[] { customerId });

        // Removes the token again, due to shared client.
        api.Client.RemoveToken();
    }
}