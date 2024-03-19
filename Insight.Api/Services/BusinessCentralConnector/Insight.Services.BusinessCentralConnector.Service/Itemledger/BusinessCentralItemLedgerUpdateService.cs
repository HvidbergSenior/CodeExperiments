using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.FuelTransactions.Domain;
using Insight.Services.BusinessCentralConnector.Service.Configuration.LoadingDepots;
using Insight.Services.BusinessCentralConnector.Service.Customer;
using Insight.Services.BusinessCentralConnector.Service.Loadings;
using Insight.Services.BusinessCentralConnector.Service.Product;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Insight.BuildingBlocks.Domain;

namespace Insight.Services.BusinessCentralConnector.Service.Itemledger
{
    public class BusinessCentralItemLedgerUpdateService : BackgroundService
    {
        private readonly TimeSpan period = TimeSpan.FromHours(1);
        private readonly ILogger<BusinessCentralItemLedgerUpdateService> logger;
        private readonly IServiceScopeFactory factory;
        private bool firstRun = true;
        private bool IsEnabled { get; set; } = true;

        public BusinessCentralItemLedgerUpdateService(
            ILogger<BusinessCentralItemLedgerUpdateService> logger, IServiceScopeFactory factory)
        {
            this.logger = logger;
            this.factory = factory;
        }
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
                        var businessCentralFuelTransactionsItemLedgerUpdateService = asyncScope.ServiceProvider
                            .GetRequiredService<BusinessCentralItemLedgerService>();
                        var productService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralProductService>();
                        var businessCentralLoadingDepotService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralLoadingDepotService>();
                        var customerService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralCustomerService>();
                        var loadingsService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralLoadingService>();

                        var fuelTransactionsRepository =
                            asyncScope.ServiceProvider.GetRequiredService<IFuelTransactionsRepository>();
                        var fuelTransactionsTimeStampOffsetRepository = asyncScope.ServiceProvider
                            .GetRequiredService<IFuelTransactionsTimeStampOffsetRepository>();

                        var loadingDepotDict = (await businessCentralLoadingDepotService.GetAllAsync(stoppingToken)).ToDictionary(c => c.PlantIdentifier, c => c);

                        // Todo: we get ~26K shipments here.. consider doing it by date-range while iterating the transactions.
                        // This is quick and dirty solution.
                        var allShipments = await loadingsService.GetAllAsync(stoppingToken);
                        var shipmentDict = new Dictionary<long, BusinessCentralLoading>();

                        foreach (var shipment in allShipments)
                        {
                            shipmentDict.TryAdd(shipment.ShipmentIdentifier, shipment);
                        }

                        var customerDict = new Dictionary<string, BusinessCentralCustomer>();
                        var productDict = new Dictionary<string, BusinessCentralProduct>();
                        var notFoundProducts = new HashSet<string>();
                        var notFoundCustomers = new HashSet<string>();

                        var fuelTransactionsTimeStampOffset =
                         await fuelTransactionsTimeStampOffsetRepository.GetByFuelTransactionPosSystemAsync(FuelTransactionPosSystem.ItemLedger, stoppingToken);

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
                                await businessCentralFuelTransactionsItemLedgerUpdateService
                                .GetTransactionsAfterDateByPagingAsync(fuelTransactionsTimeStampOffset.FuelTransactionsOffsetTime.Value, transactionsToGet, fuelTransactionsTimeStampOffset.EntriesToSkip, stoppingToken);

                            var itemsToSkip = new[] { "325", "425", "430", "439", "440", "Z445", "445", "450", "465" }; // These are handled in Tokheim, Dialog and Tapnet services.

