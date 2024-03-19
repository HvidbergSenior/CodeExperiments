using System.ComponentModel.DataAnnotations;

namespace Insight.IncomingDeclarations.Application.CancelIncomingDeclarationsByUploadId
{
    public class CancelIncomingDeclarationsByUploadIdRequest
    {
        [Required(ErrorMessage = "Incoming Declaration Upload Id is required")]
        public required Guid IncomingDeclarationUploadId { get; set; }
    }
}
