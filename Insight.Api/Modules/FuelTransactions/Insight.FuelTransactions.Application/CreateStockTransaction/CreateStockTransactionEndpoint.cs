using Insight.BuildingBlocks.Application.Commands;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Domain.Stock;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Insight.FuelTransactions.Application.CreateStockTransaction
{
    public static class CreateStockTransactionEndpoint
    {
        public static void MapCreateStockTransactionEndpoint(this IEndpointRouteBuilder endpoint)
        {
            endpoint
                .MapPost(
                    StockTransactionsEndpointUrls.CREATE_STOCK_TRANSACTIONS_ENDPOINT,
                    async (
                        CreateStockTransactionRequest request,                        
                        CancellationToken cancellationToken,
                        ICommandBus commandBus
                    ) =>
                    {
                        var productNumber = ProductNumber.Create(request.ProductNumber);
                        var location = Location.Create(request.Location);
                        var quantity = Quantity.Create(request.Quantity);
                        var country = FuelTransactionCountry.Create(request.Country);
                        var companyId = StockCompanyId.Create(request.CompanyId);
                        var command = CreateStockTransactionCommand.Create(productNumber, location, country, quantity, request.TransactionDate, companyId);
                        await commandBus.Send(command, cancellationToken);
                        return Results.Ok();
                    })
                .RequireAuthorization()
                .WithName("CreateStockTransaction")
                .WithTags("StockTransactions");
        }
    }
}

