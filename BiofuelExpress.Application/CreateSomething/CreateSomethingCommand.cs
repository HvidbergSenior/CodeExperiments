using BioFuelExpress.BuildingBlocks.Application.Commands;
using BioFuelExpress.BuildingBlocks.Infrastructure;
using BioFuelExpress.Domain;
using Microsoft.Extensions.Configuration;

namespace BioFuelExpress.Application.CreateSomething
{
    public sealed class CreateSomethingCommand : ICommand<ICommandResponse>
    {
        internal SomethingId SomethingId { get; }
        internal string Name { get; }

        private CreateSomethingCommand(SomethingId somethingId, string name)
        {
            SomethingId = somethingId;
            Name = name;
        }

        public static CreateSomethingCommand Create(Guid somethingId, string name)
        {
            return new CreateSomethingCommand(SomethingId.Create(somethingId), name);
        }
    }

    internal class CreateSomethingCommandHandler : ICommandHandler<CreateSomethingCommand, ICommandResponse>
    {
        private readonly ISomethingRepository _somethingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSomethingCommandHandler(IUnitOfWork unitOfWork, ISomethingRepository somethingRepository)
        {
            _unitOfWork = unitOfWork;
            _somethingRepository = somethingRepository;
        }

        public async Task<ICommandResponse> Handle(CreateSomethingCommand request, CancellationToken cancellationToken)
        {
            await _somethingRepository.Add(Something.Create(SomethingId.Create(Guid.NewGuid())));
            await _somethingRepository.SaveChanges();
            await _unitOfWork.Commit(cancellationToken);

            var name = request.Name;
            name = "Bent";
            return EmptyCommandResponse.Default;
        }
    }
}
