using System.Security.Claims;

namespace Insight.BuildingBlocks.Application
{
    public interface IIdentityResolver
    { 
        Guid ResolveUserId(Claim? providerId, Claim? userId, Claim? name);
        
        string ResolveUserName(Claim? providerId, Claim? userId, Claim? name);
    }
}
