using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Infrastructure;
using Insight.FuelTransactions.Domain;
using ProductName = Insight.BuildingBlocks.Domain.ProductName;

namespace Insight.FuelTransactions.Application.CreateFuelTransaction
{
    public sealed class CreateFuelTransactionCommand : IInternalCommand, ICommand<ICommandResponse>
    {
        internal FuelTransactionId FuelTransactionId { get; set; }
        internal StationNumber StationNumber { get; set; }
        internal StationName StationName { get; set; }
        internal CustomerName CustomerName { get; set; }
        internal FuelTransactionDate FuelTransactionDate { get; set; }
        internal FuelTransactionTime FuelTransactionTime { get; set; }
        internal ProductNumber ProductNumber { get; set; }
        internal ProductName ProductName { get; set; }
        internal Quantity Quantity { get; set; }
        internal Odometer Odometer { get; set; }
        internal CardTypeId CardTypeId { get; set; }
        internal CardAcceptanceId CardAcceptanceId { get; set; }
        internal DriverCardNumber DriverCardNumber { get; set; }
        internal VehicleCardNumber VehicleCardNumber { get; set; }
        internal CustomerNumber CustomerNumber { get; set; }
        internal Pump Pump { get; set; }
        internal FuelTransactionPosSystem FuelTransactionPosSystem { get; set; }
        internal SourceETag SourceETag { get; set; }
        internal FuelTransactionCountry Country { get; set; }
        internal SourceSystemPropertyBag SourceSystemPropertyBag { get; set; }
        internal SourceSystemId SourceSystemId { get; set; }
        internal Location Location { get; set; }
        internal CustomerType CustomerType { get; set; }
        internal CustomerSegment CustomerSegment { get; set; }
        internal CompanyName CompanyName { get; set; }
        internal AccountNumber AccountNumber { get; set; }
        internal AccountName AccountName { get; set; }  
        internal AccountCustomerId AccountCustomerId { get; set; }
        internal ProductDescription ProductDescription { get; set; }
        internal ShipToLocation ShipToLocation { get; set; }
        internal Driver Driver { get; set; }

#pragma warning disable CS8618
        private CreateFuelTransactionCommand()
#pragma warning restore CS8618
        {
            //Left empty for serialization purposes
        }

        private CreateFuelTransactionCommand(
            FuelTransactionId fuelTransactionId,
            StationNumber stationNumber,
            StationName stationName,
            CustomerName customerName,
            FuelTransactionDate fuelTransactionDate,
            FuelTransactionTime fuelTransactionTime,
            ProductNumber productNumber,
            ProductName productName,
            Quantity quantity,
            Odometer odometer,
            CardTypeId cardTypeId,
            CardAcceptanceId cardAcceptanceId,
            DriverCardNumber driverCardNumber,
            VehicleCardNumber vehicleCardNumber,
            CustomerNumber customerNumber,
            Pump pump,
            FuelTransactionPosSystem fuelTransactionPosSystem,
            SourceETag sourceETag,
            FuelTransactionCountry country,
            SourceSystemPropertyBag sourceSystemPropertyBag,
            SourceSystemId sourceSystemId,
            Location location,
            CustomerType customerType,
            CustomerSegment customerSegment,
            CompanyName companyName,
            AccountNumber accountNumber,
            AccountName accountName,
            AccountCustomerId accountCustomerId,
            ProductDescription productDescription,
            ShipToLocation shipToLocation,
            Driver driver)
        {
            FuelTransactionId = fuelTransactionId;
            StationNumber = stationNumber;
            StationName = stationName;
            CustomerName = customerName;
            FuelTransactionDate = fuelTransactionDate;
            FuelTransactionTime = fuelTransactionTime;
            ProductNumber = productNumber;
            ProductName = productName;
            Quantity = quantity;
            Odometer = odometer;
            CardTypeId = cardTypeId;
            CardAcceptanceId = cardAcceptanceId;
            DriverCardNumber = driverCardNumber;
            VehicleCardNumber = vehicleCardNumber;
            CustomerNumber = customerNumber;
            Pump = pump;
            FuelTransactionPosSystem = fuelTransactionPosSystem;
            SourceETag = sourceETag;
            Country = country;
            SourceSystemPropertyBag = sourceSystemPropertyBag;
            SourceSystemId = sourceSystemId;
            Location = location;
            CustomerType = customerType;
            CustomerSegment = customerSegment;
            CompanyName = companyName;
            AccountNumber = accountNumber;
            AccountName = accountName;
            AccountCustomerId = accountCustomerId;
            ProductDescription = productDescription;
            ShipToLocation = shipToLocation;
            Driver = driver;
        }

