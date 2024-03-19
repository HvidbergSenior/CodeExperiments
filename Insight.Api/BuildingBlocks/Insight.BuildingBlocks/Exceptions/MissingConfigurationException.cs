namespace Insight.BuildingBlocks.Infrastructure;

public class MissingConfigurationException : Exception
{
    public MissingConfigurationException(){}
    
    public MissingConfigurationException(string message) : base (message){}
    
    public MissingConfigurationException(string message, Exception inner) : base (message, inner){}
    
}