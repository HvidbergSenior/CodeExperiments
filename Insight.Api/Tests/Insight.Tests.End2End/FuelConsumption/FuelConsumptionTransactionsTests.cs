using AutoFixture;
using Insight.BuildingBlocks.Domain;
using Insight.OutgoingDeclarations.Domain;
using Insight.UserAccess.Application.RegisterUser;
using FluentAssertions;
using Insight.FuelTransactions.Domain;
using Insight.Tests.End2End.TestData;
using Xunit;
using FilteringParameters = Insight.FuelTransactions.Domain.FilteringParameters;

namespace Insight.Tests.End2End.FuelConsumption;

[Collection("End2End")]
public class FuelConsumptionTransactionsTests
{
    private readonly WebAppFixture webAppFixture;
    private readonly Api api;
    private readonly Fixture any = new();

    public FuelConsumptionTransactionsTests(WebAppFixture webAppFixture)
    {
        this.webAppFixture = webAppFixture;
        api = new Api(webAppFixture);
    }

    [Theory]
    [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, 5, 1, 5)]

    [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, 5, 1, 20)]

    [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, 5, 1, 40)]

    [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, 5, 2, 5)]

    [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, new[]{ProductNameEnumeration.Petrol}, 5, 1, 1000)]

    [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel}, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, 5, 1, 1000)]

    [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel}, new[]{ProductNameEnumeration.Petrol}, 5, 1, 1000)]

    [InlineData(5, 20, 21, 22, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, 5, 1, 1000)]

    [InlineData(5, 5, 4, 6, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, 5, 1, 1000)]

    [InlineData(5, 5, 2, 3, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, new[]{ProductNameEnumeration.Diesel, ProductNameEnumeration.Petrol}, 5, 1, 1000)]
    public async Task FuelConsumptionTransaction_UserCanGetCorrectNumberOfTransactions(int dayStartGen, int dayEndGen, int dayStartQuery, int dayEndQuery, ProductNameEnumeration[] productNamesGen, ProductNameEnumeration[] productNamesQuery, int transactionsPerProduct, int pageStart, int pageSize)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

        //Arrange
        var dateStartGen = DateOnly.FromDateTime(new DateTime(2022, 1, dayStartGen));
        var dateEndGen = DateOnly.FromDateTime(new DateTime(2022, 1, dayEndGen));
        var dateStartQuery = DateOnly.FromDateTime(new DateTime(2022, 1, dayStartQuery));
        var dateEndQuery = DateOnly.FromDateTime(new DateTime(2022, 1, dayEndQuery));
        var datePeriod = DatePeriod.Create(dateStartGen, dateEndGen);
        var seededCustomer = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
            customerNumber: any.Create<string>(), customerBillToNumber: "",
            useExistingDeclarationsIfAny: false);
        var customerId = seededCustomer.FirstOrDefault()!.CustomerId.Value;
        var customerName = CustomerName.Create("FuelTransaction_Customer");
        var productNamesQueryDto = productNamesQuery.ToArray();
        foreach (var productNameGen in productNamesGen)
        {
            var filteringParameters =
                FilteringParameters.Create(datePeriod, ProductName.Create(ProductNameEnumerationExtensions.ProductNameEnumerationToTranslatedString(productNameGen)), customerName, CompanyName.Empty());
            await FuelTransactionsTestData.SeedWithOutgoingFuelTransactions(transactionsPerProduct, webAppFixture, filteringParameters,
                false, customerId);
        }
        var tokenCommonAdmin =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
        api.Client.SetToken(tokenCommonAdmin.AccessToken);
        var (userName, password) = await api.RegisterUser(new List<RegisterUserCustomerPermissionDto>()
        {
            new()
            {
                CustomerId = customerId,
                CustomerNumber = any.Create<string>(),
                CustomerName = customerName.Value,
                Permissions = new List<CustomerPermission>()
                {
                    CustomerPermission.FuelConsumption
                }
            }
        },
            role: UserAccess.Domain.User.UserRole.User);
        var token = await api.LoginUser(userName, password);

        // Set the token on the client. (Remember to remove it again!)
        api.Client.SetToken(token.AccessToken);

        //Act
        await api.GetFuelConsumptionTransactions(dateStartQuery, dateEndQuery,   productNamesQueryDto, new Guid[] {customerId}, pageStart, pageSize, true, "FuelTransactionDate");
        // Removes the token again, due to shared client.
        api.Client.RemoveToken();

        //Assert
    }

    // [Theory]
    // [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel}, new[]{ProductNameEnumeration.Diesel}, 10, 1, 1000, CustomerPermission.FuelConsumption, false, false, false, 10 )]
    //
    // [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel}, new[]{ProductNameEnumeration.Diesel}, 10, 1, 1000, CustomerPermission.SustainabilityReport, false, false, true, 0 )]
    //
    // [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel}, new[]{ProductNameEnumeration.Diesel}, 10, 1, 1000, CustomerPermission.FuelConsumption, true, false, true, 0 )]
    //
    // [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel}, new[]{ProductNameEnumeration.Diesel}, 10, 1, 1000, CustomerPermission.FuelConsumption, false, true, true, 10 )]
    //
    // [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel}, new[]{ProductNameEnumeration.Diesel}, 10, 1, 1000, CustomerPermission.FuelConsumption, false, false, false, 10, UserAccess.Domain.User.UserRole.Admin)]
    //
    // [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel}, new[]{ProductNameEnumeration.Diesel}, 10, 1, 1000, CustomerPermission.SustainabilityReport, false, false, false, 10, UserAccess.Domain.User.UserRole.Admin)]
    //
    // [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel}, new[]{ProductNameEnumeration.Diesel}, 10, 1, 1000, CustomerPermission.FuelConsumption, true, false, false, 10, UserAccess.Domain.User.UserRole.Admin)]
    //
    // [InlineData(5, 20, 4, 21, new[]{ProductNameEnumeration.Diesel}, new[]{ProductNameEnumeration.Diesel}, 10, 1, 1000, CustomerPermission.FuelConsumption, false, true, false, 0, UserAccess.Domain.User.UserRole.Admin)]
    // public async Task FuelConsumptionTransaction_CustomerPermissionsAreHandled(int dayStartGen, int dayEndGen, int dayStartQuery, int dayEndQuery, ProductNameEnumeration[] productNamesGen, ProductNameEnumeration[] productNamesQuery, int transactionsPerProduct, int pageStart, int pageSize, CustomerPermission customerPermissionUser, bool userHasOwnCustomerId, bool askForRandomCustomerId, bool shouldCastException, int expectedTotalAmountTransactions, UserAccess.Domain.User.UserRole userRole = UserAccess.Domain.User.UserRole.User)
    // {
    //     Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
    //     Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
    //
    //     //Arrange
    //     var dateStartGen = DateOnly.FromDateTime(new DateTime(2022, 1, dayStartGen));
    //     var dateEndGen = DateOnly.FromDateTime(new DateTime(2022, 1, dayEndGen));
    //     var dateStartQuery = DateOnly.FromDateTime(new DateTime(2022, 1, dayStartQuery));
    //     var dateEndQuery = DateOnly.FromDateTime(new DateTime(2022, 1, dayEndQuery));
    //     var datePeriod = DatePeriod.Create(dateStartGen, dateEndGen);
    //     var seededCustomer = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
    //         customerNumber: any.Create<string>(), customerBillToNumber: "",
    //         useExistingDeclarationsIfAny: false);
    //     var customerId = seededCustomer.FirstOrDefault()!.CustomerId.Value;
    //     var customerName = CustomerName.Create("FuelTransaction_Customer");
    //     var productNamesQueryDto = productNamesQuery.ToArray();
    //     var exceptionCasted = false;
    //     foreach (var productNameGen in productNamesGen)
    //     {
    //         var filteringParameters =
    //             FilteringParameters.Create(datePeriod, ProductName.Create(ProductNameEnumerationExtensions.ProductNameEnumerationToTranslatedString(productNameGen)), customerName, CompanyName.Empty());
    //         await FuelTransactionsTestData.SeedWithOutgoingFuelTransactions(transactionsPerProduct, webAppFixture, filteringParameters,
    //             false, customerId);
    //     }
    //
    //     var customerPermissions = new List<RegisterUserCustomerPermissionDto>()
    //     {
    //         new()
    //         {
    //             CustomerId = userHasOwnCustomerId ? Guid.NewGuid() : customerId,
    //             CustomerNumber = any.Create<string>(),
    //             CustomerName = customerName.Value,
    //             Permissions = new List<CustomerPermission>()
    //             {
    //                 customerPermissionUser
    //             }
    //         }
    //     };
    //     var tokenCommonAdmin =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
    //     api.Client.SetToken(tokenCommonAdmin.AccessToken);
    //     var (userName, password) = await api.RegisterUser(customerPermissions,
    //         role: userRole);
    //     var token = await api.LoginUser(userName, password);
    //     var totalAmountTransactions = 0;
    //
    //     // Set the token on the client. (Remember to remove it again!)
    //     api.Client.SetToken(token.AccessToken);
    //
    //     //Act
    //     try
    //     {
    //         await api.GetFuelConsumptionTransactions(dateStartQuery, dateEndQuery,
    //             productNamesQueryDto, new Guid[] { askForRandomCustomerId ? Guid.NewGuid() : customerId }, pageStart, pageSize, true, "FuelTransactionDate");
    //     }
    //     catch (Exception)
    //     {
    //         exceptionCasted = true;
    //     }
    //     // Removes the token again, due to shared client.
    //     api.Client.RemoveToken();
    //     if (shouldCastException)
    //     {
    //         Assert.True(exceptionCasted);
    //     }
    //     else
    //     {
    //         Assert.False(exceptionCasted);
    //         Assert.Equal(expectedTotalAmountTransactions, totalAmountTransactions);
    //     }
    //}
}
