using Insight.BuildingBlocks.Integration;
using Insight.BuildingBlocks.Integration.Inbox;
using Insight.Services.BusinessCentralConnector.Integration;
using Insight.Stations.Domain;

namespace Insight.Stations.Application.CreateOrUpdateStation
{
    public class StationCreatedOrUpdated : IIntegrationEventListener<StationCreatedOrUpdatedIntegrationEvent>
    {
        private readonly IInbox inbox;

        public StationCreatedOrUpdated(IInbox inbox)
        {
            this.inbox = inbox;
        }

        public Task Handle(StationCreatedOrUpdatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            var cmd = CreateCommand(notification);
            inbox.Add(InboxMessage.From(cmd, notification.Id));
            return Task.CompletedTask;
        }

        private static CreateOrUpdateStationCommand CreateCommand(StationCreatedOrUpdatedIntegrationEvent notification)
        {   
            if(notification.StationSystem == "Tokheim DiaLOG")
            {
                // Todo: Handle this in a better way
                notification.StationSystem = StationSystem.Tokheim.ToString();
            }
            var success = Enum.TryParse<StationSystem>(notification.StationSystem, true, out var stationSystem);
            if(!success)
            {
                throw new NotSupportedException($"Station System {notification.StationSystem} not supported");
            }
            
            return CreateOrUpdateStationCommand.Create(StationId.Create(notification.Id),
                SourceETag.Create(notification.SourceETag),
                StationNumber.Create(notification.StationNumber),
                StationName.Create(notification.StationName),
                StationAddress.Create(notification.StationAddress),
                StationAddress.Create(notification.Address2),
                StationPostCode.Create(notification.StationPostalCode),
                StationCity.Create(notification.StationCity),
                StationCountryRegionCode.Create(notification.StationCountry),
                StationLatitude.Create(notification.Latitude),
                StationLongitude.Create(notification.Longitude),
                stationSystem);
        }
    }
}
