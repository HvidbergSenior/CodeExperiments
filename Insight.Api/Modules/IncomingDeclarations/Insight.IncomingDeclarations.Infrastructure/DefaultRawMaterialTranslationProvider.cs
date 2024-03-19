using Insight.BuildingBlocks.Infrastructure.InitialData;
using Insight.IncomingDeclarations.Domain.Incoming;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.IncomingDeclarations.Infrastructure
{
    public class DefaultRawMaterialTranslationProvider : IDefaultDataProvider
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public DefaultRawMaterialTranslationProvider(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Populate(IDocumentStore documentStore, CancellationToken cancellation)
        {
            using var asyncScope = serviceScopeFactory.CreateAsyncScope();
            using var session = documentStore.IdentitySession();
            if(!session.Query<RawMaterialTranslation>().Any())
            {
                var rawMaterialTranslationRepository = asyncScope.ServiceProvider.GetRequiredService<IRawMaterialTranslationRepository>();
                var unitOfWork = asyncScope.ServiceProvider.GetRequiredService<BuildingBlocks.Infrastructure.IUnitOfWork>();
                await CreateRawMaterialTranslationAsync(rawMaterialTranslationRepository);

                await unitOfWork.Commit(cancellation);
            }
        }
        public async Task CreateRawMaterialTranslationAsync(IRawMaterialTranslationRepository rawMaterialTranslationRepository)
        {
            bool isDataSeeded = await rawMaterialTranslationRepository.AnyAsync(CancellationToken.None);
            
            if (!isDataSeeded)
            {
                await rawMaterialTranslationRepository.Add(RawMaterialTranslation.Create(
                    RawMaterialStandard.Create("ANIMAL_CAT0"),
                    RawMaterialDescription.Create("Animal fats from rendering"),
                    new List<RawMaterialVariant> {
                        RawMaterialVariant.Create("Animal fats from rendering"),
                        RawMaterialVariant.Create("Animal Fat (No Category)"),
                        RawMaterialVariant.Create("ANIMAL_CAT0")}), CancellationToken.None);

                await rawMaterialTranslationRepository.Add(RawMaterialTranslation.Create(
                    RawMaterialStandard.Create("ANIMAL_CAT1"), 
                    RawMaterialDescription.Create("Animal Fat (Category 1)"),
                    new List<RawMaterialVariant> {
                        RawMaterialVariant.Create("ANIMAL_CAT1"),
                        RawMaterialVariant.Create("Animal Fat (Category 1)")}), CancellationToken.None);

                await rawMaterialTranslationRepository.Add(RawMaterialTranslation.Create(
                    RawMaterialStandard.Create("ANIMAL_CAT2"), 
                    RawMaterialDescription.Create("Animal Fat (Category 2)"),
                    new List<RawMaterialVariant> {
                        RawMaterialVariant.Create("ANIMAL_CAT2"), 
                        RawMaterialVariant.Create("Animal Fat (Category 2)")}), CancellationToken.None);

                await rawMaterialTranslationRepository.Add(RawMaterialTranslation.Create(
                    RawMaterialStandard.Create("ANIMAL_CAT3"), 
                    RawMaterialDescription.Create("Animal Fat, waste/residue (Category 3)"),
                    new List<RawMaterialVariant> {
                        RawMaterialVariant.Create("ANIMAL_CAT3"),
                        RawMaterialVariant.Create("Animal fats from rendering (Category 3)"),
                        RawMaterialVariant.Create("Animal Fat, waste/residue (Category 3)")}), CancellationToken.None);

                await rawMaterialTranslationRepository.Add(RawMaterialTranslation.Create(
                    RawMaterialStandard.Create("CPO"), 
                    RawMaterialDescription.Create("Crude Palm Oil"),
                    new List<RawMaterialVariant> {
                        RawMaterialVariant.Create("CPO"),
                        RawMaterialVariant.Create("Crude Palm Oil")}), CancellationToken.None);

                await rawMaterialTranslationRepository.Add(RawMaterialTranslation.Create(
                    RawMaterialStandard.Create("PFAD"), 
                    RawMaterialDescription.Create("Palm Fatty Acid Distillate"),
                    new List<RawMaterialVariant> {
                        RawMaterialVariant.Create("PFAD"),
                        RawMaterialVariant.Create("Palm Fatty Acid Distillate")}), CancellationToken.None);

                await rawMaterialTranslationRepository.Add(RawMaterialTranslation.Create(
                    RawMaterialStandard.Create("POME"), 
                    RawMaterialDescription.Create("Palm Oil Mill Effluent"),
                    new List<RawMaterialVariant> {
                        RawMaterialVariant.Create("POME"),
                        RawMaterialVariant.Create("Palm Oil Mill Effluent")}), CancellationToken.None);

                await rawMaterialTranslationRepository.Add(RawMaterialTranslation.Create(
                    RawMaterialStandard.Create("RAPESEED"), 
                    RawMaterialDescription.Create("Rapeseed Oil"),
                    new List<RawMaterialVariant> {
                        RawMaterialVariant.Create("RAPESEED"),
                        RawMaterialVariant.Create("Rapeseed Oil")}), CancellationToken.None);

                await rawMaterialTranslationRepository.Add(RawMaterialTranslation.Create(
                    RawMaterialStandard.Create("SOYBEAN"), 
                    RawMaterialDescription.Create("Soybean Oil"),
                    new List<RawMaterialVariant> {
                        RawMaterialVariant.Create("SOYBEAN"),
                        RawMaterialVariant.Create("Soybean Oil")}), CancellationToken.None);

                await rawMaterialTranslationRepository.Add(RawMaterialTranslation.Create(
                    RawMaterialStandard.Create("TCO"), 
                    RawMaterialDescription.Create("Technical Corn Oil"),
                    new List<RawMaterialVariant> {
                        RawMaterialVariant.Create("TCO"),
                        RawMaterialVariant.Create("Technical Corn Oil")}), CancellationToken.None);

                await rawMaterialTranslationRepository.Add(RawMaterialTranslation.Create(
                    RawMaterialStandard.Create("UCO"),
                    RawMaterialDescription.Create("Used Cooking Oil"),
                    new List<RawMaterialVariant> {
                        RawMaterialVariant.Create("UCO"),
                        RawMaterialVariant.Create("Used Cooking Oil"),
                        RawMaterialVariant.Create("Used Cooking Oil (Vegetable)"),
                        RawMaterialVariant.Create("Used Cooking Oil (Mixed)")}), CancellationToken.None);                        

                await rawMaterialTranslationRepository.SaveChanges(CancellationToken.None);
            }
        }
    }
}
