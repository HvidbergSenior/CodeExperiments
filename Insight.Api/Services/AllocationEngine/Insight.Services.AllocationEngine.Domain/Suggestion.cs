using Insight.IncomingDeclarations.Domain.Incoming;

namespace Insight.Services.AllocationEngine.Domain
{
    public class Suggestion
    {
        public required Guid Id { get; set; }
        public required string PosNumber { get; set; }
        public required DateOnly Period { get; set; }
        public required string Storage { get; set; }
        public required string Company { get; set; }
        public required string Product { get; set; }
        public required string RawMaterial { get; set; }
        public required string Supplier { get; set; }
        public required string CountryOfOrigin { get; set; }
        public required string Country { get; set; }
        public required IncomingDeclarationState IncomingDeclarationState { get; set; }
        public decimal VolumeAvailable { get; set; }
        public decimal Volume { get; set; }
        public decimal GHGReduction { get; set; }
        public bool HasWarnings => Warnings.Length != 0;
        public required string[] Warnings { get; set; } = Array.Empty<string>();
    }
}
