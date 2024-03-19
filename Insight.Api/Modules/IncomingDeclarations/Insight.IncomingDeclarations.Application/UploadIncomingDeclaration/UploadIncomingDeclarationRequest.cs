using System.ComponentModel.DataAnnotations;
using Insight.IncomingDeclarations.Domain.Parsing;
using Microsoft.AspNetCore.Http;

namespace Insight.IncomingDeclarations.Application.UploadIncomingDeclaration
{
    public class UploadIncomingDeclarationRequest// : IEndpointParameterMetadataProvider
    {
        [Required(ErrorMessage = "Excel file is required")]
        public required IFormFile ExcelFile { get; set; }

        [Required(ErrorMessage = "Incoming declaration supplier is required")]
        public required IncomingDeclarationSupplier IncomingDeclarationSupplier { get; set; }
    }
}
