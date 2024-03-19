using System.ComponentModel.DataAnnotations;
using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.IncomingDeclarations.Application.GetIncomingDeclarationsByPageAndPageSize
{
    public sealed class GetIncomingDeclarationsByPageAndPageSizeResponse
    {
        [Required]
        public bool HasMoreDeclarations { get; set; } 
        [Required]
        public decimal TotalAmountOfDeclarations { get; set; }
        [Required]
        public IReadOnlyList<IncomingDeclarationResponse> IncomingDeclarationsByPageAndPageSize { get; private set; }

        public GetIncomingDeclarationsByPageAndPageSizeResponse(
            IReadOnlyList<IncomingDeclarationResponse> incomingDeclarationsByPageAndPageSize,
            bool hasMoreDeclarations,
            decimal totalAmountOfDeclarations)
        {
            IncomingDeclarationsByPageAndPageSize = incomingDeclarationsByPageAndPageSize;
            TotalAmountOfDeclarations = totalAmountOfDeclarations;
            HasMoreDeclarations = hasMoreDeclarations;
        }
    }
    public sealed class IncomingDeclarationResponse
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
        public string Id { get; private set; }
        [Required]
        public string PosNumber { get; private set; }        
        [Required]
        public string CountryOfOrigin { get; private set; }
        [Required]
        public IncomingDeclarationState IncomingDeclarationState { get; private set; }
        [Required]
        public string PlaceOfDispatch { get; private set; }
        [Required]
        public DateOnly DateOfDispatch { get; private set; }
        [Required]
        public decimal Quantity { get; private set; }
        [Required]
        public decimal GhgEmissionSaving { get; private set; }
        
        [Required]
        public decimal RemainingVolume { get; private set; }

        public IncomingDeclarationResponse(string id, string company, string country, string product, string supplier, string rawMaterial, string countryOfOrigin, string posNumber, IncomingDeclarationState incomingDeclarationState, string placeOfDispatch, DateOnly dateOfDispatch, decimal quantity, decimal ghgEmissionSaving, decimal remainingVolume)
        {
            Id = id;
            Company = company;
            Country = country;
            Product = product;
            Supplier = supplier;
            RawMaterial = rawMaterial;
            CountryOfOrigin = countryOfOrigin;
            PosNumber = posNumber;
            IncomingDeclarationState = incomingDeclarationState;
            PlaceOfDispatch = placeOfDispatch;
            DateOfDispatch = dateOfDispatch;
            Quantity = quantity;
            GhgEmissionSaving = ghgEmissionSaving;
            RemainingVolume = remainingVolume;
        }
    }
}