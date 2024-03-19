using Insight.BuildingBlocks.Events;
using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.BusinessCentralEntities.Domain;
using Insight.BusinessCentralEntities.Domain.Companies;
using Marten;

namespace Insight.BusinessCentralEntities.Infrastructure
{
    public class CompanyRepository : MartenDocumentRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
        }

        public async Task<IEnumerable<Company>> GetAllCompanies(CancellationToken cancellationToken = default)
        {
            return await Query().ToListAsync(cancellationToken);
        }

        public async Task<Company?> GetCompanyByCompanyId(CompanyId companyId, CancellationToken cancellationToken = default)
        {
            return await Query().FirstOrDefaultAsync(p => p.CompanyId.Value == companyId.Value, cancellationToken);
        }

        public async Task<IEnumerable<CompanyName>> GetCompanyNames(CancellationToken cancellationToken = default)
        {
            return await Query().Select(p => p.CompanyName).Distinct().ToListAsync(cancellationToken);
        }
    }
}
