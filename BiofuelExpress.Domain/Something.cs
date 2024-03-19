using BioFuelExpress.BuildingBlocks.Domain;

namespace BioFuelExpress.Domain;

public sealed class Something : Entity
{
    public SomethingId SomethingId { get; private set; }
    private Something()
    {
        Id = Guid.NewGuid();
        SomethingId = SomethingId.Create(Id);
    }

    private Something(SomethingId somethingId)
    {
        Id = somethingId.Value;
        SomethingId = somethingId;
    }

    public static Something Create(SomethingId somethingId)
    {
        var something = new Something(somethingId);
        //project.AddDomainEvent(ProjectCreatedDomainEvent.Create(projectId, projectName, projectDescription, projectOwner, projectPieceworkType));
        return something;
    }

}
