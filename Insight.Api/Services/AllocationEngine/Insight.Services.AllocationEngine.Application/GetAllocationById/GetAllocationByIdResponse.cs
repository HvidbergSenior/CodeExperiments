using System.ComponentModel.DataAnnotations;

namespace Insight.Services.AllocationEngine.Application.GetAllocationById
{
    public sealed class GetAllocationByIdResponse
    {
        public AllocationByIdResponse AllocationByIdResponse { get; set; }
        public IReadOnlyList<AllocationIncomingDeclarationDto> AllocationIncomingDeclarationDtos { get; set; }

        public GetAllocationByIdResponse(AllocationByIdResponse allocationByIdResponse, IReadOnlyList<AllocationIncomingDeclarationDto> allocationIncomingDeclarationDtos)
        {
            AllocationByIdResponse = allocationByIdResponse;
            AllocationIncomingDeclarationDtos = allocationIncomingDeclarationDtos;
        }
    }
    public sealed class AllocationByIdResponse
    {
        [Required]
        public required Guid Id { get; set; }
        [Required]
        public required string Customer { get; set; }
        [Required]
        public required string CustomerNumber { get; set; }
        [Required]
        public required string Country { get; set; }
        [Required]
        public required string Product { get; set; }
        [Required]
        public required decimal Volume { get; set; }
        
    }
    
    public sealed class AllocationIncomingDeclarationDto
    {
        public string Company { get; private set; }
        public string Country { get; private set; }
        public string Product { get; private set; }
        public string Supplier { get; private set; }
        public string RawMaterial { get; private set; }
        public string PosNumber { get; private set; }
        public string CountryOfOrigin { get; private set; }
        public string PlaceOfDispatch { get; private set; }
        public DateOnly DateOfDispatch { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal GhgEmissionSaving { get; private set; }

        public AllocationIncomingDeclarationDto(
            string company,
            string country,
            string product,
            string supplier,
            string rawMaterial,
            string posNumber,
            string countryOfOrigin,
            string placeOfDispatch,
            DateOnly dateOfDispatch,
            decimal quantity,
            decimal ghgEmissionSaving)
        {
            Company = company;
            Country = country;
            Product = product;
            Supplier = supplier;
            RawMaterial = rawMaterial;
            PosNumber = posNumber;
            CountryOfOrigin = countryOfOrigin;
            PlaceOfDispatch = placeOfDispatch;
            DateOfDispatch = dateOfDispatch;
            Quantity = quantity;
            GhgEmissionSaving = ghgEmissionSaving;
        }
    }
}