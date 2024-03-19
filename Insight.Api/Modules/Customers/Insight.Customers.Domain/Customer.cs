using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class Customer : Entity
{
    public CompanyId CompanyId { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public CustomerDetails CustomerDetails { get; private set; }
    public SalesPerson SalesPerson { get; private set; }
    public SourceETag SourceETag { get; private set; }
    public CreditLimit CreditLimit { get; private set; }
    public Blocked Blocked { get; private set; }
    public BalanceLcy BalanceLcy { get; private set; }
    public BalanceDueLcy BalanceDueLcy { get; private set; }
    public OutstandingOrdersLcy OutstandingOrdersLcy { get; private set; }
    public NumberNumber NumberNumber { get; private set; }
    public OrganisationNumber OrganisationNumber { get; private set; }
    public PdiAndLdPointNumber PdiAndLdPointNumber { get; private set; }
    public VatRegNumber VatRegNumber { get; private set; }
    public PaymentTermsCode PaymentTermsCode { get; private set; }
    public ShipmentMethodCode ShipmentMethodCode { get; private set; }
    public ShippingAgentCode ShippingAgentCode { get; private set; }
    public CardCustomer CardCustomer { get; private set; }
    public AllowedRawMaterials AllowedRawMaterials { get; private set; } = AllowedRawMaterials.Empty();
    public CO2Target CO2Target { get; private set; } = CO2Target.Empty();

    private Customer()
    {
        CompanyId = CompanyId.Empty();
        CustomerId = CustomerId.Empty();
        Id = CustomerId.Value;
        CustomerDetails = CustomerDetails.Empty();
        SalesPerson = SalesPerson.Empty();
        SourceETag = SourceETag.Empty();
        CreditLimit = CreditLimit.Empty();
        Blocked = Blocked.Empty();
        OrganisationNumber = OrganisationNumber.Empty();
        BalanceLcy = BalanceLcy.Empty();
        BalanceDueLcy = BalanceDueLcy.Empty();
        OutstandingOrdersLcy = OutstandingOrdersLcy.Empty();
        NumberNumber = NumberNumber.Empty();
        PdiAndLdPointNumber = PdiAndLdPointNumber.Empty();
        VatRegNumber = VatRegNumber.Empty();
        PaymentTermsCode = PaymentTermsCode.Empty();
        ShipmentMethodCode = ShipmentMethodCode.Empty();
        ShippingAgentCode = ShippingAgentCode.Empty();
        CardCustomer = CardCustomer.Empty();
    }

    private Customer(
        CompanyId companyId,
        CustomerId customerId,
        CustomerDetails customerDetails,
        BalanceLcy balanceLcy,
        BalanceDueLcy balanceDueLcy,
        OutstandingOrdersLcy outstandingOrdersLcy,
        NumberNumber numberNumber,
        PdiAndLdPointNumber pdiAndLdPointNumber,
        VatRegNumber vatRegNumber,
        OrganisationNumber organisationNumber,
        PaymentTermsCode paymentTermsCode,
        ShipmentMethodCode shipmentMethodCode,
        ShippingAgentCode shippingAgentCode,
        SalesPerson salesPerson,
        SourceETag sourceEtag,
        CreditLimit creditLimit,
        Blocked blocked,
        CardCustomer cardCustomer)
    {
        CompanyId = companyId;
        CustomerId = customerId;
        Id = CustomerId.Value;
        CustomerDetails = customerDetails;
        BalanceLcy = balanceLcy;
        BalanceDueLcy = balanceDueLcy;
        OutstandingOrdersLcy = outstandingOrdersLcy;
        NumberNumber = numberNumber;
        PdiAndLdPointNumber = pdiAndLdPointNumber;
        VatRegNumber = vatRegNumber;
        OrganisationNumber = organisationNumber;
        PaymentTermsCode = paymentTermsCode;
        ShipmentMethodCode = shipmentMethodCode;
        ShippingAgentCode = shippingAgentCode;
        SalesPerson = salesPerson;
        SourceETag = sourceEtag;
        CreditLimit = creditLimit;
        Blocked = blocked;
        CardCustomer = cardCustomer;
    }

    public static Customer Create(
        CompanyId companyId,
        CustomerId customerId,
        CustomerDetails customerDetails,
        BalanceLcy balanceLcy,
        BalanceDueLcy balanceDueLcy,
        OutstandingOrdersLcy outstandingOrdersLcy,
        NumberNumber numberNumber,
        PdiAndLdPointNumber pdiAndLdPointNumber,
        VatRegNumber vatRegNumber,
        OrganisationNumber organisationNumber,
        PaymentTermsCode paymentTermsCode,
        ShipmentMethodCode shipmentMethodCode,
        ShippingAgentCode shippingAgentCode,
        SalesPerson salesPerson,
        SourceETag sourceEtag,
        CreditLimit creditLimit,
        Blocked blocked,
        CardCustomer cardCustomer
    )
    {
        return new Customer(
            companyId,
            customerId,
            customerDetails,
            balanceLcy,
            balanceDueLcy,
            outstandingOrdersLcy,
            numberNumber,
            pdiAndLdPointNumber,
            vatRegNumber,
            organisationNumber,
            paymentTermsCode,
            shipmentMethodCode,
            shippingAgentCode,
            salesPerson,
            sourceEtag,
            creditLimit,
            blocked,
            cardCustomer);
    }

    public static Customer Empty()
    {
        return new Customer();
    }

    public void UpdateCustomer(
        CompanyId companyId,
        CustomerId customerId,
        CustomerDetails customerDetails,
        BalanceLcy balanceLcy,
        BalanceDueLcy balanceDueLcy,
        OutstandingOrdersLcy outstandingOrdersLcy,
        NumberNumber numberNumber,
        PdiAndLdPointNumber pdiAndLdPointNumber,
        VatRegNumber vatRegNumber,
        OrganisationNumber organisationNumber,
        PaymentTermsCode paymentTermsCode,
        ShipmentMethodCode shipmentMethodCode,
        ShippingAgentCode shippingAgentCode,
        SalesPerson salesPerson,
        SourceETag sourceEtag,
        CreditLimit creditLimit,
        Blocked blocked,
        CardCustomer cardCustomer)
    {
        CompanyId = companyId;
        CustomerId = customerId;
        CustomerDetails = customerDetails;
        BalanceLcy = balanceLcy;
        BalanceDueLcy = balanceDueLcy;
        OutstandingOrdersLcy = outstandingOrdersLcy;
        NumberNumber = numberNumber;
        PdiAndLdPointNumber = pdiAndLdPointNumber;
        VatRegNumber = vatRegNumber;
        OrganisationNumber = organisationNumber;
        PaymentTermsCode = paymentTermsCode;
        ShipmentMethodCode = shipmentMethodCode;
        ShippingAgentCode = shippingAgentCode;
        SalesPerson = salesPerson;
        SourceETag = sourceEtag;
        CreditLimit = creditLimit;
        Blocked = blocked;
        CardCustomer = cardCustomer;
    }

    public void SetCO2Target(CO2Target co2Target)
    {
        CO2Target = co2Target;
    }

    public void SetAllowedRawMaterials(AllowedRawMaterials allowedRawMaterials)
    {
        AllowedRawMaterials = allowedRawMaterials;
    }
}