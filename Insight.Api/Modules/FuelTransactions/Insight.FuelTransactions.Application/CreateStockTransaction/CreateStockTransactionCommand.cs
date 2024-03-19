using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Infrastructure;
using Insight.BusinessCentralEntities.Integration.GetProductByNumber;
using Insight.FuelTransactions.Domain;
using Insight.FuelTransactions.Domain.Stock;

namespace Insight.FuelTransactions.Application.CreateStockTransaction
{
    public sealed class CreateStockTransactionCommand : ICommand<ICommandResponse>
    {
        private CreateStockTransactionCommand(ProductNumber productNumber, Location location, FuelTransactionCountry fuelTransactionCountry, Quantity quantity, DateOnly transactionDate, StockCompanyId stockCompanyId)
        {
            ProductNumber = productNumber;
            Location = location;
            FuelTransactionCountry = fuelTransactionCountry;
            Quantity = quantity;
            TransactionDate = transactionDate;
            StockCompanyId = stockCompanyId;
        }

        public ProductNumber ProductNumber { get; private set; } = ProductNumber.Empty();
        public Location Location { get; private set; } = Location.Empty();
        public FuelTransactionCountry FuelTransactionCountry { get; private set; } = FuelTransactionCountry.Empty();
        public Quantity Quantity { get; private set; } = Quantity.Empty();
        public DateOnly TransactionDate { get; private set; }
        public StockCompanyId StockCompanyId { get; private set; } = StockCompanyId.Empty();

        public static CreateStockTransactionCommand Create(ProductNumber productNumber, Location location, FuelTransactionCountry fuelTransactionCountry, Quantity quantity, DateOnly transactionDate, StockCompanyId stockCompanyId)
        {
            return new CreateStockTransactionCommand(productNumber, location, fuelTransactionCountry, quantity, transactionDate, stockCompanyId);
        }
    }

    internal class CreateStockTransactionCommandHandler : ICommandHandler<CreateStockTransactionCommand, ICommandResponse>
    {   
        private readonly IUnitOfWork unitOfWork;
        private readonly IStockTransactionsRepository stockTransactionsRepository;
        private readonly IQueryBus queryBus;

        public CreateStockTransactionCommandHandler(IUnitOfWork unitOfWork, IStockTransactionsRepository stockTransactionsRepository, IQueryBus queryBus)
        {
            this.unitOfWork = unitOfWork;
            this.stockTransactionsRepository = stockTransactionsRepository;
            this.queryBus = queryBus;
        }

        public async Task<ICommandResponse> Handle(CreateStockTransactionCommand request, CancellationToken cancellationToken)
        {
            var productQuery = GetProductByNumberQuery.Create(request.ProductNumber.Value, request.StockCompanyId.Value);

            var product = await queryBus.Send<GetProductByNumberQuery, ProductDto>(productQuery, cancellationToken);

            var stockTransaction = StockTransaction.Create(StockTransactionId.Create(Guid.NewGuid()), request.Location, request.ProductNumber, ProductName.Create(product.ProductName), request.Quantity, StockTransactionDate.Create(request.TransactionDate), request.StockCompanyId, CompanyName.Create(product.CompanyName));
            await stockTransactionsRepository.Add(stockTransaction, cancellationToken);
            await stockTransactionsRepository.SaveChanges(cancellationToken);
            await unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }

    internal class CreateStockTransactionCommandAuthorizer : IAuthorizer<CreateStockTransactionCommand>
    {
        private readonly IExecutionContext executionContext;

        public CreateStockTransactionCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(CreateStockTransactionCommand query,
            CancellationToken cancellation)
        {
            if (await executionContext.GetAdminPrivileges(cancellation))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}
