using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Domain;

namespace Insight.IncomingDeclarations.Integration.GetIncomingDeclarationsByIds
{
    public sealed class GetIncomingDeclarationsByIdsQuery : IQuery<GetIncomingDeclarationsDto>
    {
        public IEnumerable<Guid> IncomingDeclarationIds { get; private set; }

        private GetIncomingDeclarationsByIdsQuery(IEnumerable<Guid> incomingDeclarationIds)
        {
            IncomingDeclarationIds = incomingDeclarationIds;
        }

        public static GetIncomingDeclarationsByIdsQuery Create(IEnumerable<Guid> incomingDeclarationIds)
        {
            return new GetIncomingDeclarationsByIdsQuery(incomingDeclarationIds);
        }
    }
}
