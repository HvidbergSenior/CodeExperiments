using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Exceptions;
using Insight.BusinessCentralEntities.Domain;
using Insight.BusinessCentralEntities.Domain.Products;
using Insight.BusinessCentralEntities.Integration.GetProductByNumber;

namespace Insight.BusinessCentralEntities.Application.GetProductByNumber
{
    internal class GetProductByNumberQueryHandler : IQueryHandler<GetProductByNumberQuery, ProductDto>
    {
        private readonly IProductRepository productRepository;

        public GetProductByNumberQueryHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<ProductDto> Handle(GetProductByNumberQuery request, CancellationToken cancellationToken)
        {
            var productNumber = ProductNumber.Create(request.ProductNumber);
            var companyId = CompanyId.Create(request.CompanyId);

            var product = await productRepository.GetProductByProductNumberAndCompanyIdAsync(productNumber, companyId, cancellationToken);
            if (product == null)
            {
                throw new NotFoundException($"Product with product number {productNumber.Value} not found");
            }
            return new ProductDto(product.ProductNumber.Value, product.ItemCategoryCode.Value, product.CompanyId.Value, product.CompanyName.Value);
        }
    }

    internal class GetProductByNumberQueryAuthorizer : IAuthorizer<GetProductByNumberQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetProductByNumberQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetProductByNumberQuery query,
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
