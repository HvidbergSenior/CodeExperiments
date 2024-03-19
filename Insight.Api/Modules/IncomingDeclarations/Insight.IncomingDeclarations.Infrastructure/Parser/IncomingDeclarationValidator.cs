using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using FluentValidation;
using Insight.IncomingDeclarations.Domain.Incoming;
using BCECompany = Insight.BusinessCentralEntities.Domain.Companies.Company;

#pragma warning disable CS0183
namespace Insight.IncomingDeclarations.Infrastructure.Parser
{
    public class IncomingDeclarationValidator : AbstractValidator<IncomingDeclaration>
    {
        private readonly IEnumerable<BCECompany> companies;
        
        public IncomingDeclarationValidator(IEnumerable<BCECompany> companies,
            IReadOnlyList<string> productVariants, IReadOnlyList<string> validRawMaterialVariants)
        {
            this.companies = companies;
            
            RuleFor(x => x.Company).NotEmpty().Must(BeCompany);
            RuleFor(x => x.Country).NotEmpty().Must(BeCountry);
            RuleFor(x => x.Product).NotEmpty();
            RuleFor(x => x.DateOfDispatch).NotEmpty().Must(BeValidDateOnlyDateOfDispatch);
            RuleFor(x => x.Supplier).NotEmpty(); // V2: Validate against supplier list
            RuleFor(x => x.CertificationSystem).Must(BeCertificationSystem);
            RuleFor(x => x.SupplierCertificateNumber).NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.CertificationSystem.Value)).Must(BeSupplierCertificateNumber);
            RuleFor(x => x.PosNumber).NotEmpty();
            RuleFor(x => x.DateOfIssuance).NotEmpty().Must(BeValidDateOnlyDateOfIssuance).Must(
                (incomingDeclaration, dateOfIssuance) =>
                    dateOfIssuance.Value.CompareTo(incomingDeclaration.DateOfDispatch.Value) < 0);
            RuleFor(x => x.PlaceOfDispatch).NotEmpty(); // V2: Validate against "place" list
            RuleFor(x => x.ProductionCountry)
                .NotEmpty(); // V2: Validate against longform country name. See BeCountry() and CountryOfOrigin
            RuleFor(x => x.DateOfInstallation).Must(NotBeEmptyDateOfInstallation);
            RuleFor(x => x.TypeOfProduct).NotEmpty();
            RuleFor(x => x.RawMaterial).NotEmpty();
            // TODO: JDO: Move validation to private method
            RuleFor(x => x.RawMaterial).Must(rm => validRawMaterialVariants.Any(validRm => String.Equals(validRm, rm.Value, StringComparison.OrdinalIgnoreCase)))
                .WithMessage(id => $"The value '{id.RawMaterial.Value}' for RawMaterial is not in the predefined list af values.");
            RuleFor(x => x.Product).Must(product => productVariants.Any(validProduct => String.Equals(validProduct, product.Value, StringComparison.OrdinalIgnoreCase)))
                .WithMessage(id => $"The value '{id.Product.Value}' for Product is not in the predefined list af values.");
            RuleFor(x => x.AdditionalInformation);
            RuleFor(x => x.CountryOfOrigin)
                .NotEmpty(); // V2: Validate against longform country name. See BeCountry() and ProductionCountry
            RuleFor(x => x.Quantity).NotEmpty().Must(quantity => decimal.IsPositive(quantity.Value));
            RuleFor(x => x.UnitOfMeasurement).IsInEnum();
            RuleFor(x => x.Quantity.Value).GreaterThan(0);
            RuleFor(x => x.GhgEmissionSaving.Value).InclusiveBetween(0.01m, 1);            
        }
        private bool NotBeEmptyDateOfInstallation(DateOfInstallation dateOfInstallation)
        {
            return !string.IsNullOrEmpty(dateOfInstallation.Value);
        }
        private bool BeCompany(Company company)
        {
            return companies.Any(bceCompany =>
                bceCompany.CompanyName.Value.Equals(company.Value, StringComparison.OrdinalIgnoreCase));
        }
        private bool BeValidDateOnlyDateOfDispatch(DateOfDispatch dateOfDispatch)
        {
            var isValid = dateOfDispatch.Value is DateOnly;
            return isValid;
        }
        
        private bool BeValidDateOnlyDateOfIssuance(DateOfIssuance dateOfIssuance)
        {
            var isValid = dateOfIssuance.Value is DateOnly;
            return isValid;
        }       

        private bool BeCountry(Country country)
        {
            RegionInfo regionInfo;
            try
            {
                regionInfo = new RegionInfo(country.Value);
            }
            catch
            {
                return false;
            }

            return country.Value.Equals(regionInfo.TwoLetterISORegionName, StringComparison.Ordinal);
        }

        private List<string>
            certificationSystems =
                new()
                {
                   "#", "ISCC-EU", "ISCC EU", "NABISY", "REDII", "RED-II", "RED II", "REDIII", "RED-III", "REDIII"
                };

        private bool BeCertificationSystem(CertificationSystem certificationSystem)
        {
            return string.IsNullOrEmpty(certificationSystem.Value) ||
                   certificationSystems.Contains(certificationSystem.Value.ToUpperInvariant());
        }

        private List<string>
            supplierCertificateNumberPrefixes =
                new()
                {
                   "#", "EU-ISCC-CERT-", "ISCC-",
                }; // (https://www.iscc-system.org/certification/certificate-database/all-certificates/ version 39)

        private bool BeSupplierCertificateNumber(SupplierCertificateNumber supplierCertificateNumber)
        {
            foreach (var supplierCertificateNumberPrefix in supplierCertificateNumberPrefixes)
            {
                if (supplierCertificateNumber.Value.StartsWith(supplierCertificateNumberPrefix,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

#pragma warning restore CS0183