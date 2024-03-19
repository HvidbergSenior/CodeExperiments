using System.ComponentModel.DataAnnotations;

namespace Insight.IncomingDeclarations.Application.UploadIncomingDeclaration
{
    public class IncomingDeclarationUploadIdResponse
    {
        [Required]
        public Guid Id { get; private set; }
        
        public IncomingDeclarationUploadIdResponse(Guid id)
        {
            Id = id;
        }
    }
}
