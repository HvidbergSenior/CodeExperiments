using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Domain;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using Insight.Customers.Domain;
using Insight.Customers.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Insight.Customers.Application.AllowedRawMaterials
{
    public sealed class AllowedRawMaterialUpdatedCommand : IInternalCommand, ICommand<ICommandResponse>
    {
        internal CompanyId CompanyId { get; set; }
        internal CustomerNumber CustomerNumber { get; set; }
        internal Domain.AllowedRawMaterials AllowedRawMaterials { get; set; }

#pragma warning disable CS8618
        private AllowedRawMaterialUpdatedCommand()
#pragma warning restore CS8618
        {
            //Left empty for serialization purposes
        }

        private AllowedRawMaterialUpdatedCommand(CompanyId companyId, CustomerNumber customerNumber, Domain.AllowedRawMaterials allowedRawMaterials)
        {
            CompanyId = companyId;
            CustomerNumber = customerNumber;
            AllowedRawMaterials = allowedRawMaterials;
        }

        public static AllowedRawMaterialUpdatedCommand Create(CompanyId companyId, CustomerNumber customerNumber, Domain.AllowedRawMaterials allowedRawMaterials)
        {
            return new AllowedRawMaterialUpdatedCommand(companyId, customerNumber, allowedRawMaterials);
        }
    }

    internal class AllowedRawMaterialUpdatedCommandHandler : ICommandHandler<AllowedRawMaterialUpdatedCommand, ICommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICustomerRepository customerRepository;
        private readonly ILogger<AllowedRawMaterialUpdatedCommandHandler> logger;

        public AllowedRawMaterialUpdatedCommandHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository, ILogger<AllowedRawMaterialUpdatedCommandHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.customerRepository = customerRepository;
            this.logger = logger;
        }

        public async Task<ICommandResponse> Handle(AllowedRawMaterialUpdatedCommand request, CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetByCustomerNumberAndCompanyIdAsync(request.CustomerNumber, request.CompanyId, cancellationToken);
            if (customer == null)
            {
                throw new NotFoundException("Customer not found");
            }

            customer.SetAllowedRawMaterials(request.AllowedRawMaterials);

            await customerRepository.Update(customer, cancellationToken);
            await customerRepository.SaveChanges(cancellationToken);
            await unitOfWork.Commit(cancellationToken);

            logger.LogInformation("Allowed raw materials updated for customer {CustomerName} ({CustomerNumber}) in company {CompanyId}", customer.CustomerDetails.CustomerName.Value, customer.CustomerDetails.CustomerNumber.Value, customer.CompanyId.Value);
            return EmptyCommandResponse.Default;
        }
    }
    internal class AllowedRawMaterialUpdatedCommandAuthorizer : IAuthorizer<AllowedRawMaterialUpdatedCommand>
    {
        public Task<AuthorizationResult> Authorize(AllowedRawMaterialUpdatedCommand instance, CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}
