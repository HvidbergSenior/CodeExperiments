using System.ComponentModel.DataAnnotations;

namespace Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationsByCustomerId
{
    public sealed class GetOutgoingDeclarationsByCustomerIdResponse
    {
        public IEnumerable<OutgoingDeclarationsByCustomerIdResponse> OutgoingDeclarationByCustomerIdResponse { get; set; }

        public GetOutgoingDeclarationsByCustomerIdResponse(IEnumerable<OutgoingDeclarationsByCustomerIdResponse> outgoingDeclarationByCustomerIdResponse)
        {
            OutgoingDeclarationByCustomerIdResponse = outgoingDeclarationByCustomerIdResponse;
        }
    }
    public sealed class OutgoingDeclarationsByCustomerIdResponse
    {
        [Required]
        public string OutgoingDeclarationId { get; private set; }
        [Required]
        public string Country { get; private set; }
        [Required]
        public string Product { get; private set; }
        [Required]
        public string CustomerNumber { get; private set; }
        [Required]
        public string CustomerName { get; private set; }
        public OutgoingDeclarationsByCustomerIdResponse(string outgoingDeclarationId, string country, string product, string customerNumber, string customerName)
        {
            OutgoingDeclarationId = outgoingDeclarationId;
            Country = country;
            Product = product;
            CustomerNumber = customerNumber;
            CustomerName = customerName;
        }
    }
}