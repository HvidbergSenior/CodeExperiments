using BioFuelExpress.BuildingBlocks.Application.Queries;
using BioFuelExpress.Domain;

namespace BioFuelExpress.Application.GetSomething
{
    public sealed class GetSomethingQuery : IQuery<GetSomethingResponse>
    {
        public SomethingId SomethingId { get; set; }

        private GetSomethingQuery(SomethingId somethingId)
        {
            SomethingId = somethingId;
        }

        public static GetSomethingQuery Create(SomethingId somethingId)
        {
            return new GetSomethingQuery(somethingId);
        }
    }

    internal class GetSomethingQueryHandler : IQueryHandler<GetSomethingQuery, GetSomethingResponse>
    {
        private readonly ISomethingRepository _somethingRepository;

        public GetSomethingQueryHandler(ISomethingRepository somethingRepository)
        {
            _somethingRepository = somethingRepository;
        }

        public async Task<GetSomethingResponse> Handle(GetSomethingQuery query, CancellationToken cancellationToken)
        {
            var byId = await _somethingRepository.GetById(Guid.Parse("7f93581c-b852-428a-b8ec-188f0623a9d8"), cancellationToken);
            var response = new GetSomethingResponse(Guid.NewGuid(), "New Title");
            return response;
        }
    }
}


