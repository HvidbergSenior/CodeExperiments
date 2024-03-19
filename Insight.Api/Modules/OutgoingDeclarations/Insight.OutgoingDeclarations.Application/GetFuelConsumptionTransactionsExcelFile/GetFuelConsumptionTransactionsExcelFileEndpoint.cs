using Azure.Core;
using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Domain;
using Insight.FuelTransactions.Domain;
using Insight.OutgoingDeclarations.Domain;
using Insight.OutgoingDeclarations.Domain.FuelConsumptionTransactionsExcelFile;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.OutgoingDeclarations.Application.GetFuelConsumptionTransactionsExcelFile;

public static class GetFuelConsumptionTransactionsExcelFileEndpoint
{
    public static void MapGetFuelConsumptionTransactionsExcelFileEndpoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint
            .MapPost(
                OutgoingDeclarationsEndpointUrls.GET_FUELCONSUMPTION_TRANSACTIONS_EXCEL_FILE_ENDPOINT,
                async (
                    GetFuelConsumptionTransactionsExcelFileRequest request,
                    IExecutionContext executionContext,
                    IQueryBus queryBus,
                    CancellationToken cancellationToken
                ) =>
                {
                    var datePeriod = FuelTransactionsBetweenDates.Empty();

                    if (request.DateFrom is not null && request.DateTo is not null)
                    {
                        datePeriod = FuelTransactionsBetweenDates.Create((DateOnly)request.DateFrom, (DateOnly)request.DateTo);
                    }

                    var productNamesFilter = new List<ProductName>();
                    if (request.ProductNames != null)
                    {
                        foreach (var productName in request.ProductNames)
                        {
                            productNamesFilter.Add(ProductName.Create(ProductNameEnumerationExtensions.ProductNameEnumerationToTranslatedString(productName)));
                        }
                    }
                    var customerIdsFilter = new List<FuelTransactionCustomerId>();
                    if (await executionContext.GetAdminPrivileges())
                    {
                        //If user is admin just add the customers it wants to see as it has access to all and if empty leave blank to show them all
                        if (request.CustomerIds != null)
                        {
                            customerIdsFilter = request.CustomerIds.Select(c =>
                                FuelTransactionCustomerId.Create(c)).ToList();
                        }
                    }
                    else
                    {
                        //If user is not admin we must only get the customers of which it has fuel consumption access. If the list is empty we must set it to the list of customer permissions it has to filter by it
                        var customerPermissions = await executionContext.GetCustomersPermissionsAsync(true, cancellationToken);
                        if (request.CustomerIds != null && request.CustomerIds.Length > 0)
                        {
                            customerIdsFilter = request.CustomerIds.Where(c =>
                            {
                                var customerPermission = customerPermissions.Where(p => p.CustomerId.Value == c)
                                    .FirstOrDefault();
                                return ((customerPermission != null) && (customerPermission.Permissions.Contains(CustomerPermission.FuelConsumption) || customerPermission.Permissions.Contains(CustomerPermission.Admin)));
                            }).Select(c =>
                                FuelTransactionCustomerId.Create(c)).ToList();
                        }
                        else
                        {
                            //If empty just put all the customers the user has
                            customerIdsFilter = customerPermissions
                                .Where(c => c.Permissions.Contains(CustomerPermission.FuelConsumption) || c.Permissions.Contains(CustomerPermission.Admin))
                                .Select(c => FuelTransactionCustomerId.Create(c.CustomerId.Value)).ToList();
                        }

                        if (customerIdsFilter.IsEmpty())
                        {
                            //If the list of customers is still empty for a regular user we must abort as the filter will be empty and we would show all transactions otherwise!
                            throw new ArgumentException("A regular user without customers assigned has no transactions");
                        }
                    }

                    var filteringParameters = FuelConsumptionTransactionsExcelFileFilteringParameters.Create(datePeriod, productNamesFilter, customerIdsFilter);

                    var query = GetFuelConsumptionTransactionsExcelFileQuery.Create(filteringParameters);

                    var results = await queryBus
                        .Send<GetFuelConsumptionTransactionsExcelFileQuery,
                            GetFuelConsumptionTransactionsExcelFileResponse>(query, cancellationToken);

                    return Results.File(results.Data, results.ContentType, results.FileName);
                })
            .RequireAuthorization()
            .WithName("GetFuelConsumptionTransactionsExcelFile")
            .WithTags("OutgoingDeclarations");
    }
}