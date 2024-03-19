using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.FuelTransactions.Domain;
using Insight.Services.BusinessCentralConnector.Service.Customer;
using Insight.Services.BusinessCentralConnector.Service.FuelCardAcceptance;
using Insight.Services.BusinessCentralConnector.Service.Product;
using Insight.Services.BusinessCentralConnector.Service.Station;
using Marten;
using Marten.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Globalization;
using System.Transactions;
using Insight.BuildingBlocks.Domain;
using Insight.Services.BusinessCentralConnector.Service.Company;
using Insight.Services.BusinessCentralConnector.Service.Helpers;
using Insight.Services.BusinessCentralConnector.Service.FuelCardBiofuelExpress;

namespace Insight.Services.BusinessCentralConnector.Service.TransactionsTokheim
{
    public class BusinessCentralFuelTransactionsTokheimUpdateService : BackgroundService
    {
        private readonly TimeSpan period = TimeSpan.FromHours(1);
        private readonly ILogger<BusinessCentralFuelTransactionsTokheimUpdateService> logger;
        private readonly IServiceScopeFactory factory;
        private bool firstRun = true;
        private bool IsEnabled { get; set; } = true;

        public BusinessCentralFuelTransactionsTokheimUpdateService(
            ILogger<BusinessCentralFuelTransactionsTokheimUpdateService> logger,
            IServiceScopeFactory factory)
        {
            this.logger = logger;
            this.factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(period);

            while (
            !stoppingToken.IsCancellationRequested && (firstRun || await timer.WaitForNextTickAsync(stoppingToken)))
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
                        var businessCentralFuelTransactionsTokheimUpdateService = asyncScope.ServiceProvider
                            .GetRequiredService<BusinessCentralFuelTransactionsTokheimService>();
                        var businessCentralStationService =
                            asyncScope.ServiceProvider.GetRequiredService<BusinessCentralStationService>();
                        var productService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralProductService>();
                        var customerService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralCustomerService>();
                        var companyService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralCompanyService>();

                        var fuelCardAcceptanceService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralFuelCardAcceptanceService>();
                        var fuelTransactionsRepository =
                            asyncScope.ServiceProvider.GetRequiredService<IFuelTransactionsRepository>();
                        var fuelTransactionsTimeStampOffsetRepository = asyncScope.ServiceProvider
                            .GetRequiredService<IFuelTransactionsTimeStampOffsetRepository>();

                        var fuelCardService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralFuelCardBiofuelExpressService>();

                        var allFuelcards = (await fuelCardService.GetAllAsync(stoppingToken));

                        var allFuelcardsDict = allFuelcards.ToDictionary(c => c.CardNumber, c => c);

                        var allFuelCardAcceptanceEntries = await fuelCardAcceptanceService.GetAllAsync(stoppingToken);
                        var fuelCardAcceptanceDict = new Dictionary<string, string>();
                        foreach (var fca in allFuelCardAcceptanceEntries)
                        {
                            if (string.IsNullOrEmpty(fca.BFExternalCardsTokheimCardtype) || string.IsNullOrEmpty(fca.BFExternalCardsAccount))
                            {
                                continue;
                            }
                            fuelCardAcceptanceDict.TryAdd(fca.BFExternalCardsTokheimCardtype, fca.BFExternalCardsAccount);
                        }

                        var stationDict = new Dictionary<string, BusinessCentralStation>();
                        var customerDict = new Dictionary<string, BusinessCentralCustomer>();
                        var productDict = new Dictionary<string, BusinessCentralProduct>();
                        var notFoundProducts = new HashSet<string>();
                        var notFoundCustomers = new HashSet<string>();

                        var fuelTransactionsTimeStampOffset =
                       await fuelTransactionsTimeStampOffsetRepository.GetByFuelTransactionPosSystemAsync(FuelTransactionPosSystem.Tokheim, stoppingToken);

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
                                await businessCentralFuelTransactionsTokheimUpdateService
                                    .GetTransactionsAfterDateByPagingAsync(fuelTransactionsTimeStampOffset.FuelTransactionsOffsetTime.Value, transactionsToGet,
                                        fuelTransactionsTimeStampOffset.EntriesToSkip, stoppingToken);
                            var companies = await companyService.GetCompanies(stoppingToken);

                            var skippedTransactions = false;
                            foreach (var transaction in fuelTransactions)
                            {
                                if (transaction.SystemCreatedAt == DateTime.MinValue)
                                {
                                    transaction.SystemCreatedAt = DateTime.ParseExact($"{transaction.TransDate} {transaction.TransTime}", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                }

                                if (notFoundProducts.Contains(transaction.ItemNumber) || notFoundCustomers.Contains(transaction.Account))
                                {
                                    skippedTransactions = true;
                                    skipCount++;
                                    continue;
                                }

                                // Enrich with customer data
                                if (string.IsNullOrWhiteSpace(transaction.Account))
                                {
                                    // Check for card type.
                                    if (!string.IsNullOrEmpty(transaction.CardType) && fuelCardAcceptanceDict.ContainsKey(transaction.CardType))
                                    {
                                        transaction.Account = fuelCardAcceptanceDict[transaction.CardType];
                                    }
                                    else
                                    {
                                        logger.LogWarning("Customer number is empty for transaction with id {Transaction}", JsonConvert.SerializeObject(transaction));
                                        skippedTransactions = true;
                                        skipCount++;
                                        continue;
                                    }
                                }

                                if (!customerDict.TryGetValue(transaction.Account, out BusinessCentralCustomer? customer))
                                {
                                    var apiCustomer = await customerService.GetCustomerByCustomerNumber(transaction.Account, stoppingToken);
                                    if (apiCustomer == null)
                                    {
                                        logger.LogWarning("Customer with customer number {CustomerNumber} not found", transaction.Account);
                                        notFoundCustomers.Add(transaction.Account);
                                        skippedTransactions = true;
                                        skipCount++;
                                        continue;
                                    }

                                    customer = apiCustomer;
                                    customerDict.Add(transaction.Account, customer);
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
                                if (!productDict.TryGetValue(transaction.ItemNumber, out BusinessCentralProduct? product))
                                {
                                    var apiProduct = await productService.GetProductByProductAndCompanyIdNumber(transaction.ItemNumber, transaction.CompanyId, transaction.CompanyName, stoppingToken);
                                    if (apiProduct == null)
                                    {
                                        logger.LogWarning("Product with product number {ProductNumber} not found", transaction.ItemNumber);
                                        notFoundProducts.Add(transaction.ItemNumber);
                                        skippedTransactions = true;
                                        skipCount++;
                                        continue;
                                    }

                                    product = apiProduct;
                                    productDict.Add(transaction.ItemNumber, product);
                                }

                                // Enrich with station data
                                if (!stationDict.TryGetValue(transaction.StationNumber, out BusinessCentralStation? value))
                                {
                                    var station = await businessCentralStationService.GetStationByStationNumber(transaction.StationNumber, stoppingToken);
                                    if (station == null)
                                    {
                                        throw new NotFoundException($"Station with station number {transaction.StationNumber} not found");
                                    }

                                    value = station;
                                    stationDict.Add(transaction.StationNumber, value);
                                }

                                // Enrich with fuel card data
                                allFuelcardsDict.TryGetValue(transaction.CardNumber, out BusinessCentralFuelCardBiofuelExpress? businessCentralFuelCardBiofuelExpress);
                                
                                if (string.IsNullOrEmpty(transaction.CountrySold?.Trim()))
                                {
                                    if (transaction.Currency == "SEK")
                                    {
                                        transaction.CountrySold = "SE";
                                    }
                                    if (transaction.Currency == "DK")
                                    {
                                        transaction.CountrySold = "DK";
                                    }
                                }
                                transactions.Add(CreateFuelTransaction(transaction, value, customer, billToCustomer, product, companies, businessCentralFuelCardBiofuelExpress));
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

                                await unitOfWork.Commit(stoppingToken);
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
                        logger.LogInformation("Executed {ServiceName}", nameof(BusinessCentralFuelTransactionsTokheimUpdateService));

                        fuelTransactionsTimeStampOffset = await UpdateOffset(unitOfWork, fuelTransactionsTimeStampOffsetRepository, fuelTransactionsTimeStampOffset, latestTransactionDate, stoppingToken);
                    }
                    else
                    {
                        logger.LogInformation("Skipped {ServiceName}", nameof(BusinessCentralFuelTransactionsTokheimUpdateService));
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to execute {ServiceName} with exception message {ErrorMessage}", nameof(BusinessCentralFuelTransactionsTokheimUpdateService), ex.Message);
                }
            }
        }

        private static async Task<FuelTransactionsOffset> UpdateOffset(IUnitOfWork unitOfWork, IFuelTransactionsTimeStampOffsetRepository fuelTransactionsTimeStampOffsetRepository, FuelTransactionsOffset? fuelTransactionsTimeStampOffset, DateTimeOffset latestTransactionDate, CancellationToken stoppingToken)
        {
            fuelTransactionsTimeStampOffset = await fuelTransactionsTimeStampOffsetRepository.GetByFuelTransactionPosSystemAsync(FuelTransactionPosSystem.Tokheim, stoppingToken);
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

            }
            return fuelTransactionsTimeStampOffset;
        }

