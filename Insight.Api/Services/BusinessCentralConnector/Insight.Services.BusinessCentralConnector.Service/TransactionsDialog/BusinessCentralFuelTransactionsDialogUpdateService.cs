using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.FuelTransactions.Domain;
using Insight.Services.BusinessCentralConnector.Service.Customer;
using Insight.Services.BusinessCentralConnector.Service.Product;
using Insight.Services.BusinessCentralConnector.Service.Station;
using Marten;
using Marten.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Insight.Services.BusinessCentralConnector.Service.Company;
using System.Globalization;
using Insight.BuildingBlocks.Domain;
using Insight.Services.BusinessCentralConnector.Service.FuelCardBiofuelExpress;

namespace Insight.Services.BusinessCentralConnector.Service.TransactionsDialog
{
    public class BusinessCentralFuelTransactionsDialogUpdateService : BackgroundService
    {
        private readonly TimeSpan period = TimeSpan.FromHours(1);
        private readonly ILogger<BusinessCentralFuelTransactionsDialogUpdateService> logger;
        private readonly IServiceScopeFactory factory;
        private bool firstRun = true;

        private bool IsEnabled { get; set; } = true;
        public BusinessCentralFuelTransactionsDialogUpdateService(
            ILogger<BusinessCentralFuelTransactionsDialogUpdateService> logger, IServiceScopeFactory factory)
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
                        var businessCentralFuelTransactionsDialogUpdateService = asyncScope.ServiceProvider
                            .GetRequiredService<BusinessCentralFuelTransactionsDialogService>();
                        var productService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralProductService>();
                        var businessCentralStationService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralStationService>();
                        var customerService = asyncScope.ServiceProvider.GetRequiredService<BusinessCentralCustomerService>();
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

                        var fuelTransactionsTimeStampOffset =
                         await fuelTransactionsTimeStampOffsetRepository.GetByFuelTransactionPosSystemAsync(FuelTransactionPosSystem.Dialog, stoppingToken);

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
                                await businessCentralFuelTransactionsDialogUpdateService
                                    .GetTransactionsAfterDateByPagingAsync(fuelTransactionsTimeStampOffset.FuelTransactionsOffsetTime.Value, transactionsToGet, fuelTransactionsTimeStampOffset.EntriesToSkip, stoppingToken);

