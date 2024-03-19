using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BuildingBlocks.Infrastructure.InitialData;
using Insight.UserAccess.Domain.Auth;
using Insight.UserAccess.Domain.User;
using Marten;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.UserAccess.Infrastructure
{
    public class DefaultUserProvider : IDefaultDataProvider
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public DefaultUserProvider(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Populate(IDocumentStore documentStore, CancellationToken cancellation)
        {
            using var asyncScope = serviceScopeFactory.CreateAsyncScope();
            using var session = documentStore.IdentitySession();
            if(!session.Query<User>().Any())
            {
                var userManager = asyncScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var unitOfWork = asyncScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                await CreateAdminUser(userManager);
                await CreateCustomerUser(userManager);

                await unitOfWork.Commit(cancellation);
            }
        }

        private async Task CreateAdminUser(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser adminUser = new()
            {
                Email = "admin@biofuel-express.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "admin@biofuel-express.com",
                FirstName = "Hans",
                LastName = "Jensen"
            };
            await userManager.AddToRoleAsync(adminUser, "Admin");

            var createUserResult = await userManager.CreateAsync(adminUser, "Test1234!");

            if (!createUserResult.Succeeded)
                throw new BusinessException("User creation failed!");
        }

        private async Task CreateCustomerUser(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser adminUser = new()
            {
                Email = "customer@biofuel-express.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "customer@biofuel-express.com",
                FirstName = "Mik",
                LastName = "Jensen"
            };
            await userManager.AddToRoleAsync(adminUser, "User");

            var createUserResult = await userManager.CreateAsync(adminUser, "Test1234!");

            if (!createUserResult.Succeeded)
                throw new BusinessException("User creation failed!");
        }
    }
}
