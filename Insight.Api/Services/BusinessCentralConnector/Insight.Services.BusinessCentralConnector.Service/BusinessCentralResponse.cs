namespace Insight.Services.BusinessCentralConnector.Service;

public class BusinessCentralResponse<T>
{
    public required T[] Value { get; set; }

}