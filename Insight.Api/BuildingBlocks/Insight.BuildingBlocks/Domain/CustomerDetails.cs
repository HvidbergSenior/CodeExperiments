namespace Insight.BuildingBlocks.Domain;

public sealed class CustomerDetails : ValueObject
{
    public CustomerNumber CustomerNumber { get; private set; }
    public CustomerAddress CustomerAddress { get; private set; }
    public CustomerBillToName CustomerBillToName { get; private set; }
    public CustomerBillToNumber CustomerBillToNumber { get; private set; }
    public CustomerCity CustomerCity { get; private set; }
    public CustomerDeliveryType CustomerDeliveryType { get; private set; }
    public CustomerIndustry CustomerIndustry { get; private set; }
    public CustomerName CustomerName { get; private set; }
    public CustomerPostCode CustomerPostCode { get; private set; }
    public CustomerCountryRegion CustomerCountryRegion { get; private set; }

    private CustomerDetails()
    {
        CustomerNumber = CustomerNumber.Empty();
        CustomerAddress = CustomerAddress.Empty();
        CustomerBillToName = CustomerBillToName.Empty();
        CustomerBillToNumber = CustomerBillToNumber.Empty();
        CustomerCity = CustomerCity.Empty();
        CustomerDeliveryType = CustomerDeliveryType.Empty();
        CustomerIndustry = CustomerIndustry.Empty();
        CustomerName = CustomerName.Empty();
        CustomerPostCode = CustomerPostCode.Empty();
        CustomerCountryRegion = CustomerCountryRegion.Empty();
    }

    private CustomerDetails(CustomerNumber customerNumber, CustomerAddress customerAddress,
        CustomerBillToName customerBillToName,
        CustomerBillToNumber customerBillToNumber, CustomerCity customerCity, CustomerDeliveryType customerDeliveryType,
        CustomerIndustry customerIndustry, CustomerName customerName, CustomerPostCode customerPostCode,
        CustomerCountryRegion customerCountryRegion)
    {
        CustomerNumber = customerNumber;
        CustomerAddress = customerAddress;
        CustomerBillToName = customerBillToName;
        CustomerBillToNumber = customerBillToNumber;
        CustomerCity = customerCity;
        CustomerDeliveryType = customerDeliveryType;
        CustomerIndustry = customerIndustry;
        CustomerName = customerName;
        CustomerPostCode = customerPostCode;
        CustomerCountryRegion = customerCountryRegion;
    }

    public static CustomerDetails Create(CustomerNumber customerNumber, CustomerAddress customerAddress,
        CustomerBillToName customerBillToName,
        CustomerBillToNumber customerBillToNumber, CustomerCity customerCity, CustomerDeliveryType customerDeliveryType,
        CustomerIndustry customerIndustry, CustomerName customerName, CustomerPostCode customerPostCode,
        CustomerCountryRegion customerCountryRegion)
    {
        return new CustomerDetails(customerNumber, customerAddress, customerBillToName,
            customerBillToNumber, customerCity, customerDeliveryType,
            customerIndustry, customerName, customerPostCode, customerCountryRegion);
    }

    public static CustomerDetails Empty()
    {
        return new CustomerDetails();
    }
}