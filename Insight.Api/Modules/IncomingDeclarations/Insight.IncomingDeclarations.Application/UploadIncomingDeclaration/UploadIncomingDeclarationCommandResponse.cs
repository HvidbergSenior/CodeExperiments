using System.ComponentModel.DataAnnotations;

namespace Insight.IncomingDeclarations.Application.UploadIncomingDeclaration
{
    public sealed class UploadIncomingDeclarationCommandResponse
    {
        [Required] public IncomingDeclarationUploadIdResponse IncomingDeclarationUploadId { get; set; }
        [Required] public IEnumerable<IncomingDeclarationParseResponse> IncomingDeclarationParseResponses { get; set; }

        [Required] public DateOnly OldestEntry { get; set; }
        [Required] public DateOnly NewestEntry { get; set; }

        public UploadIncomingDeclarationCommandResponse(IncomingDeclarationUploadIdResponse incomingDeclarationUploadId,
            IEnumerable<IncomingDeclarationParseResponse> incomingDeclarationParseResponses, DateOnly oldestEntry, DateOnly newestEntry)
        {
            IncomingDeclarationUploadId = incomingDeclarationUploadId;
            IncomingDeclarationParseResponses = incomingDeclarationParseResponses;
            OldestEntry = oldestEntry;
            NewestEntry = newestEntry;
        }
    }
}
