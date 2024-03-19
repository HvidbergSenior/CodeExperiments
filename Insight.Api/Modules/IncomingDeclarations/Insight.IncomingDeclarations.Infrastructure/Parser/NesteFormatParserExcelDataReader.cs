using System.Data;
using System.Globalization;
using System.Text;
using ExcelDataReader;
using Insight.BusinessCentralEntities.Domain.Companies;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Domain.Parsing;

namespace Insight.IncomingDeclarations.Infrastructure.Parser
{
    public class NesteFormatParserExcelDataReader : IIncomingDeclarationParser
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IProductTranslationRepository productTranslationRepository;
        private readonly IRawMaterialTranslationRepository rawMaterialTranslationRepository;

        public NesteFormatParserExcelDataReader(ICompanyRepository companyRepository, IProductTranslationRepository productTranslationRepository, IRawMaterialTranslationRepository rawMaterialTranslationRepository)
        {
            this.companyRepository = companyRepository;
            this.productTranslationRepository = productTranslationRepository;
            this.rawMaterialTranslationRepository = rawMaterialTranslationRepository ?? throw new ArgumentNullException(nameof(rawMaterialTranslationRepository));
        }

        public bool CanParseDocument(IncomingDeclarationSupplier declarationSupplier)
        {
            return declarationSupplier == IncomingDeclarationSupplier.Neste;
        }

        public async Task<(IReadOnlyCollection<ColumnParsingError> errors, IEnumerable<IncomingDeclaration> declarations)> ParseDeclarationDocumentAsync(Stream document, CancellationToken cancellationToken)
        {
            var errors = new List<ColumnParsingError>();
            var declarations = new List<IncomingDeclaration>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Required once for ExcelDataReader to work

            using (var reader = ExcelReaderFactory.CreateReader(document))
            {
                var result = reader.AsDataSet();
                
                var companies = await companyRepository.GetAllCompanies(cancellationToken);
                var productVariants = await productTranslationRepository.GetProductVariantsAsync(cancellationToken);
                var validRawMaterialVariants = await rawMaterialTranslationRepository.GetRawMaterialVariantsAsync(cancellationToken);

                IncomingDeclarationValidator validator = new IncomingDeclarationValidator(companies, productVariants, validRawMaterialVariants);
                var rawMaterialTranslations = await rawMaterialTranslationRepository.GetAllAsync(cancellationToken);
                var productTranslations = await productTranslationRepository.GetAllAsync(cancellationToken);
                
                foreach(DataTable table in result.Tables)
                {
                    if(table.TableName.Contains("hidden", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    var startRow = 13;
                    var currentRow = 0;

                    foreach (DataRow row in table.Rows)
                    {
                        if (currentRow < startRow)
                        {
                            currentRow++;
                            continue;
                        }

                        try
                        {
                            NesteFormat record = RowToDeclaration(row, currentRow);

                            var dec = record.ToIncomingDeclaration(rawMaterialTranslations, productTranslations);

                            var validationResult = await validator.ValidateAsync(dec, cancellationToken);

                            if (!validationResult.IsValid)
                            {
                                var errorMessages = string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage));

                                errors.Add(ColumnParsingError.Create(currentRow, 0, record.PoSNumber ?? "MISSING-MISSING", errorMessages));

                                currentRow++;
                                continue;
                            }

                            declarations.Add(record.ToIncomingDeclaration(rawMaterialTranslations, productTranslations));
                        }
                        catch (Exception ex)
                        {
                            var posNumber = row[14].ToString();
                            var posTicketNumber = row[15].ToString();

                            var posConcat = $"{posNumber ?? "MISSING"}-{posTicketNumber ?? "MISSING"}";

                            errors.Add(ColumnParsingError.Create(currentRow + 1, 0, posConcat, ex.Message));
                        }
                        currentRow++;
                    }
                }
            }

            return (errors.AsReadOnly(), declarations);
        }

        private static NesteFormat RowToDeclaration(DataRow row, int rowNumber)
        {
            //Thread.CurrentThread.CurrentCulture <- ExcelDataReader handles the decimal parsings.

            return new NesteFormat()
            {
                Supplier = row[2].ToString()!,
                PlaceOfDispatch = row[5].ToString()!,
                CountryString = row[8].ToString()!,
                Company = row[11].ToString()!,
                DateOfDispatchString = row[12].ToString()!,
                PoSNumber = row[14].ToString()!,
                PoSTicketNumber = row[15].ToString()!,
                Product = row[7].ToString()!,
                RawMaterial = row[17].ToString()!,
                CountryOfOrigin = row[18].ToString()!,
                ProductionCountry = row[19].ToString()!,
                CertificationSystem = row[20].ToString()!,
                SupplierCertificateNumber = row[21].ToString()!,
                QuantityInLitres = decimal.Parse(row[27].ToString()!, Thread.CurrentThread.CurrentCulture),
                GHGEmissionSaving = decimal.Parse(row[28].ToString()!, Thread.CurrentThread.CurrentCulture),
                GHGEee = decimal.Parse(row[29].ToString()!, Thread.CurrentThread.CurrentCulture),
                GHGEccr = decimal.Parse(row[30].ToString()!, Thread.CurrentThread.CurrentCulture),
                GHGEccs = decimal.Parse(row[31].ToString()!, Thread.CurrentThread.CurrentCulture),
                GHGEsca = decimal.Parse(row[32].ToString()!, Thread.CurrentThread.CurrentCulture),
                GHGDistribution = decimal.Parse(row[33].ToString()!, Thread.CurrentThread.CurrentCulture),
                GHGTransport = decimal.Parse(row[34].ToString()!, Thread.CurrentThread.CurrentCulture),
                GHGEp = decimal.Parse(row[35].ToString()!, Thread.CurrentThread.CurrentCulture),
                GHGEec = decimal.Parse(row[37].ToString()!, Thread.CurrentThread.CurrentCulture),
                GHGgCO2eqMJ = decimal.Parse(row[38].ToString()!, Thread.CurrentThread.CurrentCulture),
                BioAllocQty = decimal.Parse(row[24].ToString()!, Thread.CurrentThread.CurrentCulture),
                BioQuantityNL = decimal.Parse(row[25].ToString()!, Thread.CurrentThread.CurrentCulture),
                RowNumber = rowNumber
            };
        }
    }
}
