using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Insight.BuildingBlocks.Domain;
using Insight.UserAccess.Domain.User;

namespace Insight.UserAccess.Application.UpdateUser
{
    public class UpdateUserRequest
    {
        [Required]
        public required string UserId { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public required string FirstName { get; set; } 
        
        [Required(ErrorMessage = "Last Name is required")]
        public required string LastName { get; set; } 
        
        [Required(ErrorMessage = "Status is required")]
        public required UserStatus Status { get; set; } 
        
        [Required(ErrorMessage = "User Name is required")]
        public required string Username { get; set; }
   
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage ="Role is required")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required UserRole Role { get; set; }
        public required IEnumerable<UpdateUserCustomerPermissionDto> CustomerPermissions { get; set; }
    }
    public class UpdateUserCustomerPermissionDto
    {
        [Required(ErrorMessage = "Customer id is required")]
        public required Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Customer number is required")]
        public required string CustomerNumber { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        public required string CustomerName { get; set; }

        [Required(ErrorMessage = "Customer permissions are required")]
        public required IEnumerable<CustomerPermission> Permissions { get; set; }
    }
}