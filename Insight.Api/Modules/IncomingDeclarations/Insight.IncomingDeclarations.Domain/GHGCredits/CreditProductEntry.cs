using Insight.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.IncomingDeclarations.Domain.GHGCredits
{
    public sealed class CreditProductEntry : ValueObject
    {
        public CreditProductName CreditProductName { get; private set; } = CreditProductName.Empty();
        public CreditRawMaterialCode CreditRawMaterialCode { get; private set; } = CreditRawMaterialCode.Empty();
        public CreditRawMaterialDescription CreditRawMaterialDescription { get; private set; } = CreditRawMaterialDescription.Empty();
        public Credits Credits { get; private set; } = Credits.Empty();
        public Incoming Incoming { get; private set; } = Incoming.Empty();
        public OutgoingAllocated OutgoingAllocated { get; private set; } = OutgoingAllocated.Empty();
        public CreditsTransferred CreditsTransferred { get; private set; } = CreditsTransferred.Empty();
        public CreditStartDate CreditStartDate { get; private set; } = CreditStartDate.Empty();
        public CreditEndDate CreditEndDate { get; private set; } = CreditEndDate.Empty();
        public decimal AvailableForAllocation => Credits.Value + Incoming.Value;
        public decimal AvailableCredits => AvailableForAllocation - OutgoingAllocated.Value;
        public decimal Balance => AvailableCredits - CreditsTransferred.Value;

        private CreditProductEntry()
        {
            // Intentionally left empty.
        }

        private CreditProductEntry(CreditProductName creditProductName, CreditRawMaterialCode creditRawMaterialCode, CreditRawMaterialDescription creditRawMaterialDescription, Credits credits, Incoming incoming, OutgoingAllocated outgoingAllocated, CreditsTransferred creditsTransferred, CreditStartDate creditStartDate, CreditEndDate creditEndDate)
        {
            CreditProductName = creditProductName;
            CreditRawMaterialCode = creditRawMaterialCode;
            CreditRawMaterialDescription = creditRawMaterialDescription;
            Credits = credits;
            Incoming = incoming;
            OutgoingAllocated = outgoingAllocated;
            CreditsTransferred = creditsTransferred;
            CreditStartDate = creditStartDate;
            CreditEndDate = creditEndDate;
        }



        public static CreditProductEntry Create(CreditProductName creditProductName, CreditRawMaterialCode creditRawMaterialCode, CreditRawMaterialDescription creditRawMaterialDescription, Credits credits, Incoming incoming, OutgoingAllocated outgoingAllocated, CreditsTransferred creditsTransferred, CreditStartDate creditStartDate, CreditEndDate creditEndDate)
        {
            return new CreditProductEntry(creditProductName, creditRawMaterialCode, creditRawMaterialDescription, credits, incoming, outgoingAllocated, creditsTransferred, creditStartDate, creditEndDate);
        }

        public static CreditProductEntry Empty()
        {
            return new CreditProductEntry();
        }
    }
}