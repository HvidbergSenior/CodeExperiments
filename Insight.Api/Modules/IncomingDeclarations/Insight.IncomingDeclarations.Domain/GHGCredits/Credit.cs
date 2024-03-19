using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Domain.GHGCredits
{
    public sealed class Credit : Entity
    {
        public CreditId CreditId { get; private set; } = CreditId.Empty();
        public CreditCompanyId CreditCompanyId { get; private set; } = CreditCompanyId.Empty();
        public CreditCompanyName CreditCompanyName { get; private set; } = CreditCompanyName.Empty();
        public CreditLocation CreditLocation { get; private set; } = CreditLocation.Empty();
        public List<CreditProductEntry> CreditProductEntries { get; private set; } = new List<CreditProductEntry>();
        
        private Credit()
        {
            Id = CreditId.Value;
        }

        private Credit(CreditId creditId, CreditCompanyId creditCompanyId, CreditCompanyName creditCompanyName, CreditLocation creditLocation)
        {
            Id = creditId.Value;
            CreditId = creditId;
            CreditCompanyId = creditCompanyId;
            CreditCompanyName = creditCompanyName;
            CreditLocation = creditLocation;
        }

        public static Credit Create(CreditId creditId, CreditCompanyId creditCompanyId, CreditCompanyName creditCompanyName, CreditLocation creditLocation)
        {
            return new Credit(creditId, creditCompanyId, creditCompanyName, creditLocation);
        }
    }
}
