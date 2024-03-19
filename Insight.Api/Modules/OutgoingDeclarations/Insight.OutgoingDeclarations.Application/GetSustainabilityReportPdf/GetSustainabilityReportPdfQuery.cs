using System.Globalization;
using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Integration.GetGroupedTransactionsQuery;
using Insight.IncomingDeclarations.Integration.GetIncomingDeclarationsByIds;
using Insight.OutgoingDeclarations.Application.Helpers;
using Insight.OutgoingDeclarations.Domain;
using Insight.OutgoingDeclarations.Domain.SustainabilityReports;
using DatePeriod = Insight.BuildingBlocks.Domain.DatePeriod;

namespace Insight.OutgoingDeclarations.Application.GetSustainabilityReportPdf
{
    public sealed class GetSustainabilityReportPdfQuery : IQuery<GetSustainabilityReportPdfResponse>
    {
        public SustainabilityAndFuelConsumptionFilteringParameters SustainabilityAndFuelConsumptionFilteringParameters { get; private set; }

        private GetSustainabilityReportPdfQuery(SustainabilityAndFuelConsumptionFilteringParameters sustainabilityAndFuelConsumptionFilteringParameters)
        {
            SustainabilityAndFuelConsumptionFilteringParameters = sustainabilityAndFuelConsumptionFilteringParameters;
        }

        public static GetSustainabilityReportPdfQuery Create(
            SustainabilityAndFuelConsumptionFilteringParameters sustainabilityAndFuelConsumptionFilteringParameters)
        {
            return new GetSustainabilityReportPdfQuery(sustainabilityAndFuelConsumptionFilteringParameters);
        }
    }

    public sealed class GetSustainabilityReportPdfHandler : IQueryHandler<GetSustainabilityReportPdfQuery, GetSustainabilityReportPdfResponse>
    {
        private readonly IOutgoingDeclarationRepository outgoingDeclarationsRepository;
        private readonly IQueryBus queryBus;

        public GetSustainabilityReportPdfHandler(IOutgoingDeclarationRepository outgoingDeclarationsRepository, IQueryBus queryBus)
        {
            this.outgoingDeclarationsRepository = outgoingDeclarationsRepository;
            this.queryBus = queryBus;
        }
        public async Task<GetSustainabilityReportPdfResponse> Handle(GetSustainabilityReportPdfQuery request,
            CancellationToken cancellationToken)
        {
            //OutgoingDeclarations from this module
            var outgoingDeclarations = await outgoingDeclarationsRepository.GetOutgoingDeclarationsForMany(
                FilteringParametersSelectMany.Create(request.SustainabilityAndFuelConsumptionFilteringParameters.DatePeriod,
                    request.SustainabilityAndFuelConsumptionFilteringParameters.ProductNames,
                    request.SustainabilityAndFuelConsumptionFilteringParameters.CustomerNumbers));

            //Affected Incoming Declarations from IncomingDeclarations module
            var affectedIncomingDeclarationsIds = outgoingDeclarations.SelectMany(decl => decl.IncomingDeclarationPairings.Select(pairing => pairing.IncomingDeclarationId.Value)).ToList();
            
            var incomingDeclarationsByIdsQuery = GetIncomingDeclarationsByIdsQuery.Create(affectedIncomingDeclarationsIds);
     
            var affectedIncomingDeclarations = await queryBus.Send<GetIncomingDeclarationsByIdsQuery, GetIncomingDeclarationsDto>(incomingDeclarationsByIdsQuery, cancellationToken);
            
            //FuelTransactions from FuelTransactions module
            var groupedFuelTransactions = GetGroupedFuelTransactionsQuery.Create(request.SustainabilityAndFuelConsumptionFilteringParameters);
     
            var groupedFuelTransactionsDto = await queryBus.Send<GetGroupedFuelTransactionsQuery, GetGroupedFuelTransactionsDto>(groupedFuelTransactions, cancellationToken);
            
            var responsePayLoad = await GetResponsePayload(request, groupedFuelTransactionsDto, outgoingDeclarations, affectedIncomingDeclarations);

            return responsePayLoad;
        }

