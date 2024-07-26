using PostmanTester.Application.Models.Enums;

namespace PostmanTester.Application.Models.Requests.Auth
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public short RoleId { get; set; } = (short)RolesEnum.User;
    }
}