        public static CreateFuelTransactionCommand Create(
            FuelTransactionId fuelTransactionId,
            StationNumber stationNumber,
            StationName stationName,
            CustomerName customerName,
            FuelTransactionDate fuelTransactionDate,
            FuelTransactionTime fuelTransactionTime,
            ProductNumber productNumber,
            ProductName productName,
            Quantity quantity,
            Odometer odometer,
            CardTypeId cardTypeId,
            CardAcceptanceId cardAcceptanceId,
            DriverCardNumber driverCardNumber,
            VehicleCardNumber vehicleCardNumber,
            CustomerNumber customerNumber,
            Pump pump,
            FuelTransactionPosSystem fuelTransactionPosSystem,
            SourceETag sourceETag,
            FuelTransactionCountry country,
            SourceSystemPropertyBag sourceSystemPropertyBag,
            SourceSystemId sourceSystemId,
            Location location,
            CustomerType customerType,
            CustomerSegment customerSegment,
            CompanyName companyName,
            AccountNumber accountNumber,
            AccountName accountName,
            AccountCustomerId accountCustomerId,
            ProductDescription productDescription,
            ShipToLocation shipToLocation,
            Driver driver)
        {
            return new CreateFuelTransactionCommand(
                fuelTransactionId,
                stationNumber,
                stationName,
                customerName,
                fuelTransactionDate,
                fuelTransactionTime,
                productNumber,
                productName,
                quantity,
                odometer,
                cardTypeId,
                cardAcceptanceId,
                driverCardNumber,
                vehicleCardNumber,
                customerNumber,
                pump,
                fuelTransactionPosSystem,
                sourceETag,
                country,
                sourceSystemPropertyBag,
                sourceSystemId,
                location,
                customerType,
                customerSegment,
                companyName,
                accountNumber, 
                accountName, 
                accountCustomerId,
                productDescription,
                shipToLocation,
                driver);
        }
    }

    internal class CreateFuelTransactionCommandHandler : ICommandHandler<CreateFuelTransactionCommand, ICommandResponse>
    {
        private readonly IFuelTransactionsRepository fuelTransactionRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateFuelTransactionCommandHandler(IUnitOfWork unitOfWork,
            IFuelTransactionsRepository fuelTransactionRepository)
        {
            this.unitOfWork = unitOfWork;
            this.fuelTransactionRepository = fuelTransactionRepository;
        }

        public async Task<ICommandResponse> Handle(CreateFuelTransactionCommand request,
            CancellationToken cancellationToken)
        {
            var transaction = CreateNewTransaction(request);
            await fuelTransactionRepository.Add(transaction, cancellationToken);

            await fuelTransactionRepository.SaveChanges(cancellationToken);
            await unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }

        private static FuelTransaction CreateNewTransaction(CreateFuelTransactionCommand request)
        {
            return FuelTransaction.Create(
                request.FuelTransactionId,
                request.FuelTransactionPosSystem,
                request.StationNumber,
                request.StationName,
                request.FuelTransactionDate,
                request.FuelTransactionTime,
                request.ProductNumber,
                request.ProductName,
                request.Quantity,
                request.Odometer,
                request.DriverCardNumber,
                request.VehicleCardNumber,
                request.CustomerNumber,
                request.CustomerName,
                request.SourceETag,
                request.Country,
                request.SourceSystemPropertyBag,
                request.SourceSystemId,
                request.Location,
                request.CustomerType,
                request.CustomerSegment,
                request.CompanyName,
                request.AccountNumber,
                request.AccountName, 
                request.AccountCustomerId,
                request.ProductDescription,
                request.ShipToLocation,
                request.Driver);
        }
    }

    internal class CreateFuelTransactionCommandAuthorizer : IAuthorizer<CreateFuelTransactionCommand>
    {
        private readonly IExecutionContext executionContext;

        public CreateFuelTransactionCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(CreateFuelTransactionCommand query,
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