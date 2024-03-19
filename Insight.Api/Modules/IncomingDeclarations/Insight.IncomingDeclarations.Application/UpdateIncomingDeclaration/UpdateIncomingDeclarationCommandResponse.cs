using System.ComponentModel.DataAnnotations;
using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.IncomingDeclarations.Application.UpdateIncomingDeclaration
{
    public sealed class UpdateIncomingDeclarationCommandResponse
    {
        [Required] 
        public UpdateIncomingDeclarationResponse UpdateIncomingDeclarationResponse { get; set; }

        public UpdateIncomingDeclarationCommandResponse(UpdateIncomingDeclarationResponse updateIncomingDeclarationResponse)
        {
            UpdateIncomingDeclarationResponse = updateIncomingDeclarationResponse;
        }
    }
    public sealed class UpdateIncomingDeclarationResponse
    {
        [Required]
        public string Company { get; private set; }
        [Required]
        public string Country { get; private set; }
        [Required]
        public string Product { get; private set; }
        [Required]
        public string Supplier { get; private set; }
        [Required]
        public string RawMaterial { get; private set; }
        [Required]
        public Guid UpdatedIncomingDeclarationId { get; private set; }
        [Required]
        public string PosNumber { get; private set; }        
        [Required]
        public string CountryOfOrigin { get; private set; }
        [Required]
        public IncomingDeclarationState IncomingDeclarationState { get; private set; }
       
        public UpdateIncomingDeclarationResponse(Guid updatedIncomingDeclarationId, string company, string country, string product, string supplier, string rawMaterial, string countryOfOrigin, string posNumber, IncomingDeclarationState incomingDeclarationState)
        {
            UpdatedIncomingDeclarationId = updatedIncomingDeclarationId;
            Company = company;
            Country = country;
            Product = product;;
            Supplier = supplier;
            RawMaterial = rawMaterial;
            CountryOfOrigin = countryOfOrigin;
            PosNumber = posNumber;
            IncomingDeclarationState = incomingDeclarationState;
        }
    }
}
