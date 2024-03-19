using System.ComponentModel.DataAnnotations;

namespace Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationById
{
    public sealed class GetOutgoingDeclarationByIdResponse
    {
        public OutgoingDeclarationByIdResponse OutgoingDeclarationByIdResponse { get; set; }

        public GetOutgoingDeclarationByIdResponse(OutgoingDeclarationByIdResponse outgoingDeclarationByIdResponse)
        {
            OutgoingDeclarationByIdResponse = outgoingDeclarationByIdResponse;
        }
    }

    public sealed class OutgoingDeclarationByIdResponse
    {
        [Required] public string OutgoingDeclarationId { get; private set; }
        [Required] public string Country { get; private set; }
        [Required] public string Product { get; private set; }
        [Required] public string CustomerNumber { get; private set; }
        [Required] public string CustomerName { get; private set; }
        public string BfeId { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public string CertificateId { get; private set; }
        public string SustainabilityDeclarationNumber { get; private set; }
        public DateOnly DateOfIssuance { get; private set; }
        public string RawMaterialName { get; private set; }
        public string RawMaterialCode { get; private set; }
        public string ProductionCountry { get; private set; }
        public string AdditionalInformation { get; private set; }
        public decimal Mt { get; private set; }
        public decimal Density { get; private set; }
        public decimal Liter { get; private set; }
        public decimal EnergyContent { get; private set; }
        public decimal GreenhouseGasEmission { get; private set; }
        public decimal GreenhouseGasReduction { get; private set; }
        public decimal EmissionSavingControl { get; private set; }
        public decimal EnergyContentControl { get; private set; }
        public IReadOnlyList<GetOutgoingDeclarationIncomingDeclarationResponse> GetOutgoingDeclarationIncomingDeclarationResponse { get; private set; }

        public OutgoingDeclarationByIdResponse(
            string outgoingDeclarationId,
            string country,
            string product,
            string customerNumber,
            string customerName,
            string bfeId,
            DateOnly startDate,
            DateOnly endDate,
            string certificateId,
            string sustainabilityDeclarationNumber,
            DateOnly dateOfIssuance,
            string rawMaterialName,
            string rawMaterialCode,
            string productionCountry,
            string additionalInformation,
            decimal mt,
            decimal density,
            decimal liter,
            decimal energyContent,
            decimal greenhouseGasEmission,
            decimal greenhouseGasReduction,
            decimal emissionSavingControl,
            decimal energyContentControl,
            IReadOnlyList<GetOutgoingDeclarationIncomingDeclarationResponse> getOutgoingDeclarationIncomingDeclarationResponse)
        {
            OutgoingDeclarationId = outgoingDeclarationId;
            Country = country;
            Product = product;
            CustomerNumber = customerNumber;
            CustomerName = customerName;
            BfeId = bfeId;
            StartDate = startDate;
            EndDate = endDate;
            CertificateId = certificateId;
            SustainabilityDeclarationNumber = sustainabilityDeclarationNumber;
            DateOfIssuance = dateOfIssuance;
            RawMaterialName = rawMaterialName;
            RawMaterialCode = rawMaterialCode;
            ProductionCountry = productionCountry;
            AdditionalInformation = additionalInformation;
            Mt = mt;
            Density = density;
            Liter = liter;
            EnergyContent = energyContent;
            GreenhouseGasEmission = greenhouseGasEmission;
            GreenhouseGasReduction = greenhouseGasReduction;
            EmissionSavingControl = emissionSavingControl;
            EnergyContentControl = energyContentControl;
            GetOutgoingDeclarationIncomingDeclarationResponse = getOutgoingDeclarationIncomingDeclarationResponse;
        }
    }

    public sealed class GetOutgoingDeclarationIncomingDeclarationResponse
    {
        public Guid IncomingDeclarationId { get; set; }
        public string Company { get; private set; }
        public string Country { get; private set; }
        public string Product { get; private set; }
        public string Supplier { get; private set; }
        public string RawMaterial { get; private set; }
        public string PosNumber { get; private set; }
        public string CountryOfOrigin { get; private set; }
        public string PlaceOfDispatch { get; private set; }
        public DateOnly DateOfDispatch { get; private set; }
        public decimal Quantity { get; private set; }

        public decimal GhgEmissionSaving { get; private set; }

        //BatchId is only used when creating an Outgoing Declaration
        public long BatchId { get; set; }

        public GetOutgoingDeclarationIncomingDeclarationResponse(
            Guid incomingDeclarationId,
            string company,
            string country,
            string product,
            string supplier,
            string rawMaterial,
            string posNumber,
            string countryOfOrigin,
            string placeOfDispatch,
            DateOnly dateOfDispatch,
            decimal quantity,
            decimal ghgEmissionSaving,
            long batchId)
        {
            IncomingDeclarationId = incomingDeclarationId;
            Company = company;
            Country = country;
            Product = product;
            Supplier = supplier;
            RawMaterial = rawMaterial;
            PosNumber = posNumber;
            CountryOfOrigin = countryOfOrigin;
            PlaceOfDispatch = placeOfDispatch;
            DateOfDispatch = dateOfDispatch;
            Quantity = quantity;
            GhgEmissionSaving = ghgEmissionSaving;
            BatchId = batchId;
        }
    }
}