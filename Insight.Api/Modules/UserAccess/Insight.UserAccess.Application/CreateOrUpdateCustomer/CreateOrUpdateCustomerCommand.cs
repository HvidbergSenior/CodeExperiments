using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Infrastructure;
using Insight.UserAccess.Domain.User;
using Microsoft.Extensions.Logging;

namespace Insight.UserAccess.Application.CreateOrUpdateCustomer;

    public sealed class CreateOrUpdateCustomerCommand : IInternalCommand, ICommand<ICommandResponse>
    {
        internal CustomerId CustomerId { get; set; }
        internal CustomerNumber CustomerNumber { get; set; }
        internal CustomerName CustomerName { get; set; }

#pragma warning disable CS8618
        private CreateOrUpdateCustomerCommand()
#pragma warning restore CS8618
        {
            //Left empty for serialization purposes
        }

        private CreateOrUpdateCustomerCommand(
            CustomerId customerId,
            CustomerNumber customerNumber,
            CustomerName customerName)
        {
            CustomerId = customerId;
            CustomerNumber = customerNumber;
            CustomerName = customerName;
        }

        public static CreateOrUpdateCustomerCommand Create(
            CustomerId customerId,
            CustomerNumber customerNumber,
            CustomerName customerName
        )
        {
            return new CreateOrUpdateCustomerCommand(
                customerId,
                customerNumber,
                customerName);
        }
    }

    internal class CreateOrUpdateCustomerCommandHandler : ICommandHandler<CreateOrUpdateCustomerCommand, ICommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;
        private readonly ILogger<CreateOrUpdateCustomerCommandHandler> logger;

        public CreateOrUpdateCustomerCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, ILogger<CreateOrUpdateCustomerCommandHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public async Task<ICommandResponse> Handle(CreateOrUpdateCustomerCommand request,
            CancellationToken cancellationToken)
        {
            await userRepository.UpdateCustomers(request.CustomerId, request.CustomerNumber, request.CustomerName, cancellationToken);
            await userRepository.SaveChanges(cancellationToken);
            await unitOfWork.Commit(cancellationToken);

            return EmptyCommandResponse.Default;
        }
    }
    internal class CreateOrUpdateCustomerCommandAuthorizer : IAuthorizer<CreateOrUpdateCustomerCommand>
    {
        private readonly IExecutionContext executionContext;

        public CreateOrUpdateCustomerCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public Task<AuthorizationResult> Authorize(CreateOrUpdateCustomerCommand query,
            CancellationToken cancellation)
        {
            // Todo: This fails when running from the inbox.
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }