using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.IncomingDeclarations.Domain.Parsing
{
    public interface IIncomingDeclarationParser
    {
        bool CanParseDocument(IncomingDeclarationSupplier declarationSupplier);
        Task<(IReadOnlyCollection<ColumnParsingError> errors, IEnumerable<IncomingDeclaration> declarations)> ParseDeclarationDocumentAsync(Stream document, CancellationToken cancellationToken);
    }
}