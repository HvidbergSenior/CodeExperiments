using FluentAssertions;
using Insight.IncomingDeclarations.Domain.Incoming;

/*
result.Add(RawMaterialTranslation.Create(RawMaterialStandard.Create("Used Cooking Oil"), [
                                            RawMaterialVariant.Create("Used Cooking Oil"),
    RawMaterialVariant.Create("Used Cooking Oil (Vegetable)"),
    RawMaterialVariant.Create("Used Cooking Oil (Mixed)")]));
*/

namespace Insight.IncomingDeclarations.Test.Domain
{
    public class RawMaterialTests
    {
        [Fact]
        public void TranslateOrDefault_ShouldReturnTranslatedValue_WhenMatchFound()
        {
            // Arrange
            var rawMaterialStandard = "Used Cooking Oil";
            var rawMaterialDescription = "Used Cooking Oil (Vegetable)";
            var rawMaterialTranslations = new List<RawMaterialTranslation>
            {
                RawMaterialTranslation.Create(
                    RawMaterialStandard.Create(rawMaterialStandard),
                    RawMaterialDescription.Create(rawMaterialDescription),
                    RawMaterialVariant.Create(rawMaterialDescription)
                    )
            };

            // Act
            var result = RawMaterialTranslations.TranslateOrDefault(rawMaterialTranslations, rawMaterialDescription);

            // Assert
            result.Should().NotBeNull();
            result.Value.Should().Be(rawMaterialStandard); 
        }

        [Fact]
        public void TranslateOrDefault_ShouldReturnDefaultValue_WhenNoMatchFound()
        {
            // Arrange
            var defaultRawMaterialText = "material (unknown)";

            var rawMaterialVariant  = "Used Cooking Oil (Vegetable)";
            var rawMaterialDescription = "Used Cooking Oil (Vegetable)";
            var rawMaterialStandard = "Used Cooking Oil";
            var rawMaterialTranslations = new List<RawMaterialTranslation>
            {
                RawMaterialTranslation.Create(
                    RawMaterialStandard.Create(rawMaterialStandard),
                    RawMaterialDescription.Create(rawMaterialDescription),
                    RawMaterialVariant.Create(rawMaterialVariant)
                    )
            };

            // Act
            var result = RawMaterialTranslations.TranslateOrDefault(rawMaterialTranslations, defaultRawMaterialText);

            // Assert
            result.Should().NotBeNull();
            result.Value.Should().NotBe(rawMaterialVariant); 
            result.Value.Should().Be(defaultRawMaterialText); 
        }

        [Fact]
        public void TranslateOrDefault_ShouldReturnTranslatedValue_WhenCaseInsensitivMatchFound()
        {
            // Arrange
            var rawMaterialStandard     = "Used Cooking Oil";
            var givenRawMaterialVariant = "used cooking oil (VEGETABLE)";
            var rawMaterialVariant      = "Used Cooking Oil (Vegetable)";
            var rawMaterialTranslations = new List<RawMaterialTranslation>
            {
                RawMaterialTranslation.Create(
                    RawMaterialStandard.Create(rawMaterialStandard),
                    RawMaterialDescription.Create(rawMaterialVariant),
                    RawMaterialVariant.Create(rawMaterialVariant)
                    )
            };

            // Act
            var result = RawMaterialTranslations.TranslateOrDefault(rawMaterialTranslations, givenRawMaterialVariant);

            // Assert
            result.Should().NotBeNull();
            result.Value.Should().NotBe(givenRawMaterialVariant);
            result.Value.Should().Be(rawMaterialStandard);
        }
    }
}