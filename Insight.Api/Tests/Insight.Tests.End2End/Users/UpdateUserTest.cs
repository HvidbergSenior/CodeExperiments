using System.Net;
using AutoFixture;
using FluentAssertions;
using Insight.BuildingBlocks.Domain;
using Insight.Tests.End2End.TestData;
using Insight.UserAccess.Application.RegisterUser;
using Insight.UserAccess.Application.UpdateUser;
using Insight.UserAccess.Domain.User;
using Xunit;

namespace Insight.Tests.End2End.Users
{
    [Collection("End2End")]
    public class UpdateUserTest
    {
        private readonly Api api;
        private readonly WebAppFixture webAppFixture;
        private readonly Fixture any = new();

        public UpdateUserTest(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
            api = new Api(webAppFixture);
        }
        
        [Fact]
        public async Task UserShouldBeUpdated()
        {
            //Arrange
            var customerId = Guid.NewGuid();
            var customerName = "SomeCustomer";
            
            var permissionsUser = new List<CustomerPermission>()
            {
                CustomerPermission.Admin,
                CustomerPermission.FuelConsumption
            };

            var permissions = new List<RegisterUserCustomerPermissionDto>()
            {
                new RegisterUserCustomerPermissionDto()
                {
                    CustomerId = customerId,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName,
                    Permissions = permissionsUser
                }
            };
            var tokenPreMadeUser =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(tokenPreMadeUser.AccessToken);
            
            var (userName, password) = await api.RegisterUser(permissions, role: UserRole.Admin);
            var token = await api.LoginUser(userName, password);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            
            var users = await api.GetAllUsers(1,1000, true, "id", "Active", "", "");
            var userId = users.Users.Where(u => u.UserName == userName).FirstOrDefault()!.UserId;
            var updateParameters = Any.UserUpdateParameters(Guid.Parse(userId));
            updateParameters.UserType = UserRole.User;
            updateParameters.Status = UserStatus.Blocked;

            //Act
            await api.UpdateUser(updateParameters, new List<UpdateUserCustomerPermissionDto>());
            var users2 = await api.GetAllUsers(1,1000, true, "id", "BlockedAndActive", "", "");

            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            Assert.Empty(users.Users.Where(u => u.UserName == updateParameters.UserName.Value));
            Assert.Single(users2.Users.Where(u => u.UserName == updateParameters.UserName.Value));
            var user2Details = users2.Users.Where(u => u.UserName == updateParameters.UserName.Value).FirstOrDefault();
            Assert.Equal(updateParameters.FirstName.Value, user2Details!.FirstName);
            Assert.Equal(updateParameters.LastName.Value, user2Details!.LastName);
            Assert.Equal(updateParameters.Email.Value, user2Details!.Email);
            Assert.Equal(updateParameters.UserType, user2Details!.UserType);
            Assert.True(user2Details!.Blocked);
        }

        [Fact]
        public async Task AdminCanUpdateAllUserPermissions()
        {
            //Arrange
            var permissionsUser = new List<CustomerPermission>()
            {
                CustomerPermission.FuelConsumption
            };
            var permissionsUserAfterUpdate = new List<CustomerPermission>()
            {
                CustomerPermission.FuelConsumption,
                CustomerPermission.SustainabilityReport
            };
            var seededCustomer1 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "SomeCustomerNumber", customerBillToNumber: "",
                useExistingDeclarationsIfAny: false);
            var seededCustomer2 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "SomeCustomerNumber", customerBillToNumber: "",
                useExistingDeclarationsIfAny: false);
            var customerId1 = seededCustomer1.FirstOrDefault()!.CustomerId.Value;
            var customerName1 = seededCustomer1.FirstOrDefault()!.CustomerDetails.CustomerName.Value;
            var customerId2 = seededCustomer2.FirstOrDefault()!.CustomerId.Value;
            var customerName2 = seededCustomer2.FirstOrDefault()!.CustomerDetails.CustomerName.Value;

