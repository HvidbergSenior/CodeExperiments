using Ganss.Excel;
using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.FuelTransactions.Domain;
using Insight.OutgoingDeclarations.Application.GetFuelConsumptionTransactions;
using Insight.OutgoingDeclarations.Domain.FuelConsumptionTransactionsExcelFile;

namespace Insight.OutgoingDeclarations.Application.GetFuelConsumptionTransactionsExcelFile;

public sealed class FuelConsumptionTransaction
{
    [Column(1, "Date")] public required string Date { get; set; }
    [Column(2, "Time")] public required string Time { get; set; }
    [Column(3, "Customer No.")] public required string CustomerNumber { get; set; }
    [Column(4, "Customer Name")] public required string CustomerName { get; set; }
    [Column(5, "Account No.")] public required string AccountNumber { get; set; }
    [Column(6, "Account Name")] public required string AccountName { get; set; }
    [Column(7, "Location")] public required string Location { get; set; }
    [Column(8, "Vehicle Card")] public required string VehicleCard { get; set; }
    [Column(9, "Vehicle")] public required string Vehicle { get; set; }
    [Column(10, "Driver Card")] public required string DriverCard { get; set; }
    [Column(11, "Driver")] public required string Driver { get; set; }
    [Column(12, "Odometer")] public required int Odometer { get; set; }
    [Column(13, "Product No.")] public required string ProductNumber { get; set; }
    [Column(14, "Product Name")] public required string ProductName { get; set; }
    [Column(15, "Quantity")] public required decimal Quantity { get; set; }
}

public sealed class
    GetFuelConsumptionTransactionsExcelFileQuery : IQuery<GetFuelConsumptionTransactionsExcelFileResponse>
{
    public FuelConsumptionTransactionsExcelFileFilteringParameters FuelConsumptionFilteringParameters
    {
        get;
        private set;
    }

    private GetFuelConsumptionTransactionsExcelFileQuery(
        FuelConsumptionTransactionsExcelFileFilteringParameters filteringParameters)
    {
        FuelConsumptionFilteringParameters = filteringParameters;
    }

    public static GetFuelConsumptionTransactionsExcelFileQuery Create(
        FuelConsumptionTransactionsExcelFileFilteringParameters filteringParameters)
    {
        return new GetFuelConsumptionTransactionsExcelFileQuery(filteringParameters);
    }
}

public sealed class GetFuelConsumptionTransactionsExcelFileHandler : IQueryHandler<GetFuelConsumptionTransactionsExcelFileQuery, GetFuelConsumptionTransactionsExcelFileResponse>
{
    private readonly IFuelTransactionsRepository fuelTransactionsRepository;

    public GetFuelConsumptionTransactionsExcelFileHandler(IFuelTransactionsRepository fuelTransactionsRepository)
    {
        this.fuelTransactionsRepository = fuelTransactionsRepository;
    }

    public async Task<GetFuelConsumptionTransactionsExcelFileResponse> Handle(
        GetFuelConsumptionTransactionsExcelFileQuery request,
        CancellationToken cancellationToken)
    {
        return await GetFuelConsumptionTransactionsExcelFile(request, cancellationToken);
    }

    private static string MaskLastCharacter(string value)
    {
        return value.Length > 0 ? $"{value.Substring(0, value.Length - 1)}*" : "";
    }

    private async Task<GetFuelConsumptionTransactionsExcelFileResponse> GetFuelConsumptionTransactionsExcelFile(
        GetFuelConsumptionTransactionsExcelFileQuery request,
        CancellationToken cancellationToken)
    {
        int page = 1;
        var thereAreMore = true;
        var countSoFar = 0;
        var totalResult = new List<FuelConsumptionTransaction>();
        while (thereAreMore)
        {
            var paginationParameters =
                Insight.FuelTransactions.Domain.PaginationParameters
                    .Create(page, 1000); //TODO: Iterate until there's no more?
            var sortingParameters =
                Insight.FuelTransactions.Domain.SortingParameters.Create(false, "FuelTransactionTimeStamp");

            var (transactions, totalCount) = await fuelTransactionsRepository.GetFuelTransactionsRefinedAsync(
                request.FuelConsumptionFilteringParameters.DatePeriod,
                request.FuelConsumptionFilteringParameters.ProductNames,
                request.FuelConsumptionFilteringParameters.CustomerIds,
                paginationParameters,
                sortingParameters,
                cancellationToken);

            countSoFar += transactions.Count();

            var result = transactions.Select(c =>
                new FuelConsumptionTransaction()
                {
                    Date = c.FuelTransactionDate.Value,
                    Time = c.FuelTransactionTime.Value,
                    CustomerNumber = c.CustomerNumber.Value,
                    CustomerName = c.CustomerName.Value,
                    AccountNumber = c.AccountNumber.Value,
                    AccountName = c.AccountName.Value,
                    Location = c.ShipToLocation.Value,
                    VehicleCard = MaskLastCharacter(c.VehicleCardNumber.Value),
                    Vehicle = c.Driver.Value,
                    DriverCard = MaskLastCharacter(c.DriverCardNumber.Value),
                    Driver = c.Driver.Value,
                    Odometer = c.Odometer.Value,
                    ProductNumber = c.ProductNumber.Value,
                    ProductName = c.ProductDescription.Value,
                    Quantity = c.Quantity.Value
                });
            totalResult.AddRange(result);

            if (countSoFar >= totalCount)
            {
                thereAreMore = false;
            }
        }

        var s = new MemoryStream();
        new ExcelMapper().Save(s, totalResult, "Transactions");
        //Important: Need to put buffer back to start!
        s.Position = 0;

        return new GetFuelConsumptionTransactionsExcelFileResponse()
        {
            Data = s,
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            FileName = "Transactions.xlsx"
        };
    }

    private static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}

//Black magic? Leave for now
internal class
    GetFuelConsumptionTransactionsExcelFileQueryAuthorizer : IAuthorizer<GetFuelConsumptionTransactionsExcelFileQuery>
{
    private readonly IExecutionContext executionContext;

    public GetFuelConsumptionTransactionsExcelFileQueryAuthorizer(IExecutionContext executionContext)
    {
        this.executionContext = executionContext;
    }

    public Task<AuthorizationResult> Authorize(GetFuelConsumptionTransactionsExcelFileQuery reportQuery,
        CancellationToken cancellation)
    {
        return Task.FromResult(AuthorizationResult.Succeed());
    }
}