        private static Task<GetSustainabilityReportPdfResponse> GetResponsePayload(GetSustainabilityReportPdfQuery request, GetGroupedFuelTransactionsDto fuelTransactions, IEnumerable<OutgoingDeclaration> outgoingDeclarations, GetIncomingDeclarationsDto affectedIncomingDeclarations)
        {
            var consumptionStats = FuelConsumptionHelper.GetConsumptionStats(fuelTransactions);

            var pdfReportPosResponses = new List<PdfReportPosResponse>();

            foreach (var outgoingDeclaration in outgoingDeclarations)
            {
                foreach (var incomingDeclarationPairing in outgoingDeclaration.IncomingDeclarationPairings)
                {
                    var incomingDeclaration = affectedIncomingDeclarations.IncomingDeclarations.First(c => c.IncomingDeclarationId == incomingDeclarationPairing.IncomingDeclarationId.Value);
                    incomingDeclaration.BatchId = incomingDeclarationPairing.BatchId.Value;
                    
                    pdfReportPosResponses.Add(new PdfReportPosResponse()
                    {
                        Recipient = SustainabilityReportPdfHelper.GetRecipient(outgoingDeclarations, incomingDeclaration),
                        Declarationinfo = SustainabilityReportPdfHelper.GetDeclarationInfo(incomingDeclaration, outgoingDeclaration),
                        //TODO: Fuel supplier. How to find address on that?
                        Renewablefuelsupplier = SustainabilityReportPdfHelper.GetRenewableFuelSupplier(outgoingDeclarations, incomingDeclaration),
                        Greenhousegasemissionssavings = SustainabilityReportPdfHelper.GetGreenhouseGasEmissionsSavings(incomingDeclaration),
                        Lifecyclegreenhousegasemissions = SustainabilityReportPdfHelper.GetLifeCycleGreenhouseGasEmissions(incomingDeclaration),
                        Rawmaterialsustainability = SustainabilityReportPdfHelper.GetRawMaterialSustainability(incomingDeclaration),
                        Scopeofcertificationandghgemission = SustainabilityReportPdfHelper.GetScopeOfCertificationAndGhgEmission(incomingDeclaration),
                        Scopeofcertificationofrawmaterial = SustainabilityReportPdfHelper.GetScopeOfCertificationOfRawMaterial(incomingDeclaration),
                        Renewablefuel = SustainabilityReportPdfHelper.GetRenewableFuel(incomingDeclaration, incomingDeclarationPairing.Quantity),
                    });
                }
            }

            return Task.FromResult(new GetSustainabilityReportPdfResponse()
            {
                //From fuel consumption
                ConsumptionStats = consumptionStats,
                ConsumptionPerProduct = FuelConsumptionHelper.GetConsumptionPerProduct(fuelTransactions.GroupedFuelTransactionDto),
                ConsumptionDevelopment = FuelConsumptionHelper.GetConsumptionDevelopment(fuelTransactions.GroupedFuelTransactionDto, request.SustainabilityAndFuelConsumptionFilteringParameters.MaxColumns),

                EmissionsStats = SustainabilityReportPdfHelper.GetEmissionStats(outgoingDeclarations, affectedIncomingDeclarations),
                Progress = SustainabilityReportPdfHelper.GetProgress(outgoingDeclarations, affectedIncomingDeclarations),
                Feedstocks = SustainabilityReportPdfHelper.GetFeedstocks(outgoingDeclarations, affectedIncomingDeclarations),
                Countries = SustainabilityReportPdfHelper.GetCountries(outgoingDeclarations, affectedIncomingDeclarations),
                ProductSpecificationItems = SustainabilityReportPdfHelper.GetProductSpecificationItems(outgoingDeclarations, affectedIncomingDeclarations),

                PdfReportPosResponses = pdfReportPosResponses,
            });
        }

        public static FuelConsumptionData GetFuelConsumptionData(IReadOnlyList<OutgoingFuelTransaction> fuelTransactions)
        {
            var renewableQuantity = fuelTransactions
                .Where(transaction => RenewalProductNames.RENEWALPRODUCTNAMES.Contains(transaction.ProductName.Value))
                .Sum(transaction => transaction.Quantity.Value);

            var fossilFuelQuantity = fuelTransactions
                .Where(transaction => !RenewalProductNames.RENEWALPRODUCTNAMES.Contains(transaction.ProductName.Value))
                .Sum(transaction => transaction.Quantity.Value);

            var fuelConsumptionCategoryQuantities = fuelTransactions
                .GroupBy(transaction => transaction.ProductName.Value.ToUpper())
                .Select(group => new NameValuePair(group.Key, group.Sum(transaction => transaction.Quantity.Value)))
                .ToArray();

            var totalFuelConsumption = fossilFuelQuantity + renewableQuantity;

            return new FuelConsumptionData
            {
                RenewableQuantity = renewableQuantity,
                FossilFuelQuantity = fossilFuelQuantity,
                TotalFuelConsumption = totalFuelConsumption,
                FuelConsumptionCategoryQuantities = fuelConsumptionCategoryQuantities
            };
        }

