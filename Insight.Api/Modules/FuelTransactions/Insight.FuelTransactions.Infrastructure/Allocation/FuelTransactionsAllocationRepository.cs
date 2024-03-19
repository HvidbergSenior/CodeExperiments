using Dapper;
using Insight.BuildingBlocks.Exceptions;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Domain.Allocations;
using Insight.FuelTransactions.Domain.OutgoingFuelTransactions;
using Marten;
using Marten.PLv8.Patching;
using System.Globalization;
using Insight.BuildingBlocks.Domain;
using Weasel.Postgresql.SqlGeneration;

namespace Insight.FuelTransactions.Infrastructure.Allocation
{
    public sealed class FuelTransactionsAllocationRepository : IFuelTransactionsAllocationRepository
    {
        private readonly IDocumentSession documentSession;
        private readonly string schemaName;

        public FuelTransactionsAllocationRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
            schemaName = documentSession.DocumentStore.Options.DatabaseSchemaName!;
        }

        public async Task ClearAllocationDraftIdAsync(DraftAllocationId draftAllocationId, CancellationToken cancellationToken)
        {
            var whereFragmentSql = $"data->'{nameof(FuelTransaction.DraftAllocationId)}'->>'Value' = '{draftAllocationId.Value}'";

            var blankedId = DraftAllocationId.Create(Guid.Empty);
            documentSession.Patch<FuelTransaction>(new WhereFragment(whereFragmentSql)).Set(nameof(FuelTransaction.DraftAllocationId), blankedId);

            await documentSession.SaveChangesAsync(cancellationToken);
        }

