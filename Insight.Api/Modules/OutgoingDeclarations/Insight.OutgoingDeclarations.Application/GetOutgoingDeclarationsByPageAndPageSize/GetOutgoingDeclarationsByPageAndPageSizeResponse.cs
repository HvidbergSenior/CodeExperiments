using System.ComponentModel.DataAnnotations;

namespace Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationsByPageAndPageSize
{
    public sealed class GetOutgoingDeclarationsByPageAndPageSizeResponse
    {
        [Required]
        public bool HasMoreDeclarations { get; set; } 
        [Required]
        public decimal TotalAmountOfDeclarations { get; set; }
        [Required]
        public IReadOnlyList<OutgoingDeclarationResponse> OutgoingDeclarationsByPageAndPageSizeResponse { get; private set; }

        public GetOutgoingDeclarationsByPageAndPageSizeResponse(IReadOnlyList<OutgoingDeclarationResponse> outgoingDeclarationsByPageAndPageSizeResponse, bool hasMoreDeclarations, decimal totalAmountOfDeclarations)
        {
            OutgoingDeclarationsByPageAndPageSizeResponse = outgoingDeclarationsByPageAndPageSizeResponse;
            TotalAmountOfDeclarations = totalAmountOfDeclarations;
            HasMoreDeclarations = hasMoreDeclarations;
        }
    }
    public sealed class OutgoingDeclarationResponse
    {
        [Required] public Guid OutgoingDeclarationId { get; set; }
        [Required] public string Country { get; set; } = string.Empty;
        [Required] public string Product { get; set; }
        [Required] public string CustomerNumber { get; set; } = string.Empty;
        [Required] public string CustomerName { get; set; } = string.Empty;
        [Required] public decimal VolumeTotal { get; set; }
        [Required] public decimal AllocationTotal { get; set; }
        [Required] public decimal GhgReduction { get; set; }
        [Required] public decimal FossilFuelComparatorgCO2EqPerMJ { get; set; }
        [Required] public IReadOnlyList<Guid> IncomingDeclarationIds { get; set; }


        public OutgoingDeclarationResponse(
            Guid outgoingDeclarationId, 
            string country, 
            string product,
            string customerNumber,
            string customerName,
            decimal volumeTotal,
            decimal allocationTotal,
            decimal ghgReduction,
            decimal fossilFuelComparatorgCO2EqPerMJ, 
            IReadOnlyList<Guid> incomingDeclarationIds)
        {
            OutgoingDeclarationId = outgoingDeclarationId;
            Country = country;
            Product = product;
            CustomerNumber = customerNumber;
            CustomerName = customerName;
            VolumeTotal = volumeTotal;
            AllocationTotal = allocationTotal;
            GhgReduction = ghgReduction;
            FossilFuelComparatorgCO2EqPerMJ = fossilFuelComparatorgCO2EqPerMJ;
            IncomingDeclarationIds = incomingDeclarationIds;
        }
    }
}