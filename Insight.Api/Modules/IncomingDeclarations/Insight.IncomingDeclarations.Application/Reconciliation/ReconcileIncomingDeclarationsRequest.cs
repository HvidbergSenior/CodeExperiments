using System.ComponentModel.DataAnnotations;

namespace Insight.IncomingDeclarations.Application.Reconciliation
{
    public class ReconcileIncomingDeclarationsRequest
    {
        [Required(ErrorMessage = "Incoming Declaration Ids are required")]
        public required Guid[] IncomingDeclarationIds { get; set; }
    }
}
