using FluentAssertions;
using Insight.BusinessCentralEntities.Domain;
using Insight.BusinessCentralEntities.Domain.Companies;
using Insight.BusinessCentralEntities.Domain.Products;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Infrastructure.Incoming;
using Insight.IncomingDeclarations.Infrastructure.Parser;
using Moq;
using Company = Insight.BusinessCentralEntities.Domain.Companies.Company;

namespace Insight.IncomingDeclarations.Test
{
    public class ParserTests
    {
        private readonly IEnumerable<Company> companies = new List<Company>
        {
            Company.Create(CompanyId.Create(Guid.Empty), CompanyName.Create("Biofuel Express AB"),
                SourcesystemEtag.Create("")),
            Company.Create(CompanyId.Create(Guid.Empty), CompanyName.Create("Biofuel Express DMCC"),
                SourcesystemEtag.Create("")),

        };

        private readonly IEnumerable<ItemCategoryCode> productCategoryCodes = new List<ItemCategoryCode>
        {
            ItemCategoryCode.Create("B100"),
            ItemCategoryCode.Create("Neste MY Renewable Diesel SE"),
            ItemCategoryCode.Create("Neste Renewable Diesel with Additive")
        };

        [Fact]
        public async Task MakeSureWeCanParseBFEFile()
        {
            //Arrange
            var mock = new Mock<IIncomingDeclarationRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(c => c.GetAllCompanies(It.IsAny<CancellationToken>())).ReturnsAsync(companies);
            var productRepositoryMock = new Mock<IProductRepository>();                       
            productRepositoryMock.Setup(c => c.GetItemCategoryCodes(It.IsAny<CancellationToken>()))
                .ReturnsAsync(productCategoryCodes);
            var rawMaterialTranslationRepository = RawMaterialTranslationInMemoryRepository.CreateWithSeededData();
            var productTranslationRepository = ProductTranslationInMemoryRepository.CreateWithSeededData();
            var sut = new BFEFormatParser(companyRepositoryMock.Object, productTranslationRepository, rawMaterialTranslationRepository);
            
            await using var fs = new FileStream("Resources/PoSIncoming.xlsx", FileMode.Open);

            //Act
            var (errors, declarations) = await sut.ParseDeclarationDocumentAsync(fs, CancellationToken.None);

            //Assert
            errors.Should().BeEmpty();
            declarations.Should().NotBeEmpty();
        }

        [Fact]
        public async Task MakeSureWeCanParseBFEFileAndGetErrorFeedback()
        {
            //Arrange
            var mock = new Mock<IIncomingDeclarationRepository>();
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(c => c.GetAllCompanies(It.IsAny<CancellationToken>())).ReturnsAsync(companies);
            var productRepositoryMock = new Mock<IProductRepository>();            
            productRepositoryMock.Setup(c => c.GetItemCategoryCodes(It.IsAny<CancellationToken>()))
                .ReturnsAsync(productCategoryCodes);
            var rawMaterialTranslationRepository = RawMaterialTranslationInMemoryRepository.CreateWithSeededData();
            var productTranslationRepository = ProductTranslationInMemoryRepository.CreateWithSeededData();
            var sut = new BFEFormatParser(companyRepositoryMock.Object, productTranslationRepository, rawMaterialTranslationRepository);
            using var fs = new FileStream("Resources/PoSWithError.xlsx", FileMode.Open);

            //Act
            var (errors, declarations) = await sut.ParseDeclarationDocumentAsync(fs, CancellationToken.None);

            //Arrange
            errors.Should().HaveCount(2);
            declarations.Should().HaveCount(1);
        }

        [Fact]
        public async Task MakeSureWeCanParseNesteFileDKCulture()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("da-DK");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("da-DK");
            //Arrange
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(c => c.GetAllCompanies(It.IsAny<CancellationToken>())).ReturnsAsync(companies);
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(c => c.GetItemCategoryCodes(It.IsAny<CancellationToken>()))
                .ReturnsAsync(productCategoryCodes);
            var rawMaterialTranslationRepository = RawMaterialTranslationInMemoryRepository.CreateWithSeededData();
            var productTranslationRepository = ProductTranslationInMemoryRepository.CreateWithSeededData();
            var sut = new NesteFormatParserExcelDataReader(
                companyRepositoryMock.Object,
                productTranslationRepository, 
                rawMaterialTranslationRepository);
            using var fs = new FileStream("Resources/PoS - Neste.xlsx", FileMode.Open);

            //Act
            var (errors, declarations) = await sut.ParseDeclarationDocumentAsync(fs, CancellationToken.None);

            //Assert
            errors.Should().BeEmpty();
            declarations.Should().HaveCount(147);
            var firstDeclaration = declarations.First();
            firstDeclaration.GhgEmissionSaving.Value.Should().BeInRange(0.0M, 0.99M);
        }

        [Fact]
        public async Task MakeSureWeCanParseNesteFileUSCulture()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            //Arrange
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(c => c.GetAllCompanies(It.IsAny<CancellationToken>())).ReturnsAsync(companies);
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(c => c.GetItemCategoryCodes(It.IsAny<CancellationToken>()))
                .ReturnsAsync(productCategoryCodes);
            var rawMaterialTranslationRepository = RawMaterialTranslationInMemoryRepository.CreateWithSeededData();
            var productTranslationRepository = ProductTranslationInMemoryRepository.CreateWithSeededData();
            var sut = new NesteFormatParserExcelDataReader(
                companyRepositoryMock.Object,
                productTranslationRepository,
                rawMaterialTranslationRepository);
            using var fs = new FileStream("Resources/PoS - Neste.xlsx", FileMode.Open);

            //Act
            var (errors, declarations) = await sut.ParseDeclarationDocumentAsync(fs, CancellationToken.None);

            //Assert
            errors.Should().BeEmpty();
            declarations.Should().HaveCount(147);
            var firstDeclaration = declarations.First();
            firstDeclaration.GhgEmissionSaving.Value.Should().BeInRange(0.0M, 0.99M);
        }


        [Fact]
        public async Task MakeSureWeCanParseNesteFileAndGetErrorFeedback()
        {
            //Arrange
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(c => c.GetAllCompanies(It.IsAny<CancellationToken>())).ReturnsAsync(companies);
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(c => c.GetItemCategoryCodes(It.IsAny<CancellationToken>()))
                .ReturnsAsync(productCategoryCodes);
            var rawMaterialTranslationRepository = RawMaterialTranslationInMemoryRepository.CreateWithSeededData();
            var productTranslationRepository = ProductTranslationInMemoryRepository.CreateWithSeededData();
            var sut = new NesteFormatParserExcelDataReader(
                companyRepositoryMock.Object,
                productTranslationRepository,
                rawMaterialTranslationRepository);
            using var fs = new FileStream("Resources/PoS - NesteWithError.xlsx", FileMode.Open);

            //Act
            var (errors, declarations) = await sut.ParseDeclarationDocumentAsync(fs, CancellationToken.None);

            //Assert
            errors.Should().HaveCount(4);
            declarations.Should().HaveCount(143);
        }
    }
}