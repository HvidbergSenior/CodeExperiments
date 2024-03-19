using System.ComponentModel.DataAnnotations;

namespace Insight.IncomingDeclarations.Application.UploadIncomingDeclaration
{
    public class IncomingDeclarationParseResponse
    {
        public IncomingDeclarationParseResponse(int rowNumber, string posNumber, string errorMessage, bool success)
        {
            RowNumber = rowNumber;
            PosNumber = posNumber;
            ErrorMessage = errorMessage;
            Success = success;
        }
        [Required]
        public int RowNumber { get; private set; }
        [Required]
        public string PosNumber { get; private set; }
        [Required]
        public string ErrorMessage { get; private set; }
        [Required]
        public bool Success { get; private set; }
    }
}
