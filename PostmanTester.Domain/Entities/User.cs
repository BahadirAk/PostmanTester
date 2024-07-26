namespace PostmanTester.Domain.Entities
{
    public class User : BaseEntity<int>
    {
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public short RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<ApiKey> ApiKeys { get; set; } = new HashSet<ApiKey>();

        public ICollection<User> UserCreatedUsers { get; set; } = new HashSet<User>();
        public ICollection<User> UserUpdatedUsers { get; set; } = new HashSet<User>();
        public ICollection<User> UserDeletedUsers { get; set; } = new HashSet<User>();

        public ICollection<Role> RoleCreatedUsers { get; set; } = new HashSet<Role>();
        public ICollection<Role> RoleUpdatedUsers { get; set; } = new HashSet<Role>();
        public ICollection<Role> RoleDeletedUsers { get; set; } = new HashSet<Role>();

        public ICollection<ApiKey> ApiKeyCreatedUsers { get; set; } = new HashSet<ApiKey>();
        public ICollection<ApiKey> ApiKeyUpdatedUsers { get; set; } = new HashSet<ApiKey>();
        public ICollection<ApiKey> ApiKeyDeletedUsers { get; set; } = new HashSet<ApiKey>();
    }
}
