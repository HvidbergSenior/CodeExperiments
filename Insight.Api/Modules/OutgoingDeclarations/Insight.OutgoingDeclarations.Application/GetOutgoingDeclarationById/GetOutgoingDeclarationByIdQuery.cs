using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Exceptions;
using Insight.IncomingDeclarations.Integration.GetIncomingDeclarationsByIds;
using Insight.OutgoingDeclarations.Domain;

namespace Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationById
{
    public sealed class GetOutgoingDeclarationByIdQuery : IQuery<GetOutgoingDeclarationByIdResponse>
    {
        public OutgoingDeclarationId OutgoingDeclarationId { get; set; }

        private GetOutgoingDeclarationByIdQuery(OutgoingDeclarationId outgoingDeclarationId)
        {
            OutgoingDeclarationId = outgoingDeclarationId;
        }

        public static GetOutgoingDeclarationByIdQuery Create(OutgoingDeclarationId outgoingDeclarationId
        )
        {
            return new GetOutgoingDeclarationByIdQuery(outgoingDeclarationId);
        }
    }

    internal class GetOutgoingDeclarationByIdQueryHandler : IQueryHandler<
        GetOutgoingDeclarationByIdQuery, GetOutgoingDeclarationByIdResponse>
    {
        private readonly IOutgoingDeclarationRepository outgoingDeclarationRepository;
        private readonly IQueryBus queryBus;


        public GetOutgoingDeclarationByIdQueryHandler(IOutgoingDeclarationRepository outgoingDeclarationRepository, IQueryBus queryBus)
        {
            this.outgoingDeclarationRepository = outgoingDeclarationRepository;
            this.queryBus = queryBus;
        }

        public async Task<GetOutgoingDeclarationByIdResponse> Handle(GetOutgoingDeclarationByIdQuery query, CancellationToken cancellationToken)
        {
            var outgoingDeclaration = await outgoingDeclarationRepository.GetById(query.OutgoingDeclarationId.Value, cancellationToken);

            if (outgoingDeclaration == null)
            {
                throw new NotFoundException("OutgoingDeclaration not found");
            }
            
            var incomingDeclarationIds = outgoingDeclaration.IncomingDeclarationPairings.Select(p => p.IncomingDeclarationId.Value);
            
            var incomingDeclarationsByIdsQuery = GetIncomingDeclarationsByIdsQuery.Create(incomingDeclarationIds);
     
            var getIncomingDeclarationDto = await queryBus.Send<GetIncomingDeclarationsByIdsQuery, GetIncomingDeclarationsDto>(incomingDeclarationsByIdsQuery, cancellationToken);
            
            var mappedDeclarations = MapToOutgoingDeclarationResponse(outgoingDeclaration, getIncomingDeclarationDto);

            return new GetOutgoingDeclarationByIdResponse(mappedDeclarations);
        }

        private static OutgoingDeclarationByIdResponse MapToOutgoingDeclarationResponse(OutgoingDeclaration outgoingDeclaration, GetIncomingDeclarationsDto incomingDeclarationsDto)
        {
            foreach (var incomingDeclarationDto in incomingDeclarationsDto.IncomingDeclarations)
            {
                var batchId = outgoingDeclaration.IncomingDeclarationPairings
                    .Where(d => d.IncomingDeclarationId.Value == incomingDeclarationDto.IncomingDeclarationId)
                    .Select(d => d.BatchId.Value).FirstOrDefault();
              
                    incomingDeclarationDto.BatchId = batchId;
            }
            var outgoingIncomingResponses = new List<GetOutgoingDeclarationIncomingDeclarationResponse>();

            foreach (var incomingDeclaration in incomingDeclarationsDto.IncomingDeclarations)
            {
                outgoingIncomingResponses.Add(new GetOutgoingDeclarationIncomingDeclarationResponse(incomingDeclaration.IncomingDeclarationId, incomingDeclaration.Company, incomingDeclaration.Country, incomingDeclaration.Product, incomingDeclaration.Supplier, incomingDeclaration.RawMaterial, incomingDeclaration.PosNumber, incomingDeclaration.CountryOfOrigin, incomingDeclaration.PlaceOfDispatch, incomingDeclaration.DateOfDispatch, incomingDeclaration.Quantity, incomingDeclaration.GhgEmissionSaving, incomingDeclaration.BatchId));
            }
            
            return new OutgoingDeclarationByIdResponse(
                outgoingDeclaration.Id.ToString(),
                outgoingDeclaration.Country.Value,
                outgoingDeclaration.Product.Value,
                outgoingDeclaration.CustomerDetails.CustomerNumber.Value,
                outgoingDeclaration.CustomerDetails.CustomerName.Value,
                outgoingDeclaration.BfeId.Value,
                outgoingDeclaration.DatePeriod.StartDate,
                outgoingDeclaration.DatePeriod.EndDate,
                outgoingDeclaration.CertificateId.Value,
                outgoingDeclaration.SustainabilityDeclarationNumber.Value,
                outgoingDeclaration.DateOfIssuance.Value,
                outgoingDeclaration.RawMaterialName.Value,
                outgoingDeclaration.RawMaterialCode.Value,
                outgoingDeclaration.ProductionCountry.Value,
                outgoingDeclaration.AdditionalInformation.Value,
                outgoingDeclaration.Mt.Value,
                outgoingDeclaration.Density.Value,
                outgoingDeclaration.Liter.Value,
                outgoingDeclaration.EnergyContent.Value,
                outgoingDeclaration.GreenhouseGasEmission.Value,
                outgoingDeclaration.GreenhouseGasReduction.Value,
                outgoingDeclaration.EmissionSavingControl.Value,
                outgoingDeclaration.EnergyContentControl.Value,
                outgoingIncomingResponses
            );
        }
    }

    internal class GetOutgoingDeclarationByIdQueryAuthorizer : IAuthorizer<GetOutgoingDeclarationByIdQuery>
    {
        private readonly IExecutionContext executionContext;

        public GetOutgoingDeclarationByIdQueryAuthorizer(IExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public async Task<AuthorizationResult> Authorize(GetOutgoingDeclarationByIdQuery query,
            CancellationToken cancellation)
        {
            if (await executionContext.GetAdminPrivileges(cancellation))
            {
                return AuthorizationResult.Succeed();
            }
            return AuthorizationResult.Fail();
        }
    }
}