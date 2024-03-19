using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.FuelTransactions.Domain;
using Insight.Services.BusinessCentralConnector.Service.Company;
using Insight.Services.BusinessCentralConnector.Service.Customer;
using Insight.Services.BusinessCentralConnector.Service.FuelCardBiofuelExpress;
using Insight.Services.BusinessCentralConnector.Service.Helpers;
using Insight.Services.BusinessCentralConnector.Service.Product;
using Insight.Services.BusinessCentralConnector.Service.Station;
using Insight.Services.BusinessCentralConnector.Service.TransactionsTokheim;
using Marten;
using Marten.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Insight.BuildingBlocks.Domain;

namespace Insight.Services.BusinessCentralConnector.Service.TransactionsTapnet
{
    public class BusinessCentralFuelTransactionsTapnetUpdateService : BackgroundService
    {
        private readonly TimeSpan period = TimeSpan.FromHours(1);
        private readonly ILogger<BusinessCentralFuelTransactionsTapnetUpdateService> logger;
        private readonly IServiceScopeFactory factory;
        private bool firstRun = true;
        private bool IsEnabled { get; set; } = true;

        public BusinessCentralFuelTransactionsTapnetUpdateService(
            ILogger<BusinessCentralFuelTransactionsTapnetUpdateService> logger, IServiceScopeFactory factory)
        {
            this.logger = logger;
            this.factory = factory;
        }

        // Todo: Refactor the three services to use the same ExecuteAsync, too many similarities to not have it generic.
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(period);

