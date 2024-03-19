using Insight.BuildingBlocks.Infrastructure;

namespace Insight.BusinessCentralEntities.Domain.Companies
{
    public interface ICompanyRepository : IRepository<Company>, IReadonlyRepository<Company>
    {
        public Task<Company?> GetCompanyByCompanyId(CompanyId companyId, CancellationToken cancellationToken = default);
        public Task<IEnumerable<Company>> GetAllCompanies(CancellationToken cancellationToken = default);
        public Task<IEnumerable<CompanyName>> GetCompanyNames(CancellationToken cancellationToken = default);
    }
}
