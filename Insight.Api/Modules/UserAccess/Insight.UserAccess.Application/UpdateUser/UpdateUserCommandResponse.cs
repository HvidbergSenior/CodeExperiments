using System.ComponentModel.DataAnnotations;

namespace Insight.UserAccess.Application.UpdateUser
{
    public sealed class UpdateUserCommandResponse
    {
        [Required] 
        public UpdateUserResponse UpdateUserResponse { get; set; }

        public UpdateUserCommandResponse(UpdateUserResponse updateUserResponse)
        {
            UpdateUserResponse = updateUserResponse;
        }
    }
    public sealed class UpdateUserResponse
    {
        [Required]
        public string UserName { get; private set; }
        [Required]
        public Guid UserId { get; private set; }
       
        public UpdateUserResponse(string userName, Guid userId)
        {
            UserName = userName;
            UserId = userId;
        }
    }
}
