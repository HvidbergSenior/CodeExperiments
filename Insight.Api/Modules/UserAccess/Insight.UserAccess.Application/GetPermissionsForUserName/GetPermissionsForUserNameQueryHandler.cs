using Insight.BuildingBlocks.Application;
using Insight.BuildingBlocks.Application.Queries;
using Insight.BuildingBlocks.Authorization;
using Insight.BuildingBlocks.Exceptions;
using Insight.UserAccess.Domain.User;
using Insight.UserAccess.Integration.GetPermissionsForUserName;

namespace Insight.UserAccess.Application.GetPermissionsForUserName;

internal class GetPermissionsForUserNameQueryHandler : IQueryHandler<GetPermissionsForUserNameQuery, GetPermissionsForUserNameDto>
{
    private readonly IUserRepository userRepository;

    public GetPermissionsForUserNameQueryHandler(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<GetPermissionsForUserNameDto> Handle(GetPermissionsForUserNameQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByUserName(request.UserName, cancellationToken);
        if (user == null)
        {
            throw new BusinessException($"Can't find user with name {request.UserName}");
        }

        return new GetPermissionsForUserNameDto(user.CustomerPermissions);
    }
}

internal class GetPermissionsForUserNameQueryAuthorizer : IAuthorizer<GetPermissionsForUserNameQuery>
{
    private readonly IExecutionContext executionContext;

    public GetPermissionsForUserNameQueryAuthorizer(IExecutionContext executionContext)
    {
        this.executionContext = executionContext;
    }

    public async Task<AuthorizationResult> Authorize(GetPermissionsForUserNameQuery query,
        CancellationToken cancellation)
    {
        return await Task.FromResult(AuthorizationResult.Succeed());
    }
}