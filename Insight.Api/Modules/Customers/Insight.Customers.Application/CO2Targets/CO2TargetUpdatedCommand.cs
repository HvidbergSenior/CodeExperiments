using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.Customers.Domain;
using Insight.Customers.Domain.Repositories;

namespace Insight.Customers.Application.CO2Targets
{
    public sealed class CO2TargetUpdatedCommand : IInternalCommand, ICommand<ICommandResponse>
    {
        public CO2TargetUpdatedCommand(CustomerNumber customerNumber, CompanyId companyId, CO2Target cO2Target)
        {
            CustomerNumber = customerNumber;
            CompanyId = companyId;
            CO2Target = cO2Target;
        }

        public CustomerNumber CustomerNumber { get; private set; }
        public CompanyId CompanyId { get; private set; }
        public CO2Target CO2Target { get; private set; }

        internal static CO2TargetUpdatedCommand Create(CustomerNumber customerNumber, CompanyId companyId, CO2Target cO2Target)
        {
            return new CO2TargetUpdatedCommand(customerNumber, companyId, cO2Target);
        }
    }

    internal class CO2TargetUpdatedCommandHandler : ICommandHandler<CO2TargetUpdatedCommand, ICommandResponse>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IUnitOfWork unitOfWork;

        public CO2TargetUpdatedCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            this.customerRepository = customerRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ICommandResponse> Handle(CO2TargetUpdatedCommand request, CancellationToken cancellationToken)
        {   
            var customer = await customerRepository.GetByCustomerNumberAndCompanyIdAsync(request.CustomerNumber,request.CompanyId, cancellationToken);
            if (customer == null)
            {
                throw new NotFoundException("Customer not found");
            }
            
            customer.SetCO2Target(request.CO2Target);
            await customerRepository.Update(customer, cancellationToken);
            await customerRepository.SaveChanges(cancellationToken);
            await unitOfWork.Commit(cancellationToken);
            return EmptyCommandResponse.Default;
        }
    }
    internal class CO2TargetUpdatedCommandAuthorizer : IAuthorizer<CO2TargetUpdatedCommand>
    {
        private readonly IExecutionContext executionContext;

        public CO2TargetUpdatedCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public Task<AuthorizationResult> Authorize(CO2TargetUpdatedCommand instance, CancellationToken cancellation)
        {
            // Todo: This fails when running from the inbox.
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}
