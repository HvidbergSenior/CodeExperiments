using FluentAssertions;
using Insight.BuildingBlocks.Fakes;
using Insight.BusinessCentralEntities.Domain.Companies;
using Insight.IncomingDeclarations.Application.UploadIncomingDeclaration;
using Insight.IncomingDeclarations.Domain.Parsing;
using Insight.IncomingDeclarations.Infrastructure.Incoming;
using Insight.IncomingDeclarations.Infrastructure.Parser;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Insight.IncomingDeclarations.Test.Application.UploadIncomingDeclaration;

public class UploadIncomingDeclarationTests
{
    [Fact]
    public async Task Should_upload_incoming_declaration()
    {
        //Arrange
        var incomingDeclarationInMemoryRepository = new IncomingDeclarationInMemoryRepository();
        var unitOfWork = new FakeUnitOfWork();
        var companyRepositoryMock = new Mock<ICompanyRepository>();
        var rawMaterialTranslationRepository = RawMaterialTranslationInMemoryRepository.CreateWithSeededData();
        var productTranslationRepository = ProductTranslationInMemoryRepository.CreateWithSeededData();
        var bfeParser = new BFEFormatParser(companyRepositoryMock.Object, productTranslationRepository, rawMaterialTranslationRepository);
        var parsers = new List<IIncomingDeclarationParser> { bfeParser };

        byte[] fileContent;

        await using (var fs = new FileStream("Resources/PoSUpload.xlsx", FileMode.Open))
        {
            fileContent = new byte[fs.Length];
            fs.Read(fileContent, 0, (int)fs.Length);
        }

        var file = new FormFile(new MemoryStream(fileContent), 0, fileContent.Length, "Data", "example.xlsx");
        var query = UploadIncomingDeclarationCommand.Create(file, IncomingDeclarationSupplier.BFE);

        var handler = new UploadIncomingDeclarationCommandHandler(incomingDeclarationInMemoryRepository, unitOfWork, parsers);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
    }
}