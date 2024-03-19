using BioFuelExpress.BuildingBlocks.Infrastructure;

namespace BioFuelExpress.Domain
{
    public interface ISomethingRepository : IRepository<Something>, IReadonlyRepository<Something>
    {

    }
}
