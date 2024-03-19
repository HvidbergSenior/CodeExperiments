using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Commands;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Infrastructure;
using Insight.Stations.Domain;
using Microsoft.Extensions.Logging;

namespace Insight.Stations.Application.CreateOrUpdateStation
{
    public sealed class CreateOrUpdateStationCommand : IInternalCommand, ICommand<ICommandResponse>
    {
        private CreateOrUpdateStationCommand(StationId stationId, SourceETag sourceETag, StationNumber stationNumber, StationName stationName, StationAddress stationAddress, StationAddress stationAddress2, StationPostCode stationPostCode, StationCity stationCity, StationCountryRegionCode stationCountryRegionCode, StationLatitude stationLatitude, StationLongitude stationLongitude, StationSystem stationSystem)
        {
            StationId = stationId;
            SourceETag = sourceETag;
            StationNumber = stationNumber;
            StationName = stationName;
            StationAddress = stationAddress;
            StationAddress2 = stationAddress2;
            StationPostCode = stationPostCode;
            StationCity = stationCity;
            StationCountryRegionCode = stationCountryRegionCode;
            StationLatitude = stationLatitude;
            StationLongitude = stationLongitude;
            StationSystem = stationSystem;
        }       

        internal StationId StationId { get; set; }
        internal SourceETag SourceETag { get; set; }
        internal StationNumber StationNumber { get; set; }
        internal StationName StationName { get; set; }
        internal StationAddress StationAddress { get; set; }
        internal StationAddress StationAddress2 { get; set; }
        internal StationPostCode StationPostCode { get; set; }
        internal StationCity StationCity { get; set; }
        internal StationCountryRegionCode StationCountryRegionCode { get; set; }
        internal StationLatitude StationLatitude { get; set; }
        internal StationLongitude StationLongitude { get; set; }
        internal StationSystem StationSystem { get; set; }

#pragma warning disable CS8618
        private CreateOrUpdateStationCommand()
#pragma warning restore CS8618
        {
            StationId = StationId.Empty();
            SourceETag = SourceETag.Empty();
            StationNumber = StationNumber.Empty();
            StationName = StationName.Empty();
            StationAddress = StationAddress.Empty();
            StationAddress2 = StationAddress.Empty();
            StationPostCode = StationPostCode.Empty();
            StationCity = StationCity.Empty();
            StationCountryRegionCode = StationCountryRegionCode.Empty();
            StationLatitude = StationLatitude.Empty();
            StationLongitude = StationLongitude.Empty();
        }

        public static CreateOrUpdateStationCommand Create(StationId stationId,
                                                          SourceETag sourceETag,
                                                          StationNumber stationNumber,
                                                          StationName stationName,
                                                          StationAddress stationAddress,
                                                          StationAddress stationAddress2,
                                                          StationPostCode stationPostCode,
                                                          StationCity stationCity,
                                                          StationCountryRegionCode stationCountryRegionCode,
                                                          StationLatitude stationLatitude,
                                                          StationLongitude stationLongitude,
                                                          StationSystem stationSystem)
        {
            return new CreateOrUpdateStationCommand(stationId,
                                                    sourceETag,
                                                    stationNumber,
                                                    stationName,
                                                    stationAddress,
                                                    stationAddress2,
                                                    stationPostCode,
                                                    stationCity,
                                                    stationCountryRegionCode,
                                                    stationLatitude,
                                                    stationLongitude,
                                                    stationSystem);
        }
    }

    internal class CreateOrUpdateStationCommandHandler : ICommandHandler<CreateOrUpdateStationCommand, ICommandResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IStationRepository stationRepository;
        private readonly ILogger<CreateOrUpdateStationCommandHandler> logger;

        public CreateOrUpdateStationCommandHandler(IUnitOfWork unitOfWork, IStationRepository stationRepository, ILogger<CreateOrUpdateStationCommandHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.stationRepository = stationRepository;
            this.logger = logger;
        }

        public async Task<ICommandResponse> Handle(CreateOrUpdateStationCommand request, CancellationToken cancellationToken)
        {
            var station = CreateStation(request);
            var existingStation = await stationRepository.GetByStationNumberAsync(station.StationNumber, cancellationToken);
            if (existingStation != null)
            {
                existingStation.Update(
                                       station.StationName,
                                       station.StationAddress,
                                       station.StationAddress2,
                                       station.StationPostCode,
                                       station.StationCity,
                                       station.StationCountryRegionCode,
                                       station.StationLatitude,
                                       station.StationLongitude,
                                       station.StationSystem);

                await stationRepository.Update(existingStation, cancellationToken);
                logger.LogInformation("Station {StationName} ({StationNumber}) updated",
                                       existingStation.StationName.Value, existingStation.StationNumber.Value);
            }
            else
            {
                await stationRepository.Add(station, cancellationToken);
                logger.LogInformation("Station {StationNumber} created", station.StationNumber.Value);
            }

            await stationRepository.SaveChanges(cancellationToken);
            await unitOfWork.Commit(cancellationToken);
            return EmptyCommandResponse.Default;
        }

        private static Station CreateStation(CreateOrUpdateStationCommand request)
        {
            return Station.Create(request.StationId,
                                  request.SourceETag,
                                  request.StationNumber,
                                  request.StationName,
                                  request.StationAddress,
                                  request.StationAddress2,
                                  request.StationPostCode,
                                  request.StationCity,
                                  request.StationCountryRegionCode,
                                  request.StationLatitude,
                                  request.StationLongitude,
                                  request.StationSystem);
        }
    }

    internal class CreateOrUpdateStationCommandAuthorizer : IAuthorizer<CreateOrUpdateStationCommand>
    {
        private readonly IExecutionContext executionContext;

        public CreateOrUpdateStationCommandAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public Task<AuthorizationResult> Authorize(CreateOrUpdateStationCommand query,
            CancellationToken cancellation)
        {
            return Task.FromResult(AuthorizationResult.Succeed());
        }
    }
}
