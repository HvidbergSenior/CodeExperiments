using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User;

public sealed class UserUpdateParameters : ValueObject
{
    public UserName UserName { get; private set; }
    public UserId UserId { get; private set; }
    public FirstName FirstName { get; set; }
    public LastName LastName { get; set; }
    public Email Email { get;set; }
    public UserRole UserType { get; set; }
    public UserStatus Status { get; set; }
    
    private UserUpdateParameters()
    {
        UserName = UserName.Empty();
        UserId = UserId.Empty();
        FirstName = FirstName.Empty();
        LastName = LastName.Empty();
        Email = Email.Empty();
        UserType = UserRole.Admin;
        Status = UserStatus.Active;
    }

    private UserUpdateParameters(UserName userName, UserId userId, FirstName firstName, LastName lastName, Email email, UserRole userType, UserStatus status)
    {
        UserName = userName;
        UserId = userId;
        Email = email;
        UserType = userType;
        Status = status;
        FirstName = firstName;
        LastName = lastName;
    }

    public static UserUpdateParameters Create(UserName userName, UserId userId, FirstName firstName, LastName lastName, Email email, UserRole userType, UserStatus status)
    { 
        return new UserUpdateParameters(userName, userId, firstName, lastName, email, userType, status);
    }
    
    public static UserUpdateParameters Empty()
    {
        return new UserUpdateParameters();
    }
}