                            var companies = await companyService.GetCompanies(stoppingToken);
                            var skippedTransactions = false;
                            foreach (var transaction in fuelTransactions)
                            {
                                if (notFoundProducts.Contains(transaction.DialogTransactionsItemNumber) || notFoundCustomers.Contains(transaction.DialogTransactionsCustomerNumber))
                                {
                                    skippedTransactions = true;
                                    skipCount++;
                                    continue;
                                }

                                if (transaction.SystemCreatedAt == DateTime.MinValue)
                                {
                                    transaction.SystemCreatedAt = DateTime.ParseExact($"{transaction.TransDate} {transaction.TransTime}", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                }

                                if (!customerDict.TryGetValue(transaction.DialogTransactionsCustomerNumber, out BusinessCentralCustomer? customer))
                                {
                                    var apiCustomer = await customerService.GetCustomerByCustomerNumber(transaction.DialogTransactionsCustomerNumber, stoppingToken);
                                    if (apiCustomer == null)
                                    {
                                        logger.LogWarning("Customer with customer number {CustomerNumber} not found", transaction.DialogTransactionsCustomerNumber);
                                        notFoundCustomers.Add(transaction.DialogTransactionsCustomerNumber);
                                        skippedTransactions = true;
                                        skipCount++;
                                        continue;
                                    }

                                    customer = apiCustomer;
                                    customerDict.Add(transaction.DialogTransactionsCustomerNumber, customer);
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

                                if (string.IsNullOrWhiteSpace(transaction.DialogTransactionsStationNumber))
                                {
                                    logger.LogInformation("Skipped transaction for Customer {Customer} due to not having a station number", transaction.DialogTransactionsCustomerNumber);
                                    skippedTransactions = true;
                                    skipCount++;
                                    continue;
                                }

                                // enrich with product data
                                if (!productDict.TryGetValue(transaction.DialogTransactionsItemNumber, out BusinessCentralProduct? product))
                                {
                                    var apiProduct = await productService.GetProductByProductAndCompanyIdNumber(transaction.DialogTransactionsItemNumber, transaction.CompanyId, transaction.CompanyName, stoppingToken);
                                    if (apiProduct == null)
                                    {
                                        logger.LogWarning("Product with product number {ProductNumber} not found", transaction.DialogTransactionsItemNumber);
                                        notFoundProducts.Add(transaction.DialogTransactionsItemNumber);
                                        skippedTransactions = true;
                                        skipCount++;
                                        continue;
                                    }

                                    product = apiProduct;
                                    productDict.Add(transaction.DialogTransactionsItemNumber, product);
                                }

                                // Enrich with station data
                                if (!stationDict.TryGetValue(transaction.DialogTransactionsStationNumber, out BusinessCentralStation? station))
                                {
                                    var apiStation = await businessCentralStationService.GetStationByStationNumber(transaction.DialogTransactionsStationNumber, stoppingToken);
                                    if (apiStation == null)
                                    {
                                        throw new NotFoundException($"Station with station number {transaction.DialogTransactionsStationNumber} not found");
                                    }

                                    station = apiStation;
                                    stationDict.Add(transaction.DialogTransactionsStationNumber, station);
                                }

                                // Enrich with fuel card data
                                allFuelcardsDict.TryGetValue(transaction.DialogTransactionsCardNumber, out BusinessCentralFuelCardBiofuelExpress? businessCentralFuelCardBiofuelExpress);

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
                                    logger.LogInformation("Stored {TransactionCount} FuelTransactions from {ServiceName}", transactions.Count, nameof(BusinessCentralFuelTransactionsDialogUpdateService));
                                }
                                catch (DocumentAlreadyExistsException e)
                                {
                                    // Session is dead.

                                    var secondUnitOfWork = asyncScope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                                    var offendingTransaction = transactions.FirstOrDefault(c => c.Id == (Guid)e.Id);
                                    e.Message.ToString();
                                    var collissions = transactions.GroupBy(c => c.ItemHash).Where(c => c.Count() > 1);
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
                        logger.LogInformation("Done processing, skipped: {SkipCount}", skipCount);
                        logger.LogInformation("Executed {ServiceName}", nameof(BusinessCentralFuelTransactionsDialogUpdateService));
                        fuelTransactionsTimeStampOffset = await UpdateOffset(unitOfWork, fuelTransactionsTimeStampOffsetRepository, fuelTransactionsTimeStampOffset, latestTransactionDate, stoppingToken);
                    }
                    else
                    {
                        logger.LogInformation("Skipped {ServiceName}", nameof(BusinessCentralFuelTransactionsDialogUpdateService));
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to execute {ServiceName} with exception message {ErrorMessage}}", nameof(BusinessCentralFuelTransactionsDialogUpdateService), ex.Message);
                }
            }
        }

        private static async Task<FuelTransactionsOffset> UpdateSkipAmountOnOffset(int amountOfEntitiesToSkip, IFuelTransactionsTimeStampOffsetRepository fuelTransactionsTimeStampOffsetRepository, FuelTransactionsOffset? fuelTransactionsTimeStampOffset, CancellationToken stoppingToken)
        {
            fuelTransactionsTimeStampOffset = await fuelTransactionsTimeStampOffsetRepository.GetByFuelTransactionPosSystemAsync(FuelTransactionPosSystem.Dialog, stoppingToken);

            ArgumentNullException.ThrowIfNull(fuelTransactionsTimeStampOffset);

            fuelTransactionsTimeStampOffset.SetEntriesToSkip(amountOfEntitiesToSkip);
            await fuelTransactionsTimeStampOffsetRepository.Update(fuelTransactionsTimeStampOffset, stoppingToken);
            await fuelTransactionsTimeStampOffsetRepository.SaveChanges(stoppingToken);
            return fuelTransactionsTimeStampOffset;
        }
        private static async Task<FuelTransactionsOffset> UpdateOffset(IUnitOfWork unitOfWork, IFuelTransactionsTimeStampOffsetRepository fuelTransactionsTimeStampOffsetRepository, FuelTransactionsOffset? fuelTransactionsTimeStampOffset, DateTimeOffset latestTransactionDate, CancellationToken stoppingToken)
        {
            fuelTransactionsTimeStampOffset = await fuelTransactionsTimeStampOffsetRepository.GetByFuelTransactionPosSystemAsync(FuelTransactionPosSystem.Dialog, stoppingToken);
            ArgumentNullException.ThrowIfNull(fuelTransactionsTimeStampOffset);
            fuelTransactionsTimeStampOffset.SetFuelTransactionsOffsetTime(FuelTransactionsOffsetTime.Create(latestTransactionDate));
            fuelTransactionsTimeStampOffset.SetEntriesToSkip(0);
            await fuelTransactionsTimeStampOffsetRepository.Update(fuelTransactionsTimeStampOffset, stoppingToken);
            await fuelTransactionsTimeStampOffsetRepository.SaveChanges(stoppingToken);

            await unitOfWork.Commit(stoppingToken);
            return fuelTransactionsTimeStampOffset;
        }

