using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

public sealed class PaymentTermsCode : ValueObject
{
    public string Value { get; private set; }

    private PaymentTermsCode()
    {
        Value = "";
    }

    private PaymentTermsCode(string value)
    {
        Value = value;
    }

    public static PaymentTermsCode Create(string value)
    {
        return new PaymentTermsCode(value);
    }

    public static PaymentTermsCode Empty()
    {
        return new PaymentTermsCode();
    }
}