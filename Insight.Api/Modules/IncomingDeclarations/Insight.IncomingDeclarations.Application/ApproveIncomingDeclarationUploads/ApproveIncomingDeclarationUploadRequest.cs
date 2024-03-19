using System.ComponentModel.DataAnnotations;

namespace Insight.IncomingDeclarations.Application.ApproveIncomingDeclarationUploads
{
    public class ApproveIncomingDeclarationUploadRequest
    {
        [Required(ErrorMessage = "Incoming Declaration Upload Id is required")]
        public required Guid IncomingDeclarationUploadId { get; set; }
    }
}
