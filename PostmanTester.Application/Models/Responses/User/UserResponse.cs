using PostmanTester.Application.Models.Responses.ApiKey;
using PostmanTester.Application.Models.Responses.Role;

namespace PostmanTester.Application.Models.Responses.User
{
    public class UserResponse : BaseResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public RoleResponse Role { get; set; }

        public virtual ICollection<ApiKeyResponse> ApiKeys { get; set; }
    }
}
