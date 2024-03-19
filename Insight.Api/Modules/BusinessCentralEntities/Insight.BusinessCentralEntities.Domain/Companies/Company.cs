using Insight.BuildingBlocks.Domain;

namespace Insight.BusinessCentralEntities.Domain.Companies
{
    public sealed class Company : Entity
    {
        public CompanyId CompanyId { get; private set; } = CompanyId.Empty();
        public CompanyName CompanyName { get; private set; } = CompanyName.Empty();
        public SourcesystemEtag SourcesystemEtag { get; private set; } = SourcesystemEtag.Empty();

        private Company(CompanyId companyId, CompanyName companyName, SourcesystemEtag sourcesystemEtag)
        {
            CompanyId = companyId;
            CompanyName = companyName;
            SourcesystemEtag = sourcesystemEtag;
        }

        private Company()
        {
            // Intentinally left blank
        }

        public static Company Create(CompanyId companyId, CompanyName companyName, SourcesystemEtag sourcesystemEtag)
        {
            return new Company(companyId, companyName, sourcesystemEtag);
        }

        public void Update(CompanyName companyName, SourcesystemEtag sourcesystemEtag)
        {
            CompanyName = companyName;
            SourcesystemEtag = sourcesystemEtag;
        }
    }
}
