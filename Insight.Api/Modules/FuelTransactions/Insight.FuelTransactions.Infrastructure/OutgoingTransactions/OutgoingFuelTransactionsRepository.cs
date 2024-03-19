using System.Globalization;
using Dapper;
using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Domain.OutgoingFuelTransactions;
using Marten;
using SortingParameters = Insight.FuelTransactions.Domain.SortingParameters;

namespace Insight.FuelTransactions.Infrastructure.OutgoingTransactions
{
    public class OutgoingFuelTransactionsRepository : IOutgoingFuelTransactionsRepository
    {
        private readonly IDocumentSession documentSession;
        private readonly string schemaName;
        
        public OutgoingFuelTransactionsRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
            schemaName = documentSession.DocumentStore.Options.DatabaseSchemaName!;
        }

        public async Task<(IReadOnlyList<OutgoingFuelTransaction> Items, int TotalCount, decimal TotalQuantity)> GetByAggregatedFuelTransactionsAsync(int page, int pageSize, SortingParameters sortingParameters, FilteringParameters filteringParameters, CancellationToken cancellationToken = default)
        {
            page = page == 0 ? 1 : page;

            var skip = page * pageSize - pageSize;

            var filteringCriteria = string.Empty;

            if (filteringParameters.CustomerName.Value.ToLower(CultureInfo.InvariantCulture) != CustomerName.Empty().Value.ToLower(CultureInfo.InvariantCulture))
            {
                filteringCriteria += " AND data->'CustomerName'->>'Value' ILIKE :customerName";
            }
            
            if (filteringParameters.ProductName.Value.ToLower(CultureInfo.InvariantCulture) != ProductName.Empty().Value.ToLower(CultureInfo.InvariantCulture))
            {
                filteringCriteria += " AND data->'ProductName'->>'Value' ILIKE :productName";
            }
            
            if (filteringParameters.CompanyName.Value.ToLower(CultureInfo.InvariantCulture) != CompanyName.Empty().Value.ToLower(CultureInfo.InvariantCulture))
            {
                filteringCriteria += " AND data->'CompanyName'->>'Value' ILIKE :companyName";
            }

            var startDate = DateOnly.MinValue;
            var endDate = DateOnly.MaxValue;
            if(filteringParameters.DatePeriod != DatePeriod.Empty())
            {
                startDate = filteringParameters.DatePeriod.StartDate;
                endDate = filteringParameters.DatePeriod.EndDate;
            }

            // NB: Parameters are not supported in ORDER BY and GROUP. (That's why those are interpolated.)            
            var sql = $@"              
                    SELECT 
                        data->'{nameof(FuelTransaction.CustomerNumber)}'->>'Value' as CustomerNumber, 
                        (data->'{nameof(FuelTransaction.CustomerId)}'->>'Value')::uuid as CustomerId,
                        data->'{nameof(FuelTransaction.CustomerName)}'->>'Value' as CustomerName,
                        data->'{nameof(FuelTransaction.ProductNumber)}'->>'Value' as ProductNumber, 
                        data->'{nameof(FuelTransaction.ProductName)}'->>'Value' as ProductName,
                    	data->'{nameof(FuelTransaction.StationNumber)}'->>'Value' as StationNumber,
                        data->'{nameof(FuelTransaction.StationName)}'->>'Value' as StationName,
                        data->'{nameof(FuelTransaction.Country)}'->>'Value' as Country,
                        data->>'{nameof(FuelTransaction.LocationId)}' as LocationId,
                        data->'{nameof(FuelTransaction.Location)}'->>'Value' as Location,
                        data->'{nameof(FuelTransaction.ShipToLocation)}'->>'Value' as ShipToLocation,
                        data->'{nameof(FuelTransaction.CustomerType)}'->>'Value' as CustomerType,
                        data->'{nameof(FuelTransaction.CustomerSegment)}'->>'Value' as CustomerSegment,
                    	SUM((data->'{nameof(FuelTransaction.Quantity)}'->>'Value')::numeric) as Quantity,
                        SUM(CASE WHEN data->'{nameof(FuelTransaction.DraftAllocationId)}'->>'Value' = '00000000-0000-0000-0000-000000000000' THEN (data->'{nameof(FuelTransaction.Quantity)}'->>'Value')::numeric ELSE 0 END) as MissingAllocationQuantity,
						SUM(CASE WHEN data->'{nameof(FuelTransaction.DraftAllocationId)}'->>'Value' != '00000000-0000-0000-0000-000000000000' THEN (data->'{nameof(FuelTransaction.Quantity)}'->>'Value')::numeric ELSE 0 END) as AllocatedQuantity,
						ROUND(SUM(CASE WHEN data->'{nameof(FuelTransaction.DraftAllocationId)}'->>'Value' != '00000000-0000-0000-0000-000000000000' THEN (data->'{nameof(FuelTransaction.Quantity)}'->>'Value')::numeric ELSE 0 END) / NULLIF(SUM((data->'{nameof(FuelTransaction.Quantity)}'->>'Value')::numeric), 0) * 100, 2) as AlreadyAllocatedPercentage,
                        SUM(SUM((data->'{nameof(FuelTransaction.Quantity)}'->>'Value')::numeric)) OVER() as TotalQuantity,
                        COUNT(*) OVER() AS ItemCount
                    FROM 
                        {schemaName}.mt_doc_fueltransaction
                    WHERE
                    	fuel_transaction_time_stamp >= :startDate AND fuel_transaction_time_stamp <= :endDate  AND data->'{nameof(FuelTransaction.CustomerId)}'->>'Value' IS NOT NULL AND data->'{nameof(FuelTransaction.CustomerId)}'->>'Value' != '00000000-0000-0000-0000-000000000000'                 
                        {filteringCriteria}
                    GROUP BY 
                        CustomerNumber,
                        CustomerId,
                        CustomerName,
                        ProductNumber,
                        ProductName,
                    	StationNumber,
                        StationName,
                        Country,
                        LocationId,
                        Location,
                        ShipToLocation,
                        CustomerType,
                        CustomerSegment                
                    ORDER BY
                    	{sortingParameters.OrderByProperty} {(sortingParameters.IsOrderDescending ? "DESC" : "ASC")}
                    LIMIT {pageSize}
                    OFFSET {skip}";

            var parameters = new
            {
                startDate = startDate,
                endDate = endDate,
                customerName = filteringParameters.CustomerName.Value != CustomerName.Empty().Value ? "%" + filteringParameters.CustomerName.Value + "%": (string?)null,
                productName = filteringParameters.ProductName.Value != ProductName.Empty().Value ? "%" + filteringParameters.ProductName.Value + "%" : (string?)null,
                companyName = filteringParameters.CompanyName.Value != CompanyName.Empty().Value ? "%" + filteringParameters.CompanyName.Value + "%" : (string?)null,
            };

            var rows = await documentSession.Connection!.QueryAsync<SimpleOutgoingFuelTransaction>(new CommandDefinition(sql, parameters: parameters, cancellationToken: cancellationToken));

            var outgoingFuelTransactions = rows.Select(c => c.AsOutgoingFuelTransaction()).ToList();


            return (outgoingFuelTransactions, outgoingFuelTransactions.FirstOrDefault()?.ItemCount.Value ?? 0, outgoingFuelTransactions.FirstOrDefault()?.TotalQuantity.Value ?? 0);
        }
    }
}