        public async Task ClearAllocationDraftIdFromBatchAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId fuelTransactionCustomerId, ProductNumber productNumber, LocationId locationId, CancellationToken cancellationToken)
        {
            var whereFragmentSql = $@"data->'{nameof(FuelTransaction.DraftAllocationId)}'->>'Value' = '{draftAllocationId.Value}'" +
                $@" AND data->'{nameof(FuelTransaction.CustomerId)}'->>'Value' = '{fuelTransactionCustomerId.Value}'" +
                $@" AND data->'{nameof(FuelTransaction.ProductNumber)}'->>'Value' = '{productNumber.Value}'" +
                $@" AND data->>'{nameof(FuelTransaction.LocationId)}' = '{locationId.Value}'";

            var blankedId = DraftAllocationId.Create(Guid.Empty);
            documentSession.Patch<FuelTransaction>(new WhereFragment(whereFragmentSql)).Set(nameof(FuelTransaction.DraftAllocationId), blankedId);

            await documentSession.SaveChangesAsync(cancellationToken);
        }

        public async Task<DraftAllocationRowResponse> DraftAllocateFuelTransactionsAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId customerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, LocationId locationId, CancellationToken cancellationToken)
        {
            var sql = $@"
                UPDATE
                    {schemaName}.mt_doc_fueltransaction
                SET
                    data = jsonb_set(data, '{{DraftAllocationId}}', '{{""Value"":""{draftAllocationId.Value}""}}', true)
                WHERE 
                    data ->'{nameof(FuelTransaction.CustomerId)}'->>'Value' = :customerId" +
                    $@" AND data->'{nameof(FuelTransaction.ProductNumber)}'->>'Value' = :productNumber" +
                    $@" AND data->'{nameof(FuelTransaction.DraftAllocationId)}'->>'Value' = '{DraftAllocationId.Empty().Value}'" +
                    $@" AND data->>'{nameof(FuelTransaction.LocationId)}' = :locationId" +
                    $@" AND fuel_transaction_time_stamp >= :startDate::timestamp AND fuel_transaction_time_stamp <= :endDate::timestamp
              RETURNING
                    (data->'{nameof(FuelTransaction.FuelTransactionId)}'->>'Value')::uuid as FuelTransactionId, data->'{nameof(FuelTransaction.Quantity)}'->>'Value' as Quantity";

            var parameters = new
            {
                customerId = customerId.Value.ToString(),
                productNumber = productNumber.Value,
                locationId = locationId.Value,
                startDate = startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                endDate = endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            };

            var affectedRows = await documentSession.Connection!.QueryAsync<UpdateResponse>(new CommandDefinition(sql, parameters: parameters, cancellationToken: cancellationToken));

            return new DraftAllocationRowResponse()
            {
                FuelTransactionIds = affectedRows.Select(c => FuelTransactionId.Create(c.FuelTransactionId)).ToArray(),
                Quantity = Quantity.Create(affectedRows.Sum(c => c.Quantity))
            };
        }

        public async Task<DraftAllocationRowResponse> DraftAllocateFuelTransactionsPartiallyAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId customerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, LocationId locationId, Quantity maxQuantity, CancellationToken cancellationToken)
        {
            var sql = $@"
                WITH CTE AS(
                    SELECT
                        id, SUM((data->'{nameof(FuelTransaction.Quantity)}'->>'Value')::decimal) OVER (ORDER BY data->>'{nameof(FuelTransaction.FuelTransactionTimeStamp)}' ASC) AS running_sum
                    FROM
                        {schemaName}.mt_doc_fueltransaction
                    WHERE
                        data->'{nameof(FuelTransaction.CustomerId)}'->>'Value' = :customerId
                        AND data->'{nameof(FuelTransaction.ProductNumber)}'->>'Value' = :productNumber
                        AND data->'{nameof(FuelTransaction.DraftAllocationId)}'->>'Value' = '{DraftAllocationId.Empty().Value}'
                        AND data->>'{nameof(FuelTransaction.LocationId)}' = :locationId
                        AND fuel_transaction_time_stamp >= :startDate::timestamp AND fuel_transaction_time_stamp <= :endDate::timestamp
                )
                UPDATE
                    {schemaName}.mt_doc_fueltransaction
                SET
                    data = jsonb_set(data, '{{DraftAllocationId}}', '{{""Value"":""{draftAllocationId.Value}""}}', true)
                FROM                
                    CTE
                WHERE 
                    running_sum <= :maxQuantity
                    AND data->'{nameof(FuelTransaction.CustomerId)}'->>'Value' = :customerId
                    AND data->'{nameof(FuelTransaction.ProductNumber)}'->>'Value' = :productNumber
                    AND data->'{nameof(FuelTransaction.DraftAllocationId)}'->>'Value' = '{DraftAllocationId.Empty().Value}'
                    AND data->>'{nameof(FuelTransaction.LocationId)}' = :locationId
                    AND fuel_transaction_time_stamp >= :startDate::timestamp AND fuel_transaction_time_stamp <= :endDate::timestamp
                    AND mt_doc_fueltransaction.id = CTE.id -- VERY IMPORTANT --
              RETURNING
                    (data->'{nameof(FuelTransaction.FuelTransactionId)}'->>'Value')::uuid as FuelTransactionId, (data->'{nameof(FuelTransaction.Quantity)}'->>'Value')::decimal as Quantity";

            var parameters = new
            {
                customerId = customerId.Value.ToString(),
                productNumber = productNumber.Value,
                locationId = locationId.Value,
                startDate = startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                endDate = endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                maxQuantity = maxQuantity.Value,
            };

            var affectedRows = await documentSession.Connection!.QueryAsync<UpdateResponse>(new CommandDefinition(sql, parameters: parameters, cancellationToken: cancellationToken));

            return new DraftAllocationRowResponse()
            {
                FuelTransactionIds = affectedRows.Select(c => FuelTransactionId.Create(c.FuelTransactionId)).ToArray(),
                Quantity = Quantity.Create(affectedRows.Sum(c => c.Quantity))
            };
        }

        public async Task<IEnumerable<FuelTransactionId>> GetFuelTransactionIdsByDraftAllocationIdAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId customerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, LocationId locationId, CancellationToken cancellationToken)
        {
            var theStartDate = new DateTimeOffset(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, TimeSpan.Zero);
            var theEndDate = new DateTimeOffset(endDate.Year, endDate.Month, endDate.Day, 0, 0, 0, TimeSpan.Zero);
            return await documentSession.Query<FuelTransaction>().Where(c => c.DraftAllocationId.Value == draftAllocationId.Value
                && c.CustomerId.Value == customerId.Value
                && c.ProductNumber.Value == productNumber.Value
                && c.LocationId == locationId.Value
                && c.FuelTransactionTimeStamp >= theStartDate
                && c.FuelTransactionTimeStamp <= theEndDate
                && c.CustomerId.Value == customerId.Value
                && c.ProductNumber.Value == productNumber.Value
                && c.LocationId == locationId.Value).Select(c => c.FuelTransactionId).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MissingAllocationRowResponse>> GetMissingAllocationForPeriodAndFiltersAsync(DateOnly startDate, DateOnly endDate, string product, string company, string customer, CancellationToken cancellationToken)
        {
            var filteringCriteria = string.Empty;

            if (!product.ToUpperInvariant().Equals(ProductName.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                filteringCriteria += " AND data->'ProductName'->>'Value' ILIKE :productName";
            }

            if (!customer.ToUpperInvariant().Equals(CustomerName.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                filteringCriteria += " AND data->'CustomerName'->>'Value' ILIKE :customerName";
            }

            if (!company.ToUpperInvariant().Equals(CompanyName.Empty().Value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                filteringCriteria += " AND data->'CompanyName'->>'Value' ILIKE :companyName";
            }

            // NB: Parameters are not supported in ORDER BY and GROUP. (That's why those are interpolated.)            
            var sql = $@"              
                    SELECT 
                        data->'{nameof(FuelTransaction.CustomerNumber)}'->>'Value' as CustomerNumber, 
                        (data->'{nameof(FuelTransaction.CustomerId)}'->>'Value')::uuid as CustomerId,
                        data->'{nameof(FuelTransaction.CustomerName)}'->>'Value' as CustomerName,
                        data->'{nameof(FuelTransaction.ProductNumber)}'->>'Value' as ProductNumber, 
                        data->'{nameof(FuelTransaction.ProductName)}'->>'Value' as ProductName,
                        data->'{nameof(FuelTransaction.CompanyName)}'->>'Value' as CompanyName,
                    	data->'{nameof(FuelTransaction.StationNumber)}'->>'Value' as StationNumber,
                        data->'{nameof(FuelTransaction.StationName)}'->>'Value' as StationName,
                        data->'{nameof(FuelTransaction.Country)}'->>'Value' as Country,
                        data->>'{nameof(FuelTransaction.LocationId)}' as LocationId,
                        data->'{nameof(FuelTransaction.Location)}'->>'Value' as Location,
                        data->'{nameof(FuelTransaction.CustomerType)}'->>'Value' as CustomerType,
                        data->'{nameof(FuelTransaction.CustomerSegment)}'->>'Value' as CustomerSegment,
                    	SUM((data->'{nameof(FuelTransaction.Quantity)}'->>'Value')::numeric) as Quantity,
                        SUM(CASE WHEN data->'{nameof(FuelTransaction.DraftAllocationId)}'->>'Value' = '00000000-0000-0000-0000-000000000000' THEN (data->'{nameof(FuelTransaction.Quantity)}'->>'Value')::numeric ELSE 0 END) as MissingAllocationQuantity,
						SUM(CASE WHEN data->'{nameof(FuelTransaction.DraftAllocationId)}'->>'Value' != '00000000-0000-0000-0000-000000000000' THEN (data->'{nameof(FuelTransaction.Quantity)}'->>'Value')::numeric ELSE 0 END) as AllocatedQuantity,
					ROUND(SUM(CASE WHEN data->'{nameof(FuelTransaction.DraftAllocationId)}'->>'Value' != '00000000-0000-0000-0000-000000000000' THEN (data->'{nameof(FuelTransaction.Quantity)}'->>'Value')::numeric ELSE 0 END) / NULLIF(SUM((data->'{nameof(FuelTransaction.Quantity)}'->>'Value')::numeric), 0) * 100, 2) as AlreadyAllocatedPercentage,
                
					    COUNT(*) OVER() AS ItemCount
                    FROM 
                        {schemaName}.mt_doc_fueltransaction
                    WHERE
                    	fuel_transaction_time_stamp >= :startDate::timestamp AND fuel_transaction_time_stamp <= :endDate::timestamp  AND data->'{nameof(FuelTransaction.CustomerId)}'->>'Value' IS NOT NULL AND data->'{nameof(FuelTransaction.CustomerId)}'->>'Value' != '00000000-0000-0000-0000-000000000000'
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
                        CustomerType,
                        CustomerSegment,
                        CompanyName";                    

            var parameters = new
            {
                startDate = startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                endDate = endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                draftAllocationId = Guid.Empty.ToString(),
                customerName = customer != CustomerName.Empty().Value ? "%" + customer + "%": (string?)null,
                productName = product != ProductName.Empty().Value ? "%" + product + "%" : (string?)null,
                companyName = company != CompanyName.Empty().Value ? "%" + company + "%" : (string?)null,
            };

            var rows = await documentSession.Connection!.QueryAsync<SimpleOutgoingFuelTransaction>(new CommandDefinition(sql, parameters: parameters, cancellationToken: cancellationToken));
            
            return rows.Select(c => MissingAllocationRowResponse.Create(FuelTransactionCustomerId.Create(c.CustomerId),
                ProductNumber.Create(c.ProductNumber),
                LocationId.Create(c.LocationId),
                Quantity.Create(c.Quantity),
                FuelTransactionCountry.Create(c.Country),
                Quantity.Create(c.AllocatedQuantity),
                Location.Create(c.Location), 
                ProductName.Create(c.ProductName),
                CompanyName.Create(c.CompanyName)));
        }

        public async Task RollBackDraftAllocationAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId fuelTransactionCustomerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, LocationId locationId, CancellationToken cancellationToken)
        {
            // Todo: Perhaps we should use the collection of FuelTransactionIds instead of the where clause

            var whereFragmentSql = $"data ->'{nameof(FuelTransaction.CustomerId)}'->>'Value' = '{fuelTransactionCustomerId.Value}' AND data->'{nameof(FuelTransaction.ProductNumber)}'->>'Value' = '{productNumber.Value}'" +
                $" AND data->'{nameof(FuelTransaction.DraftAllocationId)}'->>'Value' = '{draftAllocationId.Value}'" +
                $" AND data->>'{nameof(FuelTransaction.LocationId)}' = '{locationId.Value}' AND data->>'{nameof(FuelTransaction.FuelTransactionTimeStamp)}' BETWEEN '{startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}' AND '{endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}'";

            var blankedId = DraftAllocationId.Create(Guid.Empty);
            documentSession.Patch<FuelTransaction>(new WhereFragment(whereFragmentSql)).Set(nameof(FuelTransaction.DraftAllocationId), blankedId);

            await documentSession.SaveChangesAsync(cancellationToken);
        }

        public async Task RollBackDraftAllocationAsync(DraftAllocationId draftAllocationId, FuelTransactionCustomerId fuelTransactionCustomerId, DateOnly startDate, DateOnly endDate, ProductNumber productNumber, LocationId locationId, FuelTransactionId[] fuelTransactionIds, CancellationToken cancellationToken)
        {
            var sql = $@"
                UPDATE
                    {schemaName}.mt_doc_fueltransaction
                SET
                    data = jsonb_set(data, '{{DraftAllocationId}}', '{{""Value"":""{DraftAllocationId.Empty().Value}""}}', true)
                WHERE 
                    data ->'{nameof(FuelTransaction.CustomerId)}'->>'Value' = :customerId" +
                    $@" AND data->'{nameof(FuelTransaction.ProductNumber)}'->>'Value' = :productNumber" +
                    $@" AND data->'{nameof(FuelTransaction.DraftAllocationId)}'->>'Value' = '{draftAllocationId.Value}'" +
                    $@" AND data->>'{nameof(FuelTransaction.LocationId)}' = :locationId" +
                    $@" AND fuel_transaction_time_stamp >= :startDate::timestamp AND fuel_transaction_time_stamp <= :endDate::timestamp" +
                    $@" AND {nameof(FuelTransaction.Id)} = ANY(:fuelTransactionIds)
              RETURNING
                    (data->'{nameof(FuelTransaction.FuelTransactionId)}'->>'Value')::uuid as FuelTransactionId, data->'{nameof(FuelTransaction.Quantity)}'->>'Value'as Quantity";

            var parameters = new
            {
                customerId = fuelTransactionCustomerId.Value.ToString(),
                productNumber = productNumber.Value,
                locationId = locationId.Value,
                startDate = startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                endDate = endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                fuelTransactionIds = fuelTransactionIds.Select(c => c.Value).ToArray()
            };

            var affectedRows = await documentSession.Connection!.QueryAsync<UpdateResponse>(new CommandDefinition(sql, parameters: parameters, cancellationToken: cancellationToken));
            if (affectedRows.Select(c => c.FuelTransactionId).Count() != fuelTransactionIds.Count())
            {
                throw new BusinessException("Rollback failed");
            }
        }

        private class UpdateResponse
        {
            public Guid FuelTransactionId { get; set; }
            public decimal Quantity { get; set; }
        }
    }
}
