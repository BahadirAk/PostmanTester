namespace PostmanTester.Domain.Entities
{
    public class BaseEntity<T>
    {
        public T Id { get; set; }
        public byte Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public User? CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public User? UpdatedUser { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public User? DeletedUser { get; set; }
    }
}
