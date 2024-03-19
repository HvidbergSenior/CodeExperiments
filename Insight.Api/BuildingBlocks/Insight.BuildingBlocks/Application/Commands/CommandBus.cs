using MediatR;
namespace Insight.BuildingBlocks.Application.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly IMediator _mediator;

        public CommandBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Send<TCommand>(ICommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
        }

        public async Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
    }
}
