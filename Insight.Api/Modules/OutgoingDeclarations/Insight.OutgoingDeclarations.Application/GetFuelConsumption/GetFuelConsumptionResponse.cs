using System.ComponentModel.DataAnnotations;
using Insight.OutgoingDeclarations.Application.Helpers;
using Insight.OutgoingDeclarations.Domain;

namespace Insight.OutgoingDeclarations.Application.GetFuelConsumption
{
    public sealed class GetFuelConsumptionResponse
    {
        [Required]
        public required ConsumptionPerProduct ConsumptionPerProduct { get; set; }
        [Required]
        public required ConsumptionDevelopment ConsumptionDevelopment { get; set; }
        [Required]
        public required ConsumptionStats ConsumptionStats { get; set; }
    }
    
  
}