        private static async Task<FuelTransactionsOffset> InitializeFuelTransactionsOffsetAsync(
            IFuelTransactionsTimeStampOffsetRepository fuelTransactionsTimeStampOffsetRepository,
            IUnitOfWork unitOfWork,
            CancellationToken stoppingToken)
        {
            var fuelTransactionOffset = FuelTransactionsOffset.Create(FuelTransactionsOffsetId.Create(Guid.NewGuid()), FuelTransactionPosSystem.Dialog, FuelTransactionsOffsetTime.Create(DateTimeOffset.MinValue));
            await fuelTransactionsTimeStampOffsetRepository.Add(fuelTransactionOffset, stoppingToken);

            await fuelTransactionsTimeStampOffsetRepository.SaveChanges(stoppingToken);
            await unitOfWork.Commit(stoppingToken);
            return fuelTransactionOffset;
        }
        private static FuelTransaction CreateFuelTransaction(BusinessCentralFuelTransactionsDialog transaction, BusinessCentralStation station, BusinessCentralCustomer customer, BusinessCentralCustomer? billToCustomer, BusinessCentralProduct product, BusinessCentralCompany[] companies, BusinessCentralFuelCardBiofuelExpress? businessCentralFuelCardBiofuelExpress)
        {
            //All FuelTransactionDialogs are from Sweden
            const string companyName = "Biofuel Express AB";

            return FuelTransaction.Create(
                FuelTransactionId.Create(Guid.NewGuid()),
                FuelTransactionPosSystem.Dialog,
                StationNumber.Create(transaction.DialogTransactionsStationNumber),
                StationName.Create(station.BFStationName),
                FuelTransactionDate.Create(transaction.TransDate),
                FuelTransactionTime.Create(transaction.TransTime),
                ProductNumber.Create(transaction.DialogTransactionsItemNumber),
                ProductName.Create(product.ItemCategoryCode!),
                Quantity.Create(transaction.DialogTransactionsQuantity),
                Odometer.Create(transaction.DialogTransactionsOdometer),
                DriverCardNumber.Create(transaction.DialogTransactionsDriverCardNumber),
                VehicleCardNumber.Create(transaction.DialogTransactionsCardNumber),
                CustomerNumber.Create(billToCustomer?.Number ?? customer.Number),
                CustomerName.Create(billToCustomer?.CustomerName ?? customer.CustomerName),
                SourceETag.Create(transaction.Etag),
                FuelTransactionCountry.Create("SE"), // TODO: Where's the country info?
                SourceSystemPropertyBag.Create($"{transaction.ReportRef}:{transaction.SeqNumber}:{transaction.DialogTransactionsBatchNumber}:{transaction.SystemCreatedAt}"),
                SourceSystemId.Create(transaction.SystemId),
                Location.Create("EXTERNAL"),
                CustomerType.Create(customer.CustomerDeliveryType),
                CustomerSegment.Create(customer.CustomerIndustry),
                CompanyName.Create(companyName),
                AccountNumber.Create(customer.Number),
                AccountName.Create(customer.CustomerName),
                AccountCustomerId.Empty(),// This is set later.
                ProductDescription.Create(product.Description!),
                ShipToLocation.Create($"{station.BFStationCity}, {station.BFStationAddress} ({station.StationNumber})"),
                Driver.Create(businessCentralFuelCardBiofuelExpress?.BFCardFakturaId ?? string.Empty)); 
        }
    }
}