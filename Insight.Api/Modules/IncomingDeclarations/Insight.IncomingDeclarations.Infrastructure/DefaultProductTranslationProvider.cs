using Insight.BuildingBlocks.Infrastructure.InitialData;
using Insight.IncomingDeclarations.Domain.Incoming;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.IncomingDeclarations.Infrastructure
{
    public class DefaultProductTranslationProvider : IDefaultDataProvider
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public DefaultProductTranslationProvider(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Populate(IDocumentStore documentStore, CancellationToken cancellation)
        {
            using var asyncScope = serviceScopeFactory.CreateAsyncScope();
            using var session = documentStore.IdentitySession();
            if (!session.Query<ProductTranslation>().Any())
            {
                var productTranslationRepository = asyncScope.ServiceProvider.GetRequiredService<IProductTranslationRepository>();
                var unitOfWork = asyncScope.ServiceProvider.GetRequiredService<BuildingBlocks.Infrastructure.IUnitOfWork>();
                await CreateProductTranslationAsync(productTranslationRepository);

                await unitOfWork.Commit(cancellation);
            }
        }
        public async Task CreateProductTranslationAsync(IProductTranslationRepository ProductTranslationRepository)
        {
            bool isDataSeeded = await ProductTranslationRepository.AnyAsync(CancellationToken.None);

            if (!isDataSeeded)
            {
                await ProductTranslationRepository.Add(ProductTranslation.Create(
                    ProductStandard.Create("HVO100"),
                    ProductDescription.Create("HVO 100 Renewable Diesel"),
                    new List<ProductVariant> {
                        ProductVariant.Create("HVO 100"),
                        ProductVariant.Create("HVO100"),
                        ProductVariant.Create("Neste MY Renewable Diesel SE"),
                        ProductVariant.Create("Neste Renewable Diesel with Additive")
                    }), CancellationToken.None);

                await ProductTranslationRepository.Add(ProductTranslation.Create(
                   ProductStandard.Create("HVO DIESEL"),
                   ProductDescription.Create("HVO DIESEL"),
                   new List<ProductVariant> {
                        ProductVariant.Create("HVO DIESEL"),
                        ProductVariant.Create("HVODIESEL")
                   }), CancellationToken.None);

                await ProductTranslationRepository.Add(ProductTranslation.Create(
                    ProductStandard.Create("B100"),
                    ProductDescription.Create("B100 100% Biodiesel"),
                    new List<ProductVariant> {
                        ProductVariant.Create("B100"),
                        ProductVariant.Create("B100 Biodiesel")}), CancellationToken.None);

                await ProductTranslationRepository.SaveChanges(CancellationToken.None);
            }
        }
    }
}
