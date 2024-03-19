using Insight.BuildingBlocks.Application.Queries;

namespace Insight.UserAccess.Integration.GetPermissionsForUserName;

public sealed class GetPermissionsForUserNameQuery : IQuery<GetPermissionsForUserNameDto>
{
    public string UserName { get; private set; }

    private GetPermissionsForUserNameQuery(string userName)
    {
        UserName = userName;
    }

    public static GetPermissionsForUserNameQuery Create(string userName)
    {
        return new GetPermissionsForUserNameQuery(userName);
    }
}