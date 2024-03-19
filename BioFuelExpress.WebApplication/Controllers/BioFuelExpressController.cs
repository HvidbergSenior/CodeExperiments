using BioFuelExpress.Application.CreateSomething;
using BioFuelExpress.Application.GetSomething;
using BioFuelExpress.BuildingBlocks.Application.Commands;
using BioFuelExpress.BuildingBlocks.Application.Queries;
using BioFuelExpress.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BioFuelExpress.WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BioFuelExpressController : ControllerBase
    {
        private readonly IQueryBus _queryBus;
        private readonly ICommandBus _commandBus;
        private readonly ILogger<BioFuelExpressController> _logger;

        public BioFuelExpressController(ILogger<BioFuelExpressController> logger, IQueryBus queryBus, ICommandBus commandBus)
        {
            _logger = logger;
            _queryBus = queryBus;
            _commandBus = commandBus;
        }

        [HttpGet]
        [Route("/something")]
        [ProducesResponseType(typeof(GetSomethingResponse), 200)]
        public async Task<ActionResult> GetSomething(Guid guid)
        {
            var query = GetSomethingQuery.Create(SomethingId.Create(Guid.NewGuid()));
            var response = await _queryBus.Send<GetSomethingQuery, GetSomethingResponse>(query);
            return Ok(response);
        }

        [HttpPost]
        [Route("/something")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> UpdateBio(UpdateBioFuelExpressRequest request)
        {

            var command = CreateSomethingCommand.Create(Guid.NewGuid(), request.Name);
            await _commandBus.Send(command, HttpContext.RequestAborted);

            return Ok(command);
            // return CreatedAtRoute(routeValues, id);
        }
    }
}