            var permissions = new List<RegisterUserCustomerPermissionDto>()
            {
                new ()
                {
                    CustomerId = customerId1,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName1,
                    Permissions = permissionsUser
                },
                new ()
                {
                    CustomerId = customerId2,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName2,
                    Permissions = permissionsUser
                }
            };
            var permissionsAfterUpdate = new List<UpdateUserCustomerPermissionDto>()
            {
                new ()
                {
                    CustomerId = customerId1,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName1,
                    Permissions = permissionsUserAfterUpdate
                },
                new ()
                {
                    CustomerId = customerId2,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName2,
                    Permissions = permissionsUserAfterUpdate
                }
            };
            var tokenPreMadeUser =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(tokenPreMadeUser.AccessToken);

            var (userName, password) = await api.RegisterUser(permissions, role: UserRole.User);

            var users = await api.GetAllUsers(1,1000, true, "id", "Active", "", "");
            var userId = users.Users.Where(u => u.UserName == userName).FirstOrDefault()!.UserId;
            var updateParameters = Any.UserUpdateParameters(Guid.Parse(userId));

            //Act
            await api.UpdateUser(updateParameters, permissionsAfterUpdate);
            var users2 = await api.GetAllUsers(1,1000, true, "id", "Active", "", "");

            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            Assert.Single(users.Users.Where(u => u.UserName == userName));
            Assert.Empty(users.Users.Where(u => u.UserName == updateParameters.UserName.Value));
            Assert.Empty(users2.Users.Where(u => u.UserName == userName));
            Assert.Single(users2.Users.Where(u => u.UserName == updateParameters.UserName.Value));
            Assert.Single(users2.Users.Where(u => u.UserName == updateParameters.UserName.Value).Where(u =>
                    u.HasSustainabilityReportAccess == true && u.HasFuelConsumptionAccess == true &&
                    u.HasFleetManagementAccess == false));
        }

        [Fact]
        public async Task UserWithAdminPermissionsCanSetPermissionsItHasAdminAccessTo()
        {
            //Arrange
            var permissionsUser = new List<CustomerPermission>()
            {
                CustomerPermission.FuelConsumption
            };
            var permissionsUserAfterUpdate = new List<CustomerPermission>()
            {
                CustomerPermission.FuelConsumption,
                CustomerPermission.SustainabilityReport
            };
            var seededCustomer1 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "SomeCustomerNumber", customerBillToNumber: "",
                useExistingDeclarationsIfAny: false);
            var seededCustomer2 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "SomeCustomerNumber", customerBillToNumber: "",
                useExistingDeclarationsIfAny: false);
            var customerId1 = seededCustomer1.FirstOrDefault()!.CustomerId.Value;
            var customerName1 = seededCustomer1.FirstOrDefault()!.CustomerDetails.CustomerName.Value;
            var customerId2 = seededCustomer2.FirstOrDefault()!.CustomerId.Value;
            var customerName2 = seededCustomer2.FirstOrDefault()!.CustomerDetails.CustomerName.Value;

            var permissions = new List<RegisterUserCustomerPermissionDto>()
            {
                new ()
                {
                    CustomerId = customerId1,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName1,
                    Permissions = permissionsUser
                },
                new ()
                {
                    CustomerId = customerId2,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName2,
                    Permissions = permissionsUser
                }
            };
            var permissionsAdmin = new List<RegisterUserCustomerPermissionDto>()
            {
                new ()
                {
                    CustomerId = customerId2,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName2,
                    Permissions = new List<CustomerPermission>()
                    {
                        CustomerPermission.Admin
                    }
                }
            };
            var permissionsAfterUpdate = new List<UpdateUserCustomerPermissionDto>()
            {
                new ()
                {
                    CustomerId = customerId2,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName2,
                    Permissions = permissionsUserAfterUpdate
                }
            };
            var tokenPreMadeUser =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            api.Client.SetToken(tokenPreMadeUser.AccessToken);

            var (userName, password) = await api.RegisterUser(permissions, role: UserRole.User);
            var (userNameAdmin, passwordAdmin) = await api.RegisterUser(permissionsAdmin, role: UserRole.User);

            var users = await api.GetAllUsers(1,1000, true, "id", "Active", "", "");
            var userId = users.Users.Where(u => u.UserName == userName).FirstOrDefault()!.UserId;
            var token =  await api.LoginUser(userNameAdmin, passwordAdmin);
            api.Client.SetToken(token.AccessToken);
            var updateParameters = Any.UserUpdateParameters(Guid.Parse(userId));

            //Act
            await api.UpdateUser(updateParameters, permissionsAfterUpdate);
            api.Client.SetToken(tokenPreMadeUser.AccessToken);
            var users2 = await api.GetAllUsers(1,1000, true, "id", "Active", "", "");

            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            Assert.Single(users.Users.Where(u => u.UserName == userName));
            Assert.Empty(users.Users.Where(u => u.UserName == updateParameters.UserName.Value));
            Assert.Empty(users2.Users.Where(u => u.UserName == userName));
            Assert.Single(users2.Users.Where(u => u.UserName == updateParameters.UserName.Value));

            Assert.Single(users2.Users.Where(u => u.UserName == updateParameters.UserName.Value).Where(u =>
                    u.HasSustainabilityReportAccess == true && u.HasFuelConsumptionAccess == true &&
                    u.HasFleetManagementAccess == false));
        }

        [Fact]
        public async Task UserWithAdminPermissionsCannotSetPermissionsItHasNotAdminAccessTo()
        {
            //Arrange
            var permissionsUser = new List<CustomerPermission>()
            {
                CustomerPermission.FuelConsumption
            };
            var permissionsUserAfterUpdate = new List<CustomerPermission>()
            {
                CustomerPermission.FuelConsumption,
                CustomerPermission.SustainabilityReport
            };
            var seededCustomer1 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "SomeCustomerNumber", customerBillToNumber: "",
                useExistingDeclarationsIfAny: false);
            var seededCustomer2 = await CustomerTestData.SeedWithCustomer(1, webAppFixture,
                customerNumber: "SomeCustomerNumber", customerBillToNumber: "",
                useExistingDeclarationsIfAny: false);
            var customerId1 = seededCustomer1.FirstOrDefault()!.CustomerId.Value;
            var customerName1 = seededCustomer1.FirstOrDefault()!.CustomerDetails.CustomerName.Value;
            var customerId2 = seededCustomer2.FirstOrDefault()!.CustomerId.Value;
            var customerName2 = seededCustomer2.FirstOrDefault()!.CustomerDetails.CustomerName.Value;

            var permissions = new List<RegisterUserCustomerPermissionDto>()
            {
                new ()
                {
                    CustomerId = customerId1,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName1,
                    Permissions = permissionsUser
                },
                new ()
                {
                    CustomerId = customerId2,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName2,
                    Permissions = permissionsUser
                }
            };
            var permissionsAdmin = new List<RegisterUserCustomerPermissionDto>()
            {
                new ()
                {
                    CustomerId = customerId2,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName2,
                    Permissions = new List<CustomerPermission>()
                    {
                        CustomerPermission.Admin
                    }
                }
            };
            var permissionsAfterUpdate = new List<UpdateUserCustomerPermissionDto>()
            {
                new ()
                {
                    CustomerId = customerId1,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName1,
                    Permissions = permissionsUserAfterUpdate
                },
                new ()
                {
                    CustomerId = customerId2,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName2,
                    Permissions = permissionsUserAfterUpdate
                }
            };
            var tokenPreMadeUser =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            api.Client.SetToken(tokenPreMadeUser.AccessToken);

            var (userName, password) = await api.RegisterUser(permissions, role: UserRole.User);
            var (userNameAdmin, passwordAdmin) = await api.RegisterUser(permissionsAdmin, role: UserRole.User);

            var users = await api.GetAllUsers(1,1000, true, "id", "Active", "", "");
            var userId = users.Users.Where(u => u.UserName == userName).FirstOrDefault()!.UserId;
            var token =  await api.LoginUser(userNameAdmin, passwordAdmin);
            api.Client.SetToken(token.AccessToken);
            var updateParameters = Any.UserUpdateParameters(Guid.Parse(userId));

            //Act and assert
            await api.UpdateUser(updateParameters, permissionsAfterUpdate,
                expectedResult: HttpStatusCode.BadRequest);
            api.Client.RemoveToken();
        }
    }
}
