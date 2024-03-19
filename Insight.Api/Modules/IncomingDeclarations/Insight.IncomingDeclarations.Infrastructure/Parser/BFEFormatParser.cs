using Ganss.Excel;
using Insight.BusinessCentralEntities.Domain.Companies;
using Insight.BusinessCentralEntities.Domain.Products;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Domain.Parsing;

namespace Insight.IncomingDeclarations.Infrastructure.Parser
{
    public class BFEFormatParser : IIncomingDeclarationParser
    {
        private const string SHEET_NAME = "Insight 2.0 standard template";
        private readonly ICompanyRepository companyRepository;
        private readonly IProductTranslationRepository productTranslationRepository;
        private readonly IRawMaterialTranslationRepository rawMaterialTranslationRepository;

        public BFEFormatParser(ICompanyRepository companyRepository, IProductTranslationRepository productTranslationRepository, IRawMaterialTranslationRepository rawMaterialTranslationRepository)
        {
            this.companyRepository = companyRepository;
            this.productTranslationRepository = productTranslationRepository;
            this.rawMaterialTranslationRepository = rawMaterialTranslationRepository ?? throw new ArgumentNullException(nameof(rawMaterialTranslationRepository));
        }

        public bool CanParseDocument(IncomingDeclarationSupplier declarationSupplier)
        {
            return declarationSupplier == IncomingDeclarationSupplier.BFE;
        }

        public async Task<(IReadOnlyCollection<ColumnParsingError> errors, IEnumerable<IncomingDeclaration> declarations)> ParseDeclarationDocumentAsync(Stream document, CancellationToken cancellationToken)
        {
            var startRow = 2;
            var mapper = new ExcelMapper() { HeaderRow = false, MinRowNumber = startRow };
            var errors = new List<ColumnParsingError>();
            var declarations = new List<IncomingDeclaration>();
            bool errorInRow = false;
            ColumnParsingError currentError = ColumnParsingError.Empty();

            mapper.ErrorParsingCell += (sender, args) =>
            {
                currentError = ColumnParsingError.Create(args.Error.Line, args.Error.Column, string.Empty, args.Error.Message);
                args.Cancel = true;
                errorInRow = true;
            };
            var rowNumber = startRow + 1;

            var companies = await companyRepository.GetAllCompanies(cancellationToken);
            var validProductVariants = await productTranslationRepository.GetProductVariantsAsync(cancellationToken);            
            var validRawMaterialVariants = await rawMaterialTranslationRepository.GetRawMaterialVariantsAsync(cancellationToken);
            var rawMaterialTranslations = await rawMaterialTranslationRepository.GetAllAsync(cancellationToken);
            var productTranslations = await productTranslationRepository.GetAllAsync(cancellationToken);

            IncomingDeclarationValidator validator = new IncomingDeclarationValidator(companies, validProductVariants, validRawMaterialVariants);

            foreach (var declaration in await mapper.FetchAsync<BFEFormat>(document, SHEET_NAME))
            {
                try
                {
                    if (errorInRow)
                    {
                        errors.Add(ColumnParsingError.Create(currentError.Row, currentError.Column, declaration.PoSNumber ?? "MISSING-MISSING", currentError.ErrorMessage));

                        errorInRow = false;
                        currentError = ColumnParsingError.Empty();
                        rowNumber++;
                        continue;
                    }

                    var validationResult = await validator.ValidateAsync(declaration.ToIncomingDeclaration(rawMaterialTranslations, productTranslations), cancellationToken);

                    if(!validationResult.IsValid)
                    {
                        var errorMessages = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));
                        
                        errors.Add(ColumnParsingError.Create(rowNumber, 0, declaration.PoSNumber ?? "MISSING-MISSING", errorMessages));
                        
                        rowNumber++;
                        continue;
                    }

                    declaration.RowNumber = rowNumber;
                    declarations.Add(declaration.ToIncomingDeclaration(rawMaterialTranslations, productTranslations));
                }
                catch (Exception ex)
                {
                    errors.Add(ColumnParsingError.Create(rowNumber, 0, "MISSING-MISSING", ex.Message));
                    continue;
                }
                rowNumber++;
            }
            return (errors, declarations);
        }
    }
}