            while (
                !stoppingToken.IsCancellationRequested &&
                (firstRun || await timer.WaitForNextTickAsync(stoppingToken)))
            {
                firstRun = false;
                try
                {
                    var moreDataExists = true;
                    var amountOfEntitiesToSkip = 0;
                    const int transactionsToGet = 500;

                    if (IsEnabled)
                    {
                        await using AsyncServiceScope asyncScope = factory.CreateAsyncScope();

                        var unitOfWork = asyncScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                        var businessCentralFuelTransactionsTapnetUpdateService = asyncScope.ServiceProvider
                            .GetRequiredService<BusinessCentralFuelTransactionsTapnetService>();
                        var businessCentralStationService =
                            asyncScope.ServiceProvider.GetRequiredService<BusinessCentralStationService>();
                        var customerService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralCustomerService>();
                        var productService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralProductService>();
                        var companyService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralCompanyService>();
                        var fuelTransactionsRepository =
                            asyncScope.ServiceProvider.GetRequiredService<IFuelTransactionsRepository>();
                        var fuelTransactionsTimeStampOffsetRepository = asyncScope.ServiceProvider
                            .GetRequiredService<IFuelTransactionsTimeStampOffsetRepository>();


                        var fuelCardService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralFuelCardBiofuelExpressService>();

                        var allFuelcards = (await fuelCardService.GetAllAsync(stoppingToken));

                        var allFuelcardsDict = allFuelcards.ToDictionary(c => c.CardNumber, c => c);

                        var stationDict = new Dictionary<string, BusinessCentralStation>();
                        var customerDict = new Dictionary<string, BusinessCentralCustomer>();
                        var productDict = new Dictionary<string, BusinessCentralProduct>();
                        var notFoundProducts = new HashSet<string>();
                        var notFoundCustomers = new HashSet<string>();

                        var fuelTransactionsTimeStampOffset = await fuelTransactionsTimeStampOffsetRepository.GetByFuelTransactionPosSystemAsync(FuelTransactionPosSystem.Tapnet, stoppingToken);

                        if (fuelTransactionsTimeStampOffset == null)
                        {
                            fuelTransactionsTimeStampOffset = await InitializeFuelTransactionsOffsetAsync(fuelTransactionsTimeStampOffsetRepository, unitOfWork, stoppingToken);
                        }
                        amountOfEntitiesToSkip = fuelTransactionsTimeStampOffset.EntriesToSkip;
                        var skipCount = 0;
                        DateTimeOffset latestTransactionDate = fuelTransactionsTimeStampOffset.FuelTransactionsOffsetTime.Value;
                        while (moreDataExists)
                        {
                            var transactions = new List<FuelTransaction>();

                            var fuelTransactions =
                                await businessCentralFuelTransactionsTapnetUpdateService
                                    .GetTransactionsAfterDateByPagingAsync(fuelTransactionsTimeStampOffset.FuelTransactionsOffsetTime.Value, transactionsToGet,
                                        fuelTransactionsTimeStampOffset.EntriesToSkip, stoppingToken);
                            var companies = await companyService.GetCompanies(stoppingToken);

                            var skippedTransactions = false;
                            foreach (var transaction in fuelTransactions)
                            {
                                if (notFoundProducts.Contains(transaction.TapNetTransmissionsItemNumber) || notFoundCustomers.Contains(transaction.TapNetCustomerNumber))
                                {
                                    skippedTransactions = true;
                                    skipCount++;
                                    continue;
                                }

                                if (transaction.SystemCreatedAt == DateTime.MinValue)
                                {
                                    transaction.SystemCreatedAt = DateTime.ParseExact($"{transaction.TransDate} {transaction.TransTime}", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                }

                                // Enrich with customer data
                                if (!customerDict.TryGetValue(transaction.TapNetCustomerNumber, out BusinessCentralCustomer? customer))
                                {
                                    var apiCustomer = await customerService.GetCustomerByCustomerNumber(transaction.TapNetCustomerNumber, stoppingToken);
                                    if (apiCustomer == null)
                                    {
                                        logger.LogWarning("Customer with customer number {CustomerNumber} not found, skipping future transactions for customer", transaction.TapNetCustomerNumber);
                                        notFoundCustomers.Add(transaction.TapNetCustomerNumber);
                                        skippedTransactions = true;
                                        skipCount++;
                                        continue;
                                    }

                                    customer = apiCustomer;
                                    customerDict.Add(transaction.TapNetCustomerNumber, customer);
                                }

                                BusinessCentralCustomer? billToCustomer = null;
                                if (!string.IsNullOrWhiteSpace(customer.CustomerBillToNumber))
                                {
                                    if (!customerDict.TryGetValue(customer.CustomerBillToNumber, out billToCustomer))
                                    {
                                        var apiCustomer = await customerService.GetCustomerByCustomerNumber(customer.CustomerBillToNumber, stoppingToken);
                                        if (apiCustomer == null)
                                        {
                                            logger.LogWarning("Customer with customer number {CustomerNumber} not found", customer.CustomerBillToNumber);
                                            notFoundCustomers.Add(customer.CustomerBillToNumber);
                                            skippedTransactions = true;
                                            skipCount++;
                                            continue;
                                        }

                                        billToCustomer = apiCustomer;
                                        customerDict.Add(customer.CustomerBillToNumber, billToCustomer);
                                    }
                                }

                                // Enrich with product data
                                if (!productDict.TryGetValue(transaction.TapNetTransmissionsItemNumber, out BusinessCentralProduct? product))
                                {
                                    var apiProduct = await productService.GetProductByProductAndCompanyIdNumber(transaction.TapNetTransmissionsItemNumber, transaction.CompanyId, transaction.CompanyName, stoppingToken);
                                    if (apiProduct == null)
                                    {
                                        logger.LogWarning("Product with product number {ProductNumber} not found, skipping future transactions with same product number", transaction.TapNetTransmissionsItemNumber);
                                        notFoundProducts.Add(transaction.TapNetTransmissionsItemNumber);
                                        skippedTransactions = true;
                                        skipCount++;
                                        continue;
                                    }

                                    product = apiProduct;
                                    productDict.Add(transaction.TapNetTransmissionsItemNumber, product);
                                }

                                // Enrich with station data
                                if (!stationDict.TryGetValue(transaction.TapNetTransStationNumber, out BusinessCentralStation? station))
                                {
                                    var apiStation = await businessCentralStationService.GetStationByStationNumber(transaction.TapNetTransStationNumber, stoppingToken);
                                    if (apiStation == null)
                                    {
                                        throw new NotFoundException($"Station with station number {transaction.TapNetTransStationNumber} not found");
                                    }

                                    station = apiStation;
                                    stationDict.Add(transaction.TapNetTransStationNumber, station);
                                }

                                // Enrich with fuel card data
                                allFuelcardsDict.TryGetValue(transaction.TapNetTransmissionsCardNumber, out BusinessCentralFuelCardBiofuelExpress? businessCentralFuelCardBiofuelExpress);

                                //TODO: How to set CountrySold?
                                if (string.IsNullOrEmpty(transaction.TapNetTransmissionsCountrySold?.Trim()))
                                {
                                    if (transaction.TapNetTransmissionsCurrency == "SEK")
                                    {
                                        transaction.TapNetTransmissionsCountrySold = "SE";
                                    }
                                    if (transaction.TapNetTransmissionsCurrency == "DK")
                                    {
                                        transaction.TapNetTransmissionsCountrySold = "DK";
                                    }
                                }
                                transactions.Add(CreateFuelTransaction(transaction, station, customer, billToCustomer, product, companies, businessCentralFuelCardBiofuelExpress));
                            }

                            if (transactions.Any())
                            {
                                var itemHashes = transactions.Select(c => c.ItemHash).ToArray();
                                var conflictingHashes = await fuelTransactionsRepository.GetExistingHashesAsync(itemHashes, stoppingToken);
                                if (conflictingHashes.Any())
                                {
                                    logger.LogWarning("Conflicting hashes found");
                                    transactions = transactions.Where(c => !c.ItemHash.In(conflictingHashes.ToArray())).ToList();
                                }

                                latestTransactionDate = fuelTransactions.Last().SystemCreatedAt;

                                if (transactions.Any())
                                {
                                    await fuelTransactionsRepository.Add(transactions, stoppingToken);
                                    await fuelTransactionsRepository.SaveChanges(stoppingToken);
                                }

                                amountOfEntitiesToSkip += transactionsToGet;
                                fuelTransactionsTimeStampOffset = await UpdateSkipAmountOnOffset(amountOfEntitiesToSkip, fuelTransactionsTimeStampOffsetRepository, fuelTransactionsTimeStampOffset, stoppingToken);

                                try
                                {
                                    await unitOfWork.Commit(stoppingToken);
                                    logger.LogInformation("Stored {TransactionCount} FuelTransactions from {ServiceName}", transactions.Count, nameof(BusinessCentralFuelTransactionsTapnetUpdateService));
                                }
                                catch (DocumentAlreadyExistsException e)
                                {
                                    var offendingTransaction = transactions.FirstOrDefault(c => c.Id == (Guid)e.Id);
                                    e.Message.ToString();
                                    var collissions = transactions.GroupBy(c => c.ItemHash).Where(c => c.Count() > 1);
                                    throw;
                                }
                            }
                            else
                            {
                                if (!skippedTransactions)
                                    moreDataExists = false;

                                amountOfEntitiesToSkip += transactionsToGet;
                                fuelTransactionsTimeStampOffset = await UpdateSkipAmountOnOffset(amountOfEntitiesToSkip, fuelTransactionsTimeStampOffsetRepository, fuelTransactionsTimeStampOffset, stoppingToken);
                                await unitOfWork.Commit(stoppingToken);
                            }
                        }
                        logger.LogInformation("Executed {SericeName}", nameof(BusinessCentralFuelTransactionsTapnetUpdateService));
                        fuelTransactionsTimeStampOffset = await UpdateOffset(unitOfWork, fuelTransactionsTimeStampOffsetRepository, fuelTransactionsTimeStampOffset, latestTransactionDate, stoppingToken);
                    }
                    else
                    {
                        logger.LogInformation("Skipped {ServiceName}", nameof(BusinessCentralFuelTransactionsTapnetUpdateService));
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to execute {ServiceName} with exception message {ErrorMessage}", nameof(BusinessCentralFuelTransactionsTapnetUpdateService), ex.Message);
                }
            }
        }

        private static async Task<FuelTransactionsOffset> UpdateSkipAmountOnOffset(int amountOfEntitiesToSkip, IFuelTransactionsTimeStampOffsetRepository fuelTransactionsTimeStampOffsetRepository, FuelTransactionsOffset? fuelTransactionsTimeStampOffset, CancellationToken stoppingToken)
        {
            fuelTransactionsTimeStampOffset = await fuelTransactionsTimeStampOffsetRepository.GetByFuelTransactionPosSystemAsync(FuelTransactionPosSystem.Tapnet, stoppingToken);
            ArgumentNullException.ThrowIfNull(fuelTransactionsTimeStampOffset);
            fuelTransactionsTimeStampOffset.SetEntriesToSkip(amountOfEntitiesToSkip);
            await fuelTransactionsTimeStampOffsetRepository.Update(fuelTransactionsTimeStampOffset, stoppingToken);
            await fuelTransactionsTimeStampOffsetRepository.SaveChanges(stoppingToken);
            return fuelTransactionsTimeStampOffset;
        }

        private static async Task<FuelTransactionsOffset> UpdateOffset(IUnitOfWork unitOfWork, IFuelTransactionsTimeStampOffsetRepository fuelTransactionsTimeStampOffsetRepository, FuelTransactionsOffset? fuelTransactionsTimeStampOffset, DateTimeOffset latestTransactionDate, CancellationToken stoppingToken)
        {
            fuelTransactionsTimeStampOffset = await fuelTransactionsTimeStampOffsetRepository.GetByFuelTransactionPosSystemAsync(FuelTransactionPosSystem.Tapnet, stoppingToken);
            ArgumentNullException.ThrowIfNull(fuelTransactionsTimeStampOffset);
            fuelTransactionsTimeStampOffset.SetFuelTransactionsOffsetTime(FuelTransactionsOffsetTime.Create(latestTransactionDate));
            fuelTransactionsTimeStampOffset.SetEntriesToSkip(0);
            await fuelTransactionsTimeStampOffsetRepository.Update(fuelTransactionsTimeStampOffset, stoppingToken);
            await fuelTransactionsTimeStampOffsetRepository.SaveChanges(stoppingToken);
            try
            {
                await unitOfWork.Commit(stoppingToken);
            }
            catch (DocumentAlreadyExistsException e)
            {
                e.Message.ToString();
                throw;
            }
            return fuelTransactionsTimeStampOffset;
        }


        private async Task<FuelTransactionsOffset> InitializeFuelTransactionsOffsetAsync(
            IFuelTransactionsTimeStampOffsetRepository fuelTransactionsTimeStampOffsetRepository,
            IUnitOfWork unitOfWork,
            CancellationToken stoppingToken)
        {
            var fuelTransactionOffset = FuelTransactionsOffset.Create(FuelTransactionsOffsetId.Create(Guid.NewGuid()), FuelTransactionPosSystem.Tapnet, FuelTransactionsOffsetTime.Create(DateTimeOffset.MinValue));
            await fuelTransactionsTimeStampOffsetRepository.Add(fuelTransactionOffset, stoppingToken);

            await fuelTransactionsTimeStampOffsetRepository.SaveChanges(stoppingToken);
            await unitOfWork.Commit(stoppingToken);
            return fuelTransactionOffset;
        }

        private FuelTransaction CreateFuelTransaction(BusinessCentralFuelTransactionsTapnet transaction, BusinessCentralStation station, BusinessCentralCustomer customer, BusinessCentralCustomer? billToCustomer, BusinessCentralProduct product, BusinessCentralCompany[] companies, BusinessCentralFuelCardBiofuelExpress? businessCentralFuelCardBiofuelExpress)
        {
            var companyName = GetCompanyName(transaction.TapNetTransmissionsCountrySold, companies);

            return FuelTransaction.Create(
                FuelTransactionId.Create(Guid.NewGuid()),
                FuelTransactionPosSystem.Tapnet,
                StationNumber.Create(transaction.TapNetTransStationNumber),
                StationName.Create(station.BFStationName),
                FuelTransactionDate.Create(transaction.TransDate),
                FuelTransactionTime.Create(transaction.TransTime),
                ProductNumber.Create(transaction.TapNetTransmissionsItemNumber),
                ProductName.Create(product.ItemCategoryCode!),
                Quantity.Create(transaction.TapNetTransmissionsQuantity),
                Odometer.Create(transaction.TapNetTransmissionsOdometer),
                DriverCardNumber.Create(transaction.TapNetTransmissionsDriverId),
                VehicleCardNumber.Create(transaction.TapNetTransmissionsCardNumber),
                CustomerNumber.Create(billToCustomer?.Number ?? customer.Number),
                CustomerName.Create(billToCustomer?.CustomerName ?? customer.CustomerName),
                SourceETag.Create(transaction.Etag),
                FuelTransactionCountry.Create(string.IsNullOrWhiteSpace(transaction.TapNetTransmissionsCountrySold) ? station.BFStationCountryRegionCode : transaction.TapNetTransmissionsCountrySold),
                SourceSystemPropertyBag.Create($"{transaction.ReportRef}:{transaction.TapNetTransmissionsReportRef}:{transaction.TapNetTransCardNumber2}:{transaction.TapNetTransmissionsBatchNumber}{transaction.SeqNumber}:{transaction.SystemCreatedAt}"),
                SourceSystemId.Create(transaction.SystemId),
                Location.Create("EXTERNAL"),
                CustomerType.Create(customer.CustomerDeliveryType),
                CustomerSegment.Create(customer.CustomerIndustry),
                CompanyName.Create(companyName),
                AccountNumber.Create(customer.Number),
                AccountName.Create(customer.CustomerName),
                AccountCustomerId.Empty(), // This is set later.
                ProductDescription.Create(product.Description!),
                ShipToLocation.Create($"{station.BFStationCity}, {station.BFStationAddress} ({station.StationNumber})"), 
                Driver.Create(businessCentralFuelCardBiofuelExpress?.BFCardFakturaId ?? string.Empty));
        }

        private string GetCompanyName(string? countrySold, BusinessCentralCompany[] companies)
        {
            var companyName = "";

            switch (countrySold)
            {
                case "SE":
                    companyName = companies.First(p => p.Name.Contains("Biofuel Express AB")).Name;
                    break;
                case "DK":
                    companyName = companies.First(p => p.Name.Contains("Biofuel Express AS")).Name;
                    break;
                case "A":
                    companyName = companies.First(p => p.Name.Contains("Biofuel Express Austria GmbH")).Name;
                    break;
                case "DE":
                    companyName = companies.First(p => p.Name.Contains("Biofuel Express DMCC")).Name;
                    break;
                default:
                    //Log Error?
                    break;
            }

            return companyName;
        }
    }
}