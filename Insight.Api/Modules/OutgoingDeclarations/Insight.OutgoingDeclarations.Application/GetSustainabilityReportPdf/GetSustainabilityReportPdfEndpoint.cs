using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Exceptions;
using Insight.OutgoingDeclarations.Domain;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.OutgoingDeclarations.Application.GetSustainabilityReportPdf
{
    public static class GetSustainabilityReportPdfEndpoint
    {
        public static void MapGetSustainabilityReportPdfEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(OutgoingDeclarationsEndpointUrls.GET_SUSTAINABILITY_REPORTPDF_ENDPOINT, async (
                        GetSustainabilityReportPdfRequest request,
                        IExecutionContext executionContext,
                        IQueryBus queryBus,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        var datePeriod = DatePeriod.Empty();

                        if (request.DateFrom is not null && request.DateTo is not null)
                        {
                            datePeriod = DatePeriod.Create((DateOnly)request.DateFrom, (DateOnly)request.DateTo);
                        }
                        var productNamesFilter = new List<ProductName>();
                        if (request.ProductNames != null)
                        {
                            foreach (var productName in request.ProductNames)
                            {
                                productNamesFilter.Add(ProductName.Create(ProductNameEnumerationExtensions.ProductNameEnumerationToTranslatedString(productName)));
                            }
                        }

                        var customerIdsFilter = new List<CustomerId>();
                        var customerNumbersFilter = new List<CustomerNumber>();
                        if (await executionContext.GetAdminPrivileges())
                        {
                            //If user is admin just add the customers it wants to see as it has access to all and if empty leave blank to show them all
                            if (request.CustomerIds != null)
                            {
                                customerIdsFilter = request.CustomerIds.Select(c => CustomerId.Create(c)).ToList();
                            }
                            if (request.CustomerNumbers != null)
                            {
                                customerNumbersFilter = request.CustomerNumbers.Select(c => CustomerNumber.Create(c)).ToList();
                            }
                        }
                        else
                        {
                            //If user is not admin we must only get the customers of which it has sustainability report access. If the list is empty we must set it to the list of customer permissions it has to filter by it
                            var customerPermissions = await executionContext.GetCustomersPermissionsAsync(true, cancellationToken);
                            if (request.CustomerIds != null && request.CustomerIds.Length > 0)
                            {
                                customerIdsFilter = request.CustomerIds.Where(c =>
                                {
                                    var customerPermission = customerPermissions.Where(p => p.CustomerId.Value == c).FirstOrDefault();
                                    return ((customerPermission != null) && (customerPermission.Permissions.Contains(CustomerPermission.SustainabilityReport)|| customerPermission.Permissions.Contains(CustomerPermission.Admin)));
                                }).Select(c => CustomerId.Create(c)).ToList();
                            }
                            else
                            {
                                //If empty just put all the customers the user has
                                customerIdsFilter = customerPermissions.Where(c => c.Permissions.Contains(CustomerPermission.SustainabilityReport) || c.Permissions.Contains(CustomerPermission.Admin)).Select(c => CustomerId.Create(c.CustomerId.Value)).ToList();
                            }

                            if (request.CustomerNumbers != null && request.CustomerNumbers.Length > 0)
                            {
                                customerNumbersFilter = request.CustomerNumbers.Where(c =>
                                {
                                    var customerPermission = customerPermissions.Where(p => p.CustomerNumber.Value == c).FirstOrDefault();
                                    return ((customerPermission != null) &&
                                            (customerPermission.Permissions.Contains(CustomerPermission
                                                 .SustainabilityReport) ||
                                             customerPermission.Permissions.Contains(CustomerPermission.Admin)));
                                }).Select(c => CustomerNumber.Create(c)).ToList();
                            }
                            else
                            {
                                customerNumbersFilter = customerPermissions
                                    .Where(c => c.Permissions.Contains(CustomerPermission.SustainabilityReport) || c.Permissions.Contains(CustomerPermission.Admin))
                                    .Select(c => CustomerNumber.Create(c.CustomerNumber.Value)).ToList();
                            }

                            if (customerIdsFilter.IsEmpty() || customerNumbersFilter.IsEmpty())
                            {
                                //If the list of customers is still empty for a regular user we must abort as the filter will be empty and we would show all transactions otherwise!
                                throw new BusinessException("A regular user without customers assigned has no transactions");
                            }
                        }

                        var maxColumnsFilter = MaxColumns.Create(30);
                        if (request.MaxColumns != null)
                        {
                            maxColumnsFilter = MaxColumns.Create(request.MaxColumns.Value);
                        }
                        
                        var filteringParameters = SustainabilityAndFuelConsumptionFilteringParameters.Create(datePeriod, productNamesFilter, customerIdsFilter, customerNumbersFilter, maxColumnsFilter);

                        var query = GetSustainabilityReportPdfQuery.Create(filteringParameters);

                        var results = await queryBus.Send<GetSustainabilityReportPdfQuery, GetSustainabilityReportPdfResponse>(query, cancellationToken);

                        return Results.Ok(results);
                    })
                .RequireAuthorization()
                .Produces<GetSustainabilityReportPdfResponse>()
                .WithName("GetSustainabilityReportPdf")
                .WithTags("OutgoingDeclarations");
        }
    }
}