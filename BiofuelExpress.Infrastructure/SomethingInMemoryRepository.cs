using BioFuelExpress.BuildingBlocks.Fakes;
using BioFuelExpress.Domain;

namespace BioFuelExpress.Infrastructure
{
    public class SomethingInMemoryRepository : InMemoryRepository<Something>, ISomethingRepository
    {
    }
}
