namespace PostmanTester.Domain.Entities
{
    public class ApiKey : BaseEntity<int>
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
    }
}
