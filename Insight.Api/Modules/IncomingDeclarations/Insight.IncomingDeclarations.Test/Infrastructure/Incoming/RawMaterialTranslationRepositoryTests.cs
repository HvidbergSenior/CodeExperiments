using FluentAssertions;
using Insight.IncomingDeclarations.Infrastructure.Incoming;
using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.IncomingDeclarations.Test.Infrastructure.Incoming
{
    public class RawMaterialTranslationInMemoryRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_ReturnsAllEntities_WhenRepositoryIsNotEmpty()
        {
            // Arrange
            var repository = new RawMaterialTranslationInMemoryRepository();
            var expectedTranslations = RawMaterialTranslation.Create(
                RawMaterialStandard.Create("UCO"),
                RawMaterialDescription.Create("Used Cooking Oil"),
                new List<RawMaterialVariant>() {
                    RawMaterialVariant.Create("UCO"),
                    RawMaterialVariant.Create("Used Cooking Oil"),
                    RawMaterialVariant.Create("Used Cooking Oil (mixed)")
                });

            await repository.Add(expectedTranslations);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Single().RawMaterialStandard.Should().Be(expectedTranslations.RawMaterialStandard);
            result.Single().RawMaterialVariants.Should().HaveCount(expectedTranslations.RawMaterialVariants.Count)
                   .And.BeEquivalentTo(expectedTranslations.RawMaterialVariants);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmptyList_WhenRepositoryIsEmpty()
        {
            // Arrange
            var repository = new RawMaterialTranslationInMemoryRepository();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }
    }
}