        public static ConsumptionDevelopmentData GetConsumptionDevelopmentData(IEnumerable<OutgoingFuelTransaction> fuelTransactions, DatePeriod toAndFromDates)
        {
            var fuelTransactionsGrouped = fuelTransactions
                .GroupBy(transaction => new { transaction.ProductName, transaction.FuelTransactionYear, transaction.FuelTransactionMonth })
                .ToList();

            var products = fuelTransactionsGrouped.SelectMany(c => c.Select(o => o.ProductName.Value)).Distinct();

            var categories = new List<string>();

            var dataDictionary = new Dictionary<string, List<decimal>>();

            var currentMonth = toAndFromDates.StartDate.Month;

            for (var currentYear = toAndFromDates.StartDate.Year; currentYear <= toAndFromDates.EndDate.Year; currentYear++)
            {
                if (currentYear == toAndFromDates.EndDate.Year && currentMonth > toAndFromDates.EndDate.Month)
                    break;

                for (; currentMonth <= 12; currentMonth++)
                {
                    categories.Add(new DateOnly(currentYear, currentMonth, 1).ToString("MMM yy", new CultureInfo("da-DK")));
                    foreach (var product in products)
                    {
                        if (!dataDictionary.ContainsKey(product))
                            dataDictionary.Add(product, new List<decimal>());

                        var thisMonthsTransactions = fuelTransactionsGrouped.Where(c =>
                            c.Key.FuelTransactionYear.Value == currentYear && c.Key.FuelTransactionMonth.Value == currentMonth && c.Key.ProductName.Value == product).SelectMany(fuelTransaction => fuelTransaction);

                        dataDictionary[product].Add(thisMonthsTransactions.Sum(c => c.Quantity.Value));
                    }
                }
                currentMonth = 1;
            }

            var seriesData = dataDictionary.Select(c => new Series() { Name = c.Key, Data = c.Value.Select(o => Convert.ToInt32(o)).ToList() });

            return new ConsumptionDevelopmentData
            {
                Categories = categories,
                Series = seriesData,
                Products = products
            };
        }

    }

    public class FuelConsumptionData
    {
        public int RenewableQuantity { get; set; }
        public int FossilFuelQuantity { get; set; }
        public decimal TotalFuelConsumption { get; set; }
        public NameValuePair[] FuelConsumptionCategoryQuantities { get; set; }

        public FuelConsumptionData()
        {
            FossilFuelQuantity = default;
            RenewableQuantity = default;
            TotalFuelConsumption = default;
            FuelConsumptionCategoryQuantities = Array.Empty<NameValuePair>();
        }
    }
    public class ConsumptionDevelopmentData
    {
        public List<string> Categories { get; set; }
        public IEnumerable<Series> Series { get; set; }
        public IEnumerable<string> Products { get; set; }

        public ConsumptionDevelopmentData()
        {
            Categories = new List<string>();
            Series = new List<Series>();
            Products = new List<string>();
        }
    }
    internal class GetSustainabilityReportPdfQueryAuthorizer : IAuthorizer<GetSustainabilityReportPdfQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetSustainabilityReportPdfQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetSustainabilityReportPdfQuery query,
            CancellationToken cancellation)
        {
            if (await executionContext.GetAdminPrivileges(cancellation))
            {
                return AuthorizationResult.Succeed();
            }
            
            if (!query.SustainabilityAndFuelConsumptionFilteringParameters.CustomerIds.Any())
            {
                return AuthorizationResult.Fail();    
            }
            
            var affectedCustomerIds = query.SustainabilityAndFuelConsumptionFilteringParameters.CustomerIds;
            var myCustomerIds = (await executionContext.GetCustomersPermissionsAsync()).Select(c=> c.CustomerId)!;
            
            if (affectedCustomerIds.Except(myCustomerIds).Any())
            {
                return AuthorizationResult.Fail();    
            }
            
            return AuthorizationResult.Succeed();
        }
    }
}