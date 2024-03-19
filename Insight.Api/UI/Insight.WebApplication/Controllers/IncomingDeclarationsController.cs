using Insight.BuildingBlocks.Application.Commands;
using Insight.IncomingDeclarations.Application.UploadIncomingDeclaration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insight.WebApplication.Controllers
{
    // TODO: Multipart form data uploads are not supported in .NET 7 with minimal APIs, update when .NET 8 is released
    // More info: https://andrewlock.net/exploring-the-dotnet-8-preview-form-binding-in-minimal-apis/
    public class IncomingDeclarationsController : ApiControllerBase
    {
        private readonly ICommandBus commandBus;

        public IncomingDeclarationsController(ICommandBus commandBus)
        {
            this.commandBus = commandBus;
        }

        [HttpPost]
        [Route("/api/incomingdeclarations/upload")]
        [Consumes("multipart/form-data")]
        [Tags("IncomingDeclarations")]
        [ProducesResponseType(typeof(UploadIncomingDeclarationCommandResponse), StatusCodes.Status200OK)]

        public async Task<UploadIncomingDeclarationCommandResponse> UploadIncomingDeclaration([FromForm] UploadIncomingDeclarationRequest request)
        {
            var command = UploadIncomingDeclarationCommand.Create(request.ExcelFile, request.IncomingDeclarationSupplier);
            var result = await commandBus.Send(command, HttpContext.RequestAborted);
            return result;
        }
    }
}
