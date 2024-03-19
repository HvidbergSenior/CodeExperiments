using System.ComponentModel.DataAnnotations;
using Insight.UserAccess.Domain.User;

namespace Insight.UserAccess.Application.GetAllUsersForAdmin
{
    public sealed class GetAllUsersAdminResponse
    {
        [Required]
        public bool HasMoreUsers { get; set; }
        [Required]
        public int TotalAmountOfUsers { get; set; }
        [Required]
        public IReadOnlyList<AllUserAdminResponse> Users { get; private set; }

        public GetAllUsersAdminResponse(IReadOnlyList<AllUserAdminResponse> users, bool hasMoreUsers, int totalAmountOfUsers)
        {
            Users = users;
            HasMoreUsers = hasMoreUsers;
            TotalAmountOfUsers = totalAmountOfUsers;
        }
        
        public sealed class AllUserAdminResponse 
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
        
                public AllUserAdminResponse(
                    string userId,
                    string userName,
                    string email,
                    bool hasFuelConsumptionAccess,
                    bool hasSustainabilityReportAccess,
                    bool hasFleetManagementAccess,
                    UserRole userType,
                    bool blocked, 
                    string firstName, 
                    string lastName)
                {
                    UserName = userName;
                    UserId = userId;
                    Blocked = blocked;
                    FirstName = firstName;
                    LastName = lastName;
                    UserType = userType;
                    HasFleetManagementAccess = hasFleetManagementAccess;
                    HasSustainabilityReportAccess = hasSustainabilityReportAccess;
                    HasFuelConsumptionAccess = hasFuelConsumptionAccess;                    
                    Email = email;
                }
            }
    }
}
