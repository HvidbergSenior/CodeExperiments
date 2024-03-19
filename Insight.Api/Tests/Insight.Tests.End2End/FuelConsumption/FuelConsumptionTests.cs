using AutoFixture;
using Insight.BuildingBlocks.Domain;
using Insight.OutgoingDeclarations.Domain;
using Insight.UserAccess.Application.RegisterUser;
using Insight.UserAccess.Domain.User;
using Insight.FuelTransactions.Domain;
using Insight.Tests.End2End.TestData;
using Xunit;
using FilteringParameters = Insight.FuelTransactions.Domain.FilteringParameters;
using Xunit.Abstractions;

namespace Insight.Tests.End2End.FuelConsumption;

[Collection("End2End")]
public class FuelConsumptionTests
{
    private readonly WebAppFixture webAppFixture;
    private readonly Api api;
    private readonly Fixture any = new();

    public FuelConsumptionTests(WebAppFixture webAppFixture)
    {
        this.webAppFixture = webAppFixture;
        api = new Api(webAppFixture);
    }

    
    [Fact]
    public async Task FuelConsumption_IsCalled()
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

        //Arrange
        var dateStartGen = DateOnly.FromDateTime(new DateTime(2022, 1, 5));
        var dateEndGen = DateOnly.FromDateTime(new DateTime(2022, 1, 20));
        var dateStartQuery = DateOnly.FromDateTime(new DateTime(2022, 1, 4));
        var dateEndQuery = DateOnly.FromDateTime(new DateTime(2022, 1, 21));
        var datePeriod = DatePeriod.Create(dateStartGen, dateEndGen);
        var tokenCommonAdmin =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
        api.Client.SetToken(tokenCommonAdmin.AccessToken);
        var seededCustomer = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
            customerNumber: any.Create<string>(), customerBillToNumber: "",
            useExistingDeclarationsIfAny: false);
        var customerId = seededCustomer.FirstOrDefault()!.CustomerId.Value;
        var customerName = CustomerName.Create("FuelTransaction_Customer");
        var productNamesQueryDto = new[]{ProductNameEnumeration.Diesel, 
            ProductNameEnumeration.Petrol}.ToArray();
        //var exceptionCasted = false;
        foreach (var productNameGen in new[]{ProductNameEnumeration.Diesel, 
                     ProductNameEnumeration.Petrol})
        {
            var filteringParameters =
                FilteringParameters.Create(datePeriod, ProductName.Create(ProductNameEnumerationExtensions.ProductNameEnumerationToTranslatedString(productNameGen)), customerName, CompanyName.Empty());
            await FuelTransactionsTestData.SeedWithOutgoingFuelTransactions(5, webAppFixture, filteringParameters,
                false, customerId);
        }

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
        
        var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
        // Set the token on the client. (Remember to remove it again!)
        api.Client.SetToken(token.AccessToken);
        await api.RegisterUser(role: UserRole.Admin);
     

        // Set the token on the client. (Remember to remove it again!)
        api.Client.SetToken(token.AccessToken);

        //Act
            await api.GetFuelConsumption(dateStartQuery, dateEndQuery, productNamesQueryDto, new Guid[] { customerId});
        
    }
}
