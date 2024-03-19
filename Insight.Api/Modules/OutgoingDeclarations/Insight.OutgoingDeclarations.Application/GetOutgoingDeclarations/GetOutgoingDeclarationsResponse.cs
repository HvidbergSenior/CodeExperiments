using System.ComponentModel.DataAnnotations;

namespace Insight.OutgoingDeclarations.Application.GetOutgoingDeclarations
{
    public sealed class GetOutgoingDeclarationsResponse
    {
        public IReadOnlyList<OutgoingDeclarationsResponse> OutgoingDeclarationsResponses { get; set; }
        public GetOutgoingDeclarationsResponse(IReadOnlyList<OutgoingDeclarationsResponse> outgoingDeclarationsResponses
        )
        {
            OutgoingDeclarationsResponses = outgoingDeclarationsResponses;
        }
    }
    public sealed class OutgoingDeclarationsResponse
    {
        [Required]
        public Guid OutgoingDeclarationId { get; private set; }
        [Required]
        public string Country { get; private set; }
        [Required]
        public string Product { get; private set; }
        [Required]
        public string CustomerNumber { get; private set; }
        [Required]
        public string CustomerName { get; private set; }
        
        public OutgoingDeclarationsResponse(Guid outgoingDeclarationId, string country, string product, string customerNumber, string customerName)
        {
            OutgoingDeclarationId = outgoingDeclarationId;
            Country = country;
            Product = product;
            CustomerNumber = customerNumber;
            CustomerName = customerName;
        }
    }
}