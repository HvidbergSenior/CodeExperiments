using FluentAssertions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BusinessCentralEntities.Domain;
using Insight.BusinessCentralEntities.Domain.Products;
using Insight.FuelTransactions.Domain.Stock;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Insight.BuildingBlocks.Domain;
using Insight.UserAccess.Domain.User;
using Xunit;
using CompanyName = Insight.FuelTransactions.Domain.CompanyName;

namespace Insight.Tests.End2End.Stocks
{
    [Collection("End2End")]
    public class StockTests
    {
        private readonly WebAppFixture webAppFixture;
        private readonly Api api;

        public StockTests(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
            api = new Api(webAppFixture);
        }

        [Fact]
        public async Task UserCanNotGetStock_WithoutValidToken()
        {
            var filteringParameters = Any.IncomingFilteringParameters();

            var errorResult =
                await Assert.ThrowsAsync<HttpRequestException>(() =>
                    api.GetStocks(filteringParameters.DatePeriod.StartDate, filteringParameters.DatePeriod.EndDate, filteringParameters.Product.Value, filteringParameters.Company.Value, 0, 0, true, "Company"));

            errorResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task UserCanGetStocks_WithValidToken()
        {
            var stockRepo = webAppFixture.AppFactory.Services.GetRequiredService<IStockTransactionsRepository>();
            var unitOfWork = webAppFixture.AppFactory.Services.GetRequiredService<IUnitOfWork>();

            var productName = ProductName.Create("ProductName");
            var companyName = CompanyName.Create("CompanyName");

            await stockRepo.Add(Any.StockTransaction(productName, companyName), CancellationToken.None);
            await stockRepo.SaveChanges(CancellationToken.None);
            await unitOfWork.Commit(CancellationToken.None);

            //Arrange
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);

            var filteringParameters = Any.IncomingFilteringParameters();

            var stockTransactions =
                await api.GetStocks(filteringParameters.DatePeriod.StartDate, filteringParameters.DatePeriod.EndDate.AddDays(1), productName.Value, companyName.Value, 1, 100, true, "CompanyName");
            
            api.Client.RemoveToken();
            stockTransactions.StockTransactions.Should().NotBeNull();
            stockTransactions.StockTransactions.Should().HaveCount(1);
        }

        [Fact]
        public async Task UserCanPostStock_WithValidToken()
        {
            var productRepo = webAppFixture.AppFactory.Services.GetRequiredService<IProductRepository>();
            var unitOfWork = webAppFixture.AppFactory.Services.GetRequiredService<IUnitOfWork>();
            var product = Product.Create(BusinessCentralEntities.Domain.SourceSystemId.Create(Guid.NewGuid()),
                                         ItemCategoryCode.Create("TheJuice"),
                                         BusinessCentralEntities.Domain.Products.ProductNumber.Create("9898"),
                                         Description.Create("TheJuice"),
                                         SourcesystemEtag.Create("abc"),
                                         CompanyId.Create(Guid.NewGuid()),
                                         BusinessCentralEntities.Domain.CompanyName.Create("Company"));

            await productRepo.Add(product, CancellationToken.None);
            await productRepo.SaveChanges(CancellationToken.None);
            await unitOfWork.Commit(CancellationToken.None);

            var stockRepo = webAppFixture.AppFactory.Services.GetRequiredService<IStockTransactionsRepository>();

            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            
            await api.PostStock(product.ProductNumber.Value, product.CompanyId.Value);

            api.Client.RemoveToken();
            var stockTransactions = await stockRepo.GetAllByPaging(1, 100, CancellationToken.None);            
            stockTransactions.Should().NotBeEmpty();
        }
    }
}

