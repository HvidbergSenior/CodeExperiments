using AutoFixture;
using FluentAssertions;
using Insight.BuildingBlocks.Domain;
using Insight.UserAccess.Application.RegisterUser;
using Insight.UserAccess.Domain.User;
using Xunit;

namespace Insight.Tests.End2End.Users
{
    [Collection("End2End")]
    public class GetAllUsersWithPermissionsForCustomerTest
    {
        private readonly Api api;
        private readonly WebAppFixture webAppFixture;
        private readonly Fixture any = new();

        public GetAllUsersWithPermissionsForCustomerTest(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
            api = new Api(webAppFixture);
        }

        [Fact]
        public async Task GetAllUsers_WithPermissionsForCustomer_ShouldReturnUsersForThatCustomer()
        {
            //Arrange
            var customerId = Guid.NewGuid();

            var customerName = "SomeCustomer";

            var permissionsUser = new List<CustomerPermission>()
            {
                CustomerPermission.Admin,
                CustomerPermission.FuelConsumption
            };

            var permissionsUser2 = new List<CustomerPermission>()
            {   
                CustomerPermission.FleetManagement
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

            var permissions2 = new List<RegisterUserCustomerPermissionDto>()
            {
                new RegisterUserCustomerPermissionDto()
                {
                    CustomerId = customerId,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName,
                    Permissions = permissionsUser2
                }
            };
            var tokenPreMadeUser =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(tokenPreMadeUser.AccessToken);
            var (userName, password) = await api.RegisterUser(permissions, role: UserRole.User);
            var (userName2, password2) = await api.RegisterUser(permissions2, role: UserRole.User);
            
            var token = await api.LoginUser(userName, password);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);

            //Act
            var users = await api.GetAllUsers(1,10, true, "id", "Active", "", "");

            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            users.Users.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllUsers_WithPermissionsForOneCustomer_ShouldNotReturnUsersForOtherCustomers()
        {
            //Arrange
            var customerId = Guid.NewGuid();
            var otherCustomerId = Guid.NewGuid();

            var customerName = "SomeCustomer";

            var permissionsUser = new List<CustomerPermission>()
            {
                CustomerPermission.Admin,
                CustomerPermission.FuelConsumption
            };

            var permissionsUser2 = new List<CustomerPermission>()
            {
                CustomerPermission.FleetManagement
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

            var permissions2 = new List<RegisterUserCustomerPermissionDto>()
            {
                new RegisterUserCustomerPermissionDto()
                {
                    CustomerId = otherCustomerId,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName,
                    Permissions = permissionsUser2
                }
            };
            var tokenPreMadeUser =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(tokenPreMadeUser.AccessToken);
            
            var (userName, password) = await api.RegisterUser(permissions, role: UserRole.User);
            var (userName2, password2) = await api.RegisterUser(permissions2, role: UserRole.User);

            var token = await api.LoginUser(userName, password);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);

            //Act
            //TODO: AccountId consist of 2 things CustomerName and CustomerNumber
            //var accountId = "accountId";
            var users = await api.GetAllUsers(1, 10, true, "id", "Active", "", "");

            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            users.Users.Should().HaveCount(1);
        }
        
         [Fact]
        public async Task GetAllUsers_Filter_Email()
        {
            //Arrange
            var customerId = Guid.NewGuid();

            var customerName = "SomeCustomer";

            var permissionsUser = new List<CustomerPermission>()
            {
                CustomerPermission.Admin,
                CustomerPermission.FuelConsumption
            };

            var permissionsUser2 = new List<CustomerPermission>()
            {   
                CustomerPermission.FleetManagement
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

            var permissions2 = new List<RegisterUserCustomerPermissionDto>()
            {
                new RegisterUserCustomerPermissionDto()
                {
                    CustomerId = customerId,
                    CustomerNumber = any.Create<string>(),
                    CustomerName = customerName,
                    Permissions = permissionsUser2
                }
            };
            var tokenPreMadeUser =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(tokenPreMadeUser.AccessToken);
            var (userName, password) = await api.RegisterUser(permissions, role: UserRole.User);
            var (userName2, password2) = await api.RegisterUser(permissions2, role: UserRole.User);
            
            var token = await api.LoginUser(userName, password);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);

            //Act
            var users = await api.GetAllUsers(1,10, true, "id", "Active", "", userName);

            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            users.Users.Should().HaveCount(1);
        }
    }
}
