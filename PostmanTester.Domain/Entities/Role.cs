namespace PostmanTester.Domain.Entities
{
    public class Role : BaseEntity<short>
    {
        public string Name { get; set; }

        public ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