        private static async Task<FuelTransactionsOffset> UpdateSkipAmountOnOffset(int amountOfEntitiesToSkip, IFuelTransactionsTimeStampOffsetRepository fuelTransactionsTimeStampOffsetRepository, FuelTransactionsOffset? fuelTransactionsTimeStampOffset, CancellationToken stoppingToken)
        {
            fuelTransactionsTimeStampOffset = await fuelTransactionsTimeStampOffsetRepository.GetByFuelTransactionPosSystemAsync(FuelTransactionPosSystem.Tokheim, stoppingToken);
            ArgumentNullException.ThrowIfNull(fuelTransactionsTimeStampOffset);

            fuelTransactionsTimeStampOffset.SetEntriesToSkip(amountOfEntitiesToSkip);
            await fuelTransactionsTimeStampOffsetRepository.Update(fuelTransactionsTimeStampOffset, stoppingToken);
            await fuelTransactionsTimeStampOffsetRepository.SaveChanges(stoppingToken);
            return fuelTransactionsTimeStampOffset;
        }

        private async Task<FuelTransactionsOffset> InitializeFuelTransactionsOffsetAsync(
             IFuelTransactionsTimeStampOffsetRepository fuelTransactionsTimeStampOffsetRepository,
             IUnitOfWork unitOfWork,
             CancellationToken stoppingToken)
        {
            var fuelTransactionOffset = FuelTransactionsOffset.Create(FuelTransactionsOffsetId.Create(Guid.NewGuid()), FuelTransactionPosSystem.Tokheim, FuelTransactionsOffsetTime.Create(DateTimeOffset.MinValue));
            await fuelTransactionsTimeStampOffsetRepository.Add(fuelTransactionOffset, stoppingToken);

            await fuelTransactionsTimeStampOffsetRepository.SaveChanges(stoppingToken);
            await unitOfWork.Commit(stoppingToken);
            return fuelTransactionOffset;
        }
        private static FuelTransaction CreateFuelTransaction(BusinessCentralTransactionsTokheim transaction, BusinessCentralStation station, BusinessCentralCustomer customer, BusinessCentralCustomer? billToCustomer, BusinessCentralProduct product, BusinessCentralCompany[] companies, BusinessCentralFuelCardBiofuelExpress? businessCentralFuelCardBiofuelExpress)
        {
            var companyName = GetCompanyName(transaction.CountrySold, companies);

            return FuelTransaction.Create(
                FuelTransactionId.Create(Guid.NewGuid()),
                FuelTransactionPosSystem.Tokheim,
                StationNumber.Create(transaction.StationNumber),
                StationName.Create(station.BFStationName),
                FuelTransactionDate.Create(transaction.TransDate),
                FuelTransactionTime.Create(transaction.TransTime),
                ProductNumber.Create(transaction.ItemNumber),
                ProductName.Create(product.ItemCategoryCode!),
                Quantity.Create(transaction.LiterBF),
                Odometer.Create(transaction.KMBF),
                DriverCardNumber.Create(transaction.DriverCardNumber),
                VehicleCardNumber.Create(transaction.CardNumber),
                CustomerNumber.Create(billToCustomer?.Number ?? customer.Number),
                CustomerName.Create(billToCustomer?.CustomerName ?? customer.CustomerName),
                SourceETag.Create(transaction.Etag),
                FuelTransactionCountry.Create(transaction.CountrySold ?? string.Empty),
                SourceSystemPropertyBag.Create($"{transaction.KMBF}:{transaction.RegistrationNumber}:{transaction.SequenceNumber}:{transaction.SystemCreatedAt}"),
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

        private static string GetCompanyName(string? countrySold, BusinessCentralCompany[] companies)
        {
            string companyName = "";

            switch (countrySold)
            {
                case "SE":
                    companyName = companies.First(p => p.Name.Contains("Biofuel Express AB")).Name;
                    //Or Biofuel Express DMCC is in sweden as well
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