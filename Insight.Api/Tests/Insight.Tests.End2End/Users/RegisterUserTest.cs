using FluentAssertions;
using Insight.BuildingBlocks.Domain;
using Insight.Tests.End2End.TestData;
using Insight.UserAccess.Application.RegisterUser;
using Insight.UserAccess.Domain.User;
using Xunit;

namespace Insight.Tests.End2End.Users
{
    [Collection("End2End")]
    public class RegisterUserTest
    {
        private readonly Api api;
        private readonly WebAppFixture webAppFixture;

        public RegisterUserTest(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
            api = new Api(webAppFixture);
        }

        [Fact]
        public async Task UserCreate_WithoutError()
        {
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            api.Client.SetToken(token.AccessToken);

            await api.RegisterUser(role: UserRole.Admin);
            
            // Remove token
            api.Client.RemoveToken();
        }

        [Fact]
        public async Task UserCanLogin_WithoutError()
        {
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);

            // Login
            token.Should().NotBeNull();
            token.RefreshToken.Should().NotBeNull();
            token.AccessToken.Should().NotBeNull();
        }

        [Fact]
        public async Task UserCanChangePassword_WithoutError()
        {
            var tokenCommonAdmin =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            api.Client.SetToken(tokenCommonAdmin.AccessToken);
            var (user, password) = await api.RegisterUser(role: UserRole.User);
            var token =  await api.LoginUser(user, password);

            const string NewPassword = "Test4321!";
            
            // Set token
            api.Client.SetToken(token.AccessToken);

            // Change Password
            await api.ChangePassword(user, password, NewPassword);

            // Remove token
            api.Client.RemoveToken();
        }

        [Fact]
        public async Task UserCanNotChangePassword_WithIncorrectOldPassword()
        {
            var tokenCommonAdmin =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            api.Client.SetToken(tokenCommonAdmin.AccessToken);
            var (user, password) = await api.RegisterUser(role: UserRole.User);
            var token =  await api.LoginUser(user, password);

            const string NewPassword = "Test4321!";

            
            // Set token
            api.Client.SetToken(token.AccessToken);

            // Change Password with wrong password.
            await api.ChangePassword(Logins.AdminLogin, "AnIncorrectPassword1!", NewPassword,
                expectedResult: System.Net.HttpStatusCode.BadRequest);

            // Remove token
            api.Client.RemoveToken();
        }

        [Fact]
        public async Task RegisterUserWithPermissionsAsAdminWorks()
        {
            var tokenCommonAdmin =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            api.Client.SetToken(tokenCommonAdmin.AccessToken);

            // Change Password with wrong password.
            await api.RegisterUser(new List<RegisterUserCustomerPermissionDto>()
            {
                new()
                {
                    CustomerId = Guid.NewGuid(),
                    CustomerNumber = Any.Instance<string>(),
                    Permissions = new List<CustomerPermission>()
                    {
                        CustomerPermission.FuelConsumption,
                        CustomerPermission.SustainabilityReport,
                    },
                    CustomerName = "SomeCustomer"
                }
            });

            // Remove token
            api.Client.RemoveToken();
        }

        [Fact]
        public async Task RegisterUserWithPermissionsAsUserWithoutRequiredFails()
        {
            var tokenCommonAdmin =  await api.LoginUser(Logins.CustomerLogin, Logins.LoginPassword);
            api.Client.SetToken(tokenCommonAdmin.AccessToken);
            var exceptionSet = false;
            try
            {
                await api.RegisterUser(new List<RegisterUserCustomerPermissionDto>()
                {
                    new RegisterUserCustomerPermissionDto
                    {
                        CustomerId = Guid.NewGuid(),
                        CustomerNumber = Any.Instance<string>(),
                        Permissions = new List<CustomerPermission>()
                        {
                            CustomerPermission.FuelConsumption,
                            CustomerPermission.SustainabilityReport,
                        },
                        CustomerName = "SomeCustomer"
                    }
                });
            }
            catch (Exception)
            {
                exceptionSet = true;
            }

            // Remove token
            api.Client.RemoveToken();

            Assert.True(exceptionSet);
        }

        [Fact]
        public async Task RegisterUserWithPermissionsAsUserWithRequiredOnesWorks()
        {
            var tokenCommonAdmin =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            api.Client.SetToken(tokenCommonAdmin.AccessToken);
            var seededCustomer = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "SomeCustomerNumber", customerBillToNumber: "",
                useExistingDeclarationsIfAny: false);
            var customerId = seededCustomer.FirstOrDefault()!.CustomerId.Value;
            var customerName = seededCustomer.FirstOrDefault()!.CustomerDetails.CustomerName.Value;
            var permissions = new List<RegisterUserCustomerPermissionDto>()
            {
                new()
                {
                    CustomerId = customerId,
                    CustomerNumber = Any.Instance<string>(),
                    Permissions = new List<CustomerPermission>()
                    {
                        CustomerPermission.Admin,
                    },
                    CustomerName = customerName
                }
            };
            
            
            var (userAdmin, passwordAdmin) = await api.RegisterUser(permissions, "", "", "", "", UserRole.Admin);
            
            var token =  await api.LoginUser(userAdmin, passwordAdmin);
            api.Client.SetToken(token.AccessToken);
            var (user, password) = await api.RegisterUser(new List<RegisterUserCustomerPermissionDto>()
            {
                new()
                {
                    CustomerId = customerId,
                    CustomerNumber = Any.Instance<string>(),
                    Permissions = new List<CustomerPermission>()
                    {
                        CustomerPermission.SustainabilityReport,
                    },
                    CustomerName = customerName
                }
            });

            // Remove token
            api.Client.RemoveToken();
        }
    }
}
