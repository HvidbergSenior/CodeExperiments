using Insight.BuildingBlocks.Domain;

namespace Insight.Customers.Domain;

//NB! The name for this class, NumberNumber, has to be changed when we receive more info of what it is. 
//It is called Number in Business Central and another property called CustomerNumber exists
public sealed class NumberNumber : ValueObject
{
    public string Value { get; private set; }

    private NumberNumber()
    {
        Value = "";
    }

    private NumberNumber(string value)
    {
        Value = value;
    }

    public static NumberNumber Create(string value)
    {
        return new NumberNumber(value);
    }

    public static NumberNumber Empty()
    {
        return new NumberNumber();
    }
}