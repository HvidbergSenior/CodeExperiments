using System.ComponentModel.DataAnnotations;
using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.Services.AllocationEngine.Application.GetAllocationSuggestions
{
    public sealed class GetAllocationSuggestionsResponse
    {
        [Required]
        public bool HasMoreSuggestions { get; set; }
        [Required]
        public decimal TotalAmountOfSuggestions { get; set; }
        [Required]
        public IReadOnlyList<SuggestionResponse> Suggestions { get; private set; }

        public GetAllocationSuggestionsResponse(
            IReadOnlyList<SuggestionResponse> suggestions,
            bool hasMoreSuggestions,
            decimal totalAmountOfSuggestions)
        {
            Suggestions = suggestions;
            TotalAmountOfSuggestions = totalAmountOfSuggestions;
            HasMoreSuggestions = hasMoreSuggestions;
        }
    }

    public sealed class SuggestionResponse
    {
        [Required]
        public required Guid Id { get; set; }
        [Required]
        public required string PosNumber { get; set; }
        [Required]
        public required DateOnly Period { get; set; }
        [Required]
        public required string Storage { get; set; }
        [Required]
        public required string Company { get; set; }
        [Required]
        public required string Product { get; set; }
        [Required]
        public required string RawMaterial { get; set; }
        [Required]
        public required string Supplier { get; set; }
        [Required]
        public required string CountryOfOrigin { get; set; }
        [Required]
        public required string Country { get; set; }        
        [Required]
        public required IncomingDeclarationState IncomingDeclarationState { get; set; }
        [Required]
        public decimal VolumeAvailable { get; set; }
        [Required]
        public decimal Volume { get; set; }
        [Required]
        public decimal GHGReduction { get; set; }
        [Required]
        public bool HasWarnings => Warnings.Length != 0;
        [Required]
        public required string[] Warnings { get; set; } = Array.Empty<string>();
    }
}
