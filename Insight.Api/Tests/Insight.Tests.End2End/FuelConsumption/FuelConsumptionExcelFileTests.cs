using AutoFixture;
using Ganss.Excel;
using Insight.BuildingBlocks.Domain;
using Insight.OutgoingDeclarations.Domain;
using Insight.UserAccess.Application.RegisterUser;
using Insight.FuelTransactions.Domain;
using Insight.Tests.End2End.TestData;
using Insight.UserAccess.Domain.User;
using Xunit;
using FilteringParameters = Insight.FuelTransactions.Domain.FilteringParameters;

namespace Insight.Tests.End2End.FuelConsumption;


[Collection("End2End")]
public class FuelConsumptionExcelFileTests
{
    private readonly WebAppFixture webAppFixture;
    private readonly Api api;
    private static readonly Fixture any = new();

    public FuelConsumptionExcelFileTests(WebAppFixture webAppFixture)
    {
        this.webAppFixture = webAppFixture;
        api = new Api(webAppFixture);
    }

    [Theory]
    [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, 10)]
    public async Task FuelConsumptionExcelFile_UserCanGetSensibleValues(int dayStartGen, int dayEndGen, int dayStartQuery, int dayEndQuery, ProductNameEnumeration[] productNamesGen, ProductNameEnumeration[] productNamesQuery, int transactionsPerProduct)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

        //Arrange
        var dateStartGen = DateOnly.FromDateTime(new DateTime(2022, 1, dayStartGen));
        var dateEndGen = DateOnly.FromDateTime(new DateTime(2022, 1, dayEndGen));
        var dateStartQuery = DateOnly.FromDateTime(new DateTime(2022, 1, dayStartQuery));
        var dateEndQuery = DateOnly.FromDateTime(new DateTime(2022, 1, dayEndQuery));
        var datePeriod = DatePeriod.Create(dateStartGen, dateEndGen);
        var customerName = CustomerName.Create("FuelTransaction_Customer");
        var productNamesQueryDto = productNamesQuery.ToArray();
        var tokenCommonAdmin =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
        api.Client.SetToken(tokenCommonAdmin.AccessToken);
        var seededCustomer = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
            customerNumber: any.Create<string>(), customerBillToNumber: "",
            useExistingDeclarationsIfAny: false);
        foreach (var productNameGen in productNamesGen)
        {
            var filteringParameters =
                FilteringParameters.Create(datePeriod, ProductName.Create(ProductNameEnumerationExtensions.ProductNameEnumerationToTranslatedString(productNameGen)), customerName, CompanyName.Empty());
            await FuelTransactionsTestData.SeedWithOutgoingFuelTransactions(transactionsPerProduct, webAppFixture, filteringParameters,
                false, seededCustomer.FirstOrDefault()!.CustomerId.Value);
        }
        var (userName, password) = await api.RegisterUser(new List<RegisterUserCustomerPermissionDto>()
        {
            new()
            {
                CustomerId = seededCustomer.FirstOrDefault()!.CustomerId.Value,
                CustomerNumber = any.Create<string>(),
                CustomerName = seededCustomer.FirstOrDefault()!.CustomerDetails.CustomerName.Value,
                Permissions = new List<CustomerPermission>()
                {
                    CustomerPermission.FuelConsumption
                }
            }
        },
            role: UserRole.User);
        var token = await api.LoginUser(userName, password);

        // Set the token on the client. (Remember to remove it again!)
        api.Client.SetToken(token.AccessToken);

        //Act
        await api.GetFuelConsumptionExcelFile(dateStartQuery, dateEndQuery, productNamesQueryDto, new Guid[] {seededCustomer.FirstOrDefault()!.CustomerId.Value});
        // Removes the token again, due to shared client.
        api.Client.RemoveToken();

        //Assert
    }
}