                            var skippedTransactions = false;
                            foreach (var transaction in fuelTransactions)
                            {
                                if (transaction.EntryType != "Sale")
                                {
                                    skippedTransactions = true;
                                    skipCount++;
                                    continue;
                                }

                                if (transaction.ItemNumber.In(itemsToSkip))
                                {
                                    skippedTransactions = true;
                                    skipCount++;
                                    continue;
                                }
                                if (notFoundProducts.Contains(transaction.ItemNumber) || notFoundCustomers.Contains(transaction.SourceNumber))
                                {
                                    skippedTransactions = true;
                                    skipCount++;
                                    continue;
                                }

                                if (!customerDict.TryGetValue(transaction.SourceNumber, out BusinessCentralCustomer? customer))
                                {
                                    var apiCustomer = await customerService.GetCustomerByCustomerNumberAndCompany(transaction.SourceNumber, transaction.CompanyId, transaction.CompanyName, stoppingToken);
                                    if (apiCustomer == null)
                                    {
                                        logger.LogWarning("Customer with customer number: {CustomerNumber} not found", transaction.SourceNumber);
                                        notFoundCustomers.Add(transaction.SourceNumber);
                                        skippedTransactions = true;
                                        skipCount++;
                                        continue;
                                    }

                                    customer = apiCustomer;
                                    customerDict.Add(transaction.SourceNumber, customer);
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

                                // enrich with product data
                                if (!productDict.TryGetValue(transaction.ItemNumber, out BusinessCentralProduct? product))
                                {
                                    var apiProduct = await productService.GetProductByProductAndCompanyIdNumber(transaction.ItemNumber, transaction.CompanyId, transaction.CompanyName, stoppingToken);
                                    if (apiProduct == null)
                                    {
                                        logger.LogWarning("Product with product number: {ProductNumber} not found", transaction.ItemNumber);
                                        notFoundProducts.Add(transaction.ItemNumber);
                                        skippedTransactions = true;
                                        skipCount++;
                                        continue;
                                    }

                                    product = apiProduct;
                                    productDict.Add(transaction.ItemNumber, product);
                                }
                                
                                // Enrich with location data
                                BusinessCentralLoadingDepot? depot = null;
                                if (transaction.PDIShipmentIdentifier > 0)
                                {
                                    if (!shipmentDict.TryGetValue(transaction.PDIShipmentIdentifier, out BusinessCentralLoading? loading))
                                    {
                                        logger.LogWarning("Loading with shipment number {ShipmentIdentifier} not found", transaction.PDIShipmentIdentifier);                                        
                                    }
                                    else
                                    {
                                        if (!loadingDepotDict.TryGetValue(loading.PlantIdentifier, out depot))
                                        {
                                            logger.LogWarning("Depot with PlantIdentifier {PlantIdentifier} not found", loading.PlantIdentifier);                                            
                                        }
                                    }
                                }

                                transactions.Add(CreateFuelTransaction(transaction, depot, customer, billToCustomer, product));
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
                                logger.LogInformation("Stored {TransactionCount} FuelTransactions from {ServiceName}", transactions.Count, nameof(BusinessCentralItemLedgerUpdateService));
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
                        logger.LogInformation("Done processing, skipped: {SkipCount}", skipCount);
                        logger.LogInformation("Executed {ServiceName}", nameof(BusinessCentralItemLedgerUpdateService));
                        fuelTransactionsTimeStampOffset = await UpdateOffset(fuelTransactionsTimeStampOffsetRepository, fuelTransactionsTimeStampOffset, latestTransactionDate, stoppingToken);
                    }
                    else
                    {
                        logger.LogInformation("Skipped {ServiceName}", nameof(BusinessCentralItemLedgerUpdateService));
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to execute {ServiceName} with exception message {ErrorMessage}}", nameof(BusinessCentralItemLedgerUpdateService), ex.Message);
                }
            }
        }

        private static FuelTransaction CreateFuelTransaction(BusinessCentralItemLedger transaction, BusinessCentralLoadingDepot? depot, BusinessCentralCustomer customer, BusinessCentralCustomer? billToCustomer, BusinessCentralProduct product)
        {
            Quantity quantity;

            if (product.Number.In("101", "102", "111")) // Tanks of 1.000 liter
            {
                quantity = Quantity.Create((transaction.Quantity * -1) * 1000);
            }
            else
            {
                quantity = Quantity.Create(transaction.Quantity * -1);
            }

            return FuelTransaction.Create(
                FuelTransactionId.Create(Guid.NewGuid()),
                FuelTransactionPosSystem.ItemLedger,
                StationNumber.Create(depot?.PlantIdentifier.ToString(CultureInfo.InvariantCulture) ?? "N/A"),
                StationName.Create(depot?.Name ?? "N/A"),
                FuelTransactionDate.Create(transaction.PostingDate),
                FuelTransactionTime.Create(transaction.TransTime),
                ProductNumber.Create(product.Number!),
                ProductName.Create(product.ItemCategoryCode!),
                quantity,
                Odometer.Empty(),
                DriverCardNumber.Empty(),
                VehicleCardNumber.Empty(),
                CustomerNumber.Create(billToCustomer?.Number ?? customer.Number),
                CustomerName.Create(billToCustomer?.CustomerName ?? customer.CustomerName),
                SourceETag.Create(transaction.Etag),
                FuelTransactionCountry.Create(depot?.Country ?? customer.CustomerCountryRegion),
                SourceSystemPropertyBag.Empty(),
                SourceSystemId.Create(transaction.SystemId),
                Location.Create(depot?.ISCCStorage ?? "N/A"),
                CustomerType.Create(customer.CustomerDeliveryType),
                CustomerSegment.Create(customer.CustomerIndustry),
                CompanyName.Create(transaction.CompanyName),
                AccountNumber.Create(customer.Number),
                AccountName.Create(customer.CustomerName),
                AccountCustomerId.Empty(),// This is set later.
                ProductDescription.Create(product.Description!),
                ShipToLocation.Create($"{customer.CustomerPostCode}, {customer.CustomerAddress}"),
                Driver.Empty());
        }

        private static async Task<FuelTransactionsOffset> UpdateSkipAmountOnOffset(int amountOfEntitiesToSkip, IFuelTransactionsTimeStampOffsetRepository fuelTransactionsTimeStampOffsetRepository, FuelTransactionsOffset? fuelTransactionsTimeStampOffset, CancellationToken stoppingToken)
        {
            fuelTransactionsTimeStampOffset = await fuelTransactionsTimeStampOffsetRepository.GetByFuelTransactionPosSystemAsync(FuelTransactionPosSystem.ItemLedger, stoppingToken);

            ArgumentNullException.ThrowIfNull(fuelTransactionsTimeStampOffset);

            fuelTransactionsTimeStampOffset.SetEntriesToSkip(amountOfEntitiesToSkip);
            await fuelTransactionsTimeStampOffsetRepository.Update(fuelTransactionsTimeStampOffset, stoppingToken);
            await fuelTransactionsTimeStampOffsetRepository.SaveChanges(stoppingToken);
            return fuelTransactionsTimeStampOffset;
        }
        private static async Task<FuelTransactionsOffset> UpdateOffset(IFuelTransactionsTimeStampOffsetRepository fuelTransactionsTimeStampOffsetRepository, FuelTransactionsOffset? fuelTransactionsTimeStampOffset, DateTimeOffset latestTransactionDate, CancellationToken stoppingToken)
        {
            fuelTransactionsTimeStampOffset = await fuelTransactionsTimeStampOffsetRepository.GetByFuelTransactionPosSystemAsync(FuelTransactionPosSystem.ItemLedger, stoppingToken);
            ArgumentNullException.ThrowIfNull(fuelTransactionsTimeStampOffset);
            fuelTransactionsTimeStampOffset.SetFuelTransactionsOffsetTime(FuelTransactionsOffsetTime.Create(latestTransactionDate));
            fuelTransactionsTimeStampOffset.SetEntriesToSkip(0);
            await fuelTransactionsTimeStampOffsetRepository.Update(fuelTransactionsTimeStampOffset, stoppingToken);
            await fuelTransactionsTimeStampOffsetRepository.SaveChanges(stoppingToken);
            return fuelTransactionsTimeStampOffset;
        }

        private static async Task<FuelTransactionsOffset> InitializeFuelTransactionsOffsetAsync(
            IFuelTransactionsTimeStampOffsetRepository fuelTransactionsTimeStampOffsetRepository,
            IUnitOfWork unitOfWork,
            CancellationToken stoppingToken)
        {
            var fuelTransactionOffset = FuelTransactionsOffset.Create(FuelTransactionsOffsetId.Create(Guid.NewGuid()), FuelTransactionPosSystem.ItemLedger, FuelTransactionsOffsetTime.Create(DateTimeOffset.MinValue));
            await fuelTransactionsTimeStampOffsetRepository.Add(fuelTransactionOffset, stoppingToken);

            await fuelTransactionsTimeStampOffsetRepository.SaveChanges(stoppingToken);
            await unitOfWork.Commit(stoppingToken);
            return fuelTransactionOffset;
        }
    }
}
