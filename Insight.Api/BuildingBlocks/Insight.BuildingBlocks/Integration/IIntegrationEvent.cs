using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace Insight.BuildingBlocks.Integration
{
    [SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "<Pending>")]
    public interface IIntegrationEvent : INotification
    { }
}
