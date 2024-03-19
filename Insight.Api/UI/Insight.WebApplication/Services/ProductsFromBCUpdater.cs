using Insight.BuildingBlocks.Infrastructure;
using Insight.BusinessCentralEntities.Domain;
using Insight.BusinessCentralEntities.Domain.Products;
using Insight.Services.BusinessCentralConnector.Service.Helpers;
using Insight.Services.BusinessCentralConnector.Service.Product;

namespace Insight.WebApplication.Services
{
    public class ProductsFromBCUpdater : BackgroundService
    {
        private readonly TimeSpan period = TimeSpan.FromHours(1);
        private readonly ILogger<ProductsFromBCUpdater> logger;
        private readonly IServiceScopeFactory factory;
        private bool firstRun = true;

        public ProductsFromBCUpdater(ILogger<ProductsFromBCUpdater> logger, IServiceScopeFactory factory)
        {
            this.logger = logger;
            this.factory = factory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(period);

            while (
                !stoppingToken.IsCancellationRequested &&
                (firstRun || await timer.WaitForNextTickAsync(stoppingToken)))
            {
                firstRun = false;
                try
                {
                    await using AsyncServiceScope asyncScope = factory.CreateAsyncScope();

                    var unitOfWork = asyncScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var productService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralProductService>();
                    var productRepository = asyncScope.ServiceProvider.GetRequiredService<IProductRepository>();
                    var productsFromBC = await productService.GetAllAsync(stoppingToken);

                    foreach (var bcProduct in productsFromBC)
                    {
                        if (string.IsNullOrWhiteSpace(bcProduct.ItemCategoryCode))
                        {
                            continue;
                        }

                        var productFromDB = await productRepository.GetProductBySystemId(SourceSystemId.Create(bcProduct.SystemId), stoppingToken);
                        if (productFromDB == null)
                        {
                            logger.LogInformation("Added product {Product}", bcProduct.ItemCategoryCode);
                            await productRepository.Add(bcProduct.ToProduct(), stoppingToken);
                        }
                        else
                        {
                            if (productFromDB.SourcesystemEtag.Value == bcProduct.Etag)
                            {
                                continue;
                            }
                            logger.LogInformation("Updated product {Product}", bcProduct.ItemCategoryCode);
                            var tempProduct = bcProduct.ToProduct();

                            productFromDB.Update(tempProduct.ItemCategoryCode, tempProduct.ProductNumber, tempProduct.Description, tempProduct.SourcesystemEtag);
                            await productRepository.Update(productFromDB, stoppingToken);
                        }
                    }
                    await productRepository.SaveChanges(stoppingToken);
                    await unitOfWork.Commit(stoppingToken);
                }
                catch (Exception ex) when (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning(ex, "Execution Cancelled");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Unhandeled exception. Execution Stopping");
                }
            }
        }
    }
}
