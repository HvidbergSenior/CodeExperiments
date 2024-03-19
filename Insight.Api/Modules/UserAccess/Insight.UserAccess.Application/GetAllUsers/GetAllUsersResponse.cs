using System.ComponentModel.DataAnnotations;
using Insight.UserAccess.Domain.User;

namespace Insight.UserAccess.Application.GetAllUsers
{
    public sealed class GetAllUsersResponse
    {
        [Required]
        public bool HasMoreUsers { get; set; }
        [Required]
        public int TotalAmountOfUsers { get; set; }
        [Required]
        public IReadOnlyList<AllUserResponse> Users { get; private set; }

        public GetAllUsersResponse(IReadOnlyList<AllUserResponse> users, bool hasMoreUsers, int totalAmountOfUsers)
        {
            Users = users;
            HasMoreUsers = hasMoreUsers;
            TotalAmountOfUsers = totalAmountOfUsers;
        }
        
        public sealed class AllUserResponse 
        {
                [Required]
                public string UserId { get; private set; }
                [Required]
                public string UserName { get; private set; }
                [Required]
                public string FirstName { get; private set; }
                [Required]
                public string LastName { get; private set; }
                [Required]
                public string Email { get; private set; }
                [Required]
                public bool HasFuelConsumptionAccess { get; private set; }
                [Required]
                public bool HasSustainabilityReportAccess { get; private set; }
                [Required]
                public bool HasFleetManagementAccess { get; private set; }
                [Required]
                public UserRole UserType { get; private set; }
                [Required]
                public bool Blocked { get; private set; }
        
                public AllUserResponse(
                    string userId,
                    string firstName,
                    string lastName,
                    string userName,
                    string email,
                    bool hasFuelConsumptionAccess,
                    bool hasSustainabilityReportAccess,
                    bool hasFleetManagementAccess,
                    UserRole userType,
                    bool blocked)
                {
                    UserName = userName;
                    FirstName = firstName;
                    LastName = lastName;
                    UserId = userId;
                    Blocked = blocked;
                    UserType = userType;
                    HasFleetManagementAccess = hasFleetManagementAccess;
                    HasSustainabilityReportAccess = hasSustainabilityReportAccess;
                    HasFuelConsumptionAccess = hasFuelConsumptionAccess;
                    Email = email;
                }
            }
    }
}
