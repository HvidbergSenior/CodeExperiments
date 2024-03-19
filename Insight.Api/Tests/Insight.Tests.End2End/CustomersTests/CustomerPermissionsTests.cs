using AutoFixture;
using FluentAssertions;
using Insight.BuildingBlocks.Domain;
using Insight.Tests.End2End.TestData;
using Insight.UserAccess.Application.RegisterUser;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Xunit;

namespace Insight.Tests.End2End.CustomersTests
{
    [Collection("End2EndCustomers")]
    //Note: We put them on their own collection and run them on the same class so they run alone and one at a time to prevent conflicts with tests where user is Admin and can see ALL customers
    public class CustomerPermissionTests
    {
        private readonly Api api;
        private readonly WebAppFixture webAppFixture;
        private readonly Fixture autoFixture;

        public CustomerPermissionTests(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
            api = new Api(webAppFixture);
            autoFixture = new();
        }

        [Fact]
        public async Task AvailableUserPermissions_GetPermissionsAsAdmin()
        {
            //Arrange
            await CustomerTestData.DeleteAllCustomers(webAppFixture);
            var customerNumber = "1" + "_" + autoFixture.Create<string>();
            var customerNumber2 = "2" + "_" + autoFixture.Create<string>();
            var customerNumber21 = "21" + "_" + autoFixture.Create<string>();
            var permissionsUser2 = new List<CustomerPermission>()
            {
                CustomerPermission.FuelConsumption
            };
            var seededCustomers1 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: customerNumber, customerBillToNumber: "",
                useExistingDeclarationsIfAny: false);
            var seededCustomers2 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: customerNumber2, customerBillToNumber: "",
                useExistingDeclarationsIfAny: false);
            var seededCustomers21 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: customerNumber21, customerBillToNumber: seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value,
                useExistingDeclarationsIfAny: false);

            //Arrange
            var tokenCommonAdmin =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            api.Client.SetToken(tokenCommonAdmin.AccessToken);

            //Act

            var permissionsReceived = await api.GetAvailableCustomerPermissions();
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            permissionsReceived.CustomerNodes.Should().NotBeEmpty();
        }

        [Fact]
        public async Task PossibleCustomerPermissions_AdminHasAccessToAllCustomers()
        {
            //Arrange
            await CustomerTestData.DeleteAllCustomers(webAppFixture);
            var seededCustomers1 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "1" + "_" + autoFixture.Create<string>(), customerBillToNumber: "",
                useExistingDeclarationsIfAny: false);
            var seededCustomers2 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "2" + "_" + autoFixture.Create<string>(),
                customerBillToNumber: "", useExistingDeclarationsIfAny: false);
            var seededCustomers21 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "21" + "_" + autoFixture.Create<string>(),
                customerBillToNumber: seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value,
                useExistingDeclarationsIfAny: false);
            var token = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            api.Client.SetToken(token.AccessToken);

            //Act
            var customersReceived = await api.GetPossibleCustomerPermissions();
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            Assert.Equal(2, customersReceived.CustomerNodes.Count());
            Assert.Empty(customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers1.FirstOrDefault()!.CustomerId.Value.ToString())
                .FirstOrDefault()!.Children);
            Assert.Single(customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers2.FirstOrDefault()!.CustomerId.Value.ToString())
                .FirstOrDefault()!.Children);
            Assert.Empty(customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers2.FirstOrDefault()!.CustomerId.Value.ToString())
                .FirstOrDefault()!.Children.FirstOrDefault()!.Children);
        }

        [Theory]
        [InlineData(UserRole.User, true)]
        [InlineData(UserRole.Admin, true)]
        public async Task PossibleCustomerPermissions_CustomerDoesntHaveAccess(UserRole userRole, bool shouldHaveAccess)
        {
            //Arrange
            var permissions = new List<RegisterUserCustomerPermissionDto>();
            var tokenCommonAdmin = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            api.Client.SetToken(tokenCommonAdmin.AccessToken);
            var (userName, password) = await api.RegisterUser(permissions, role: userRole);
            var token = await api.LoginUser(userName, password);
            api.Client.SetToken(token.AccessToken);
            var exceptionThrown = false;

            //Act
            try
            {
                await api.GetPossibleCustomerPermissions();
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            Assert.Equal(!shouldHaveAccess, exceptionThrown);
        }

        [Fact]
        public async Task PossibleCustomerPermissions_CustomerAdminHasAccessToItsCustomers()
        {
            //Arrange
            var seededCustomers1 = await CustomerTestData.SeedWithCustomer(1, webAppFixture, customerNumber: "1" + "_" + autoFixture.Create<string>(), customerBillToNumber: "", useExistingDeclarationsIfAny: false);
            var seededCustomers2 = await CustomerTestData.SeedWithCustomer(1, webAppFixture, customerNumber: "2" + "_" + autoFixture.Create<string>(),
                customerBillToNumber: "", useExistingDeclarationsIfAny: false);
            var seededCustomers21 = await CustomerTestData.SeedWithCustomer(1, webAppFixture, customerNumber: "21" + "_" + autoFixture.Create<string>(), customerBillToNumber: seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value, useExistingDeclarationsIfAny: false);
            var permissions = new List<RegisterUserCustomerPermissionDto>()
            {
                new()
                {
                    CustomerId = seededCustomers1.FirstOrDefault()!.CustomerId.Value,
                    CustomerNumber = seededCustomers1.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value,
                    CustomerName = seededCustomers1.FirstOrDefault()!.CustomerDetails.CustomerName.Value,
                    Permissions =new List<CustomerPermission>()
                    {
                        CustomerPermission.FuelConsumption,
                    }
                },
                new()
                {
                    CustomerId = seededCustomers2.FirstOrDefault()!.CustomerId.Value,
                    CustomerNumber = seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value,
                    CustomerName = seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerName.Value,
                    Permissions =new List<CustomerPermission>()
                    {
                        CustomerPermission.Admin,
                    }
                }
            };
            var tokenCommonAdmin = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            api.Client.SetToken(tokenCommonAdmin.AccessToken);
            var (userName, password) = await api.RegisterUser(permissions, role: UserRole.User);
            var token = await api.LoginUser(userName, password);
            api.Client.SetToken(token.AccessToken);

            //Act
            var customersReceived = await api.GetPossibleCustomerPermissions();
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            Assert.Single(customersReceived.CustomerNodes);
            Assert.Single(customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers2.FirstOrDefault()!.CustomerId.Value.ToString())
                .FirstOrDefault()!.Children);
            Assert.Empty(customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers2.FirstOrDefault()!.CustomerId.Value.ToString())
                .FirstOrDefault()!.Children.FirstOrDefault()!.Children);
        }

        [Fact]
        public async Task PossibleCustomerPermissions_CustomerAdminHasAccessToOnlyOneChildWithoutParent()
        {
            //Arrange
            await CustomerTestData.DeleteAllCustomers(webAppFixture);
            var seededCustomers1 = await CustomerTestData.SeedWithCustomer(1, webAppFixture, customerNumber: "1" + "_" + autoFixture.Create<string>(), customerBillToNumber: "", useExistingDeclarationsIfAny: false);
            var seededCustomers2 = await CustomerTestData.SeedWithCustomer(1, webAppFixture, customerNumber: "2" + "_" + autoFixture.Create<string>(),
                customerBillToNumber: "", useExistingDeclarationsIfAny: false);
            var seededCustomers21 = await CustomerTestData.SeedWithCustomer(1, webAppFixture, customerNumber: "21" + "_" + autoFixture.Create<string>(), customerBillToNumber: seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value, useExistingDeclarationsIfAny: false);
            var seededCustomers22 = await CustomerTestData.SeedWithCustomer(1, webAppFixture, customerNumber: "22" + "_" + autoFixture.Create<string>(), customerBillToNumber: seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value, useExistingDeclarationsIfAny: false);
            var permissions = new List<RegisterUserCustomerPermissionDto>()
            {
                new RegisterUserCustomerPermissionDto()
                {
                    CustomerId = seededCustomers21.FirstOrDefault()!.CustomerId.Value,
                    CustomerNumber = seededCustomers21.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value,
                    CustomerName = seededCustomers21.FirstOrDefault()!.CustomerDetails.CustomerName.Value,
                    Permissions =new List<CustomerPermission>()
                    {
                        CustomerPermission.Admin,
                    }
                }
            };
            var tokenCommonAdmin = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            api.Client.SetToken(tokenCommonAdmin.AccessToken);
            var (userName, password) = await api.RegisterUser(permissions, role: UserRole.User);
            var token = await api.LoginUser(userName, password);
            api.Client.SetToken(token.AccessToken);

            //Act
            var customersReceived = await api.GetPossibleCustomerPermissions();
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            Assert.Single(customersReceived.CustomerNodes);
            Assert.Empty(customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers21.FirstOrDefault()!.CustomerId.Value.ToString())
                .FirstOrDefault()!.Children);
        }

        [Fact]
        public async Task PossibleCustomerPermissions_CustomerAdminHasAccessTo4LevelStructureJustFine()
        {
            //Arrange
            await CustomerTestData.DeleteAllCustomers(webAppFixture);
            var seededCustomers1 = await CustomerTestData.SeedWithCustomer(1, webAppFixture, customerNumber: "1" + "_" + autoFixture.Create<string>(), customerBillToNumber: "", useExistingDeclarationsIfAny: false);
            var seededCustomers2 = await CustomerTestData.SeedWithCustomer(1, webAppFixture, customerNumber: "2" + "_" + autoFixture.Create<string>(),
                customerBillToNumber: "", useExistingDeclarationsIfAny: false);
            var seededCustomers21 = await CustomerTestData.SeedWithCustomer(1, webAppFixture, customerNumber: "21" + "_" + autoFixture.Create<string>(), customerBillToNumber: seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value, useExistingDeclarationsIfAny: false);
            var seededCustomers211 = await CustomerTestData.SeedWithCustomer(1, webAppFixture, customerNumber: "211" + "_" + autoFixture.Create<string>(), customerBillToNumber: seededCustomers21.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value, useExistingDeclarationsIfAny: false);
            var seededCustomers2111 = await CustomerTestData.SeedWithCustomer(1, webAppFixture, customerNumber: "2111" + "_" + autoFixture.Create<string>(), customerBillToNumber: seededCustomers211.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value, useExistingDeclarationsIfAny: false);
            var permissions = new List<RegisterUserCustomerPermissionDto>()
            {
                new RegisterUserCustomerPermissionDto()
                {
                    CustomerId = seededCustomers2.FirstOrDefault()!.CustomerId.Value,
                    CustomerNumber = seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value,
                    CustomerName = seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerName.Value,
                    Permissions =new List<CustomerPermission>()
                    {
                        CustomerPermission.Admin,
                    }
                }
            };
            var tokenCommonAdmin = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            api.Client.SetToken(tokenCommonAdmin.AccessToken);
            var (userName, password) = await api.RegisterUser(permissions, role: UserRole.User);
            var token = await api.LoginUser(userName, password);
            api.Client.SetToken(token.AccessToken);

            //Act
            var customersReceived = await api.GetPossibleCustomerPermissions();
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            Assert.Single(customersReceived.CustomerNodes);
            Assert.Empty(customersReceived.CustomerNodes.FirstOrDefault()!.ParentCustomerId);
            //21
            var children21 = customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers2.FirstOrDefault()!.CustomerId.Value.ToString())
                .FirstOrDefault()!.Children;
            Assert.Single(children21);
            Assert.Equal(seededCustomers21.FirstOrDefault()!.CustomerId.Value.ToString(), children21.FirstOrDefault()!.CustomerId);
            Assert.Equal(seededCustomers21.FirstOrDefault()!.CustomerDetails.CustomerName.Value.ToString(), children21.FirstOrDefault()!.CustomerName);
            Assert.Equal(seededCustomers21.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value.ToString(), children21.FirstOrDefault()!.CustomerNumber);
            Assert.Equal(seededCustomers2.FirstOrDefault()!.CustomerId.Value.ToString(), children21.FirstOrDefault()!.ParentCustomerId);
            //211
            var children211 = customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers2.FirstOrDefault()!.CustomerId.Value.ToString())
                .FirstOrDefault()!.Children.FirstOrDefault()!.Children;
            Assert.Single(children211);
            Assert.Equal(seededCustomers211.FirstOrDefault()!.CustomerId.Value.ToString(), children211.FirstOrDefault()!.CustomerId);
            Assert.Equal(seededCustomers211.FirstOrDefault()!.CustomerDetails.CustomerName.Value.ToString(), children211.FirstOrDefault()!.CustomerName);
            Assert.Equal(seededCustomers211.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value.ToString(), children211.FirstOrDefault()!.CustomerNumber);
            Assert.Equal(seededCustomers21.FirstOrDefault()!.CustomerId.Value.ToString(), children211.FirstOrDefault()!.ParentCustomerId);
            //2111
            var children2111 = customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers2.FirstOrDefault()!.CustomerId.Value.ToString())
                .FirstOrDefault()!.Children.FirstOrDefault()!.Children.FirstOrDefault()!.Children;
            Assert.Single(children2111);
            Assert.Equal(seededCustomers2111.FirstOrDefault()!.CustomerId.Value.ToString(), children2111.FirstOrDefault()!.CustomerId);
            Assert.Equal(seededCustomers2111.FirstOrDefault()!.CustomerDetails.CustomerName.Value.ToString(), children2111.FirstOrDefault()!.CustomerName);
            Assert.Equal(seededCustomers2111.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value.ToString(), children2111.FirstOrDefault()!.CustomerNumber);
            Assert.Equal(seededCustomers211.FirstOrDefault()!.CustomerId.Value.ToString(), children2111.FirstOrDefault()!.ParentCustomerId);
            //2111x
            var children2111x = customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers2.FirstOrDefault()!.CustomerId.Value.ToString())
                .FirstOrDefault()!.Children.FirstOrDefault()!.Children.FirstOrDefault()!.Children.FirstOrDefault()!
                .Children;
            Assert.Empty(children2111x);
        }

        [Fact]
        public async Task PossibleCustomerPermissionsForGivenUser_AdminHasAccessToAllCustomers()
        {
            //Arrange
            await CustomerTestData.DeleteAllCustomers(webAppFixture);
            var seededCustomers1 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "1" + "_" + autoFixture.Create<string>(), customerBillToNumber: "",
                useExistingDeclarationsIfAny: false);
            var seededCustomers2 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "2" + "_" + autoFixture.Create<string>(),
                customerBillToNumber: "", useExistingDeclarationsIfAny: false);
            var seededCustomers21 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "21" + "_" + autoFixture.Create<string>(),
                customerBillToNumber: seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value,
                useExistingDeclarationsIfAny: false);
            var seededCustomers22 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "22" + "_" + autoFixture.Create<string>(),
                customerBillToNumber: seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value,
                useExistingDeclarationsIfAny: false);
            var token = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            api.Client.SetToken(token.AccessToken);
            var permissions = new List<RegisterUserCustomerPermissionDto>()
            {
                new RegisterUserCustomerPermissionDto()
                {
                    CustomerId = seededCustomers2.FirstOrDefault()!.CustomerId.Value,
                    CustomerNumber = seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value,
                    CustomerName = seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerName.Value,
                    Permissions =new List<CustomerPermission>()
                    {
                        CustomerPermission.FuelConsumption,
                        CustomerPermission.SustainabilityReport,
                    }
                }
            };
            var (userName, password) = await api.RegisterUser(permissions, role: UserRole.User);
            //Act
            var customersReceived = await api.GetPossibleCustomerPermissionsForGivenUser(userName);
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            Assert.Equal(2, customersReceived.CustomerNodes.Count());
            //1
            var customers1 = customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers1.FirstOrDefault()!.CustomerId.Value.ToString()).ToList();
            Assert.Single(customers1);
            Assert.Empty(customers1.FirstOrDefault()!.Children);
            Assert.Equal(Enum.GetValues(typeof(CustomerPermission)).Length, customers1[0].PermissionsAvailable.Distinct().Count());
            Assert.Empty(customers1[0].PermissionsGiven);
            //2
            var customers2 = customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers2.FirstOrDefault()!.CustomerId.Value.ToString()).ToList();
            Assert.Single(customers2);
            Assert.Equal(2, customers2.FirstOrDefault()!.Children.Count);
            Assert.Equal(Enum.GetValues(typeof(CustomerPermission)).Length, customers2[0].PermissionsAvailable.Distinct().Count());
            Assert.Equal(2, customers2[0].PermissionsGiven.Count());
            Assert.Contains(CustomerPermission.FuelConsumption, customers2[0].PermissionsGiven);
            Assert.Contains(CustomerPermission.SustainabilityReport, customers2[0].PermissionsGiven);
            //21
            var customers21 = customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers2.FirstOrDefault()!.CustomerId.Value.ToString()).FirstOrDefault()!.Children.Where(ch => ch.CustomerId == seededCustomers21.FirstOrDefault()!.CustomerId.Value.ToString()).ToList();
            Assert.Single(customers21);
            Assert.Empty(customers21.FirstOrDefault()!.Children);
            Assert.Equal(Enum.GetValues(typeof(CustomerPermission)).Length, customers21[0].PermissionsAvailable.Distinct().Count());
            Assert.Equal(2, customers21[0].PermissionsGiven.Count());
            Assert.Contains(CustomerPermission.FuelConsumption, customers21[0].PermissionsGiven);
            Assert.Contains(CustomerPermission.SustainabilityReport, customers21[0].PermissionsGiven);
            //22
            var customers22 = customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers2.FirstOrDefault()!.CustomerId.Value.ToString()).FirstOrDefault()!.Children.Where(ch => ch.CustomerId == seededCustomers22.FirstOrDefault()!.CustomerId.Value.ToString()).ToList();
            Assert.Single(customers22);
            Assert.Empty(customers22.FirstOrDefault()!.Children);
            Assert.Equal(Enum.GetValues(typeof(CustomerPermission)).Length, customers22[0].PermissionsAvailable.Distinct().Count());
            Assert.Equal(2, customers22[0].PermissionsGiven.Count());
            Assert.Contains(CustomerPermission.FuelConsumption, customers22[0].PermissionsGiven);
            Assert.Contains(CustomerPermission.SustainabilityReport, customers22[0].PermissionsGiven);
        }

        [Fact]
        public async Task PossibleCustomerPermissionsForGivenUser_UserWithAdminPermissionsHasAccessToOnlyItsCustomers()
        {
            //Arrange
            await CustomerTestData.DeleteAllCustomers(webAppFixture);
            var seededCustomers1 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "1" + "_" + autoFixture.Create<string>(), customerBillToNumber: "",
                useExistingDeclarationsIfAny: false);
            var seededCustomers2 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "2" + "_" + autoFixture.Create<string>(),
                customerBillToNumber: "", useExistingDeclarationsIfAny: false);
            var seededCustomers21 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "21" + "_" + autoFixture.Create<string>(),
                customerBillToNumber: seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value,
                useExistingDeclarationsIfAny: false);
            var seededCustomers22 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "22" + "_" + autoFixture.Create<string>(),
                customerBillToNumber: seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value,
                useExistingDeclarationsIfAny: false);
            var token = await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            api.Client.SetToken(token.AccessToken);
            var permissions = new List<RegisterUserCustomerPermissionDto>()
            {
                new()
                {
                    CustomerId = seededCustomers1.FirstOrDefault()!.CustomerId.Value,
                    CustomerNumber = seededCustomers1.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value,
                    CustomerName = seededCustomers1.FirstOrDefault()!.CustomerDetails.CustomerName.Value,
                    Permissions =new List<CustomerPermission>()
                    {
                        CustomerPermission.FuelConsumption,
                        CustomerPermission.SustainabilityReport,
                        CustomerPermission.FleetManagement,
                    }
                },
                new ()
                {
                    CustomerId = seededCustomers2.FirstOrDefault()!.CustomerId.Value,
                    CustomerNumber = seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value,
                    CustomerName = seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerName.Value,
                    Permissions =new List<CustomerPermission>()
                    {
                        CustomerPermission.FuelConsumption,
                        CustomerPermission.SustainabilityReport,
                    }
                }
            };
            var (userName, password) = await api.RegisterUser(permissions, role: UserRole.User);
            var permissionsAdmin = new List<RegisterUserCustomerPermissionDto>()
            {
                new()
                {
                    CustomerId = seededCustomers2.FirstOrDefault()!.CustomerId.Value,
                    CustomerNumber = seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerNumber.Value,
                    CustomerName = seededCustomers2.FirstOrDefault()!.CustomerDetails.CustomerName.Value,
                    Permissions =new List<CustomerPermission>()
                    {
                        CustomerPermission.Admin
                    }
                }
            };
            var (userNameAdmin, passwordAdmin) = await api.RegisterUser(permissionsAdmin, role: UserRole.User);
            token = await api.LoginUser(userNameAdmin, passwordAdmin);
            api.Client.SetToken(token.AccessToken);
            //Act
            var customersReceived = await api.GetPossibleCustomerPermissionsForGivenUser(userName);
            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            Assert.Single(customersReceived.CustomerNodes);
            //1 not found even customer has it, the one with admin permissions doesn't have acecss to it
            var customers1 = customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers1.FirstOrDefault()!.CustomerId.Value.ToString());
            Assert.Empty(customers1);
            //2
            var customers2 = customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers2.FirstOrDefault()!.CustomerId.Value.ToString()).ToList();
            Assert.Single(customers2);
            Assert.Equal(2, customers2.FirstOrDefault()!.Children.Count);
            Assert.Equal(Enum.GetValues(typeof(CustomerPermission)).Length, customers2[0].PermissionsAvailable.Distinct().Count());
            Assert.Equal(2, customers2[0].PermissionsGiven.Count());
            Assert.Contains(CustomerPermission.FuelConsumption, customers2[0].PermissionsGiven);
            Assert.Contains(CustomerPermission.SustainabilityReport, customers2[0].PermissionsGiven);
            //21
            var customers21 = customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers2.FirstOrDefault()!.CustomerId.Value.ToString()).FirstOrDefault()!.Children.Where(ch => ch.CustomerId == seededCustomers21.FirstOrDefault()!.CustomerId.Value.ToString()).ToList();
            Assert.Single(customers21);
            Assert.Empty(customers21.FirstOrDefault()!.Children);
            Assert.Equal(Enum.GetValues(typeof(CustomerPermission)).Length, customers21[0].PermissionsAvailable.Distinct().Count());
            Assert.Equal(2, customers21[0].PermissionsGiven.Count());
            Assert.Contains(CustomerPermission.FuelConsumption, customers21[0].PermissionsGiven);
            Assert.Contains(CustomerPermission.SustainabilityReport, customers21[0].PermissionsGiven);
            //22
            var customers22 = customersReceived.CustomerNodes
                .Where(c => c.CustomerId == seededCustomers2.FirstOrDefault()!.CustomerId.Value.ToString()).FirstOrDefault()!.Children.Where(ch => ch.CustomerId == seededCustomers22.FirstOrDefault()!.CustomerId.Value.ToString()).ToList();
            Assert.Single(customers22);
            Assert.Empty(customers22.FirstOrDefault()!.Children);
            Assert.Equal(Enum.GetValues(typeof(CustomerPermission)).Length, customers22[0].PermissionsAvailable.Distinct().Count());
            Assert.Equal(2, customers22[0].PermissionsGiven.Count());
            Assert.Contains(CustomerPermission.FuelConsumption, customers22[0].PermissionsGiven);
            Assert.Contains(CustomerPermission.SustainabilityReport, customers22[0].PermissionsGiven);
        }
    }
}
