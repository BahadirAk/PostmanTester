namespace PostmanTester.Application.Models.DataObjects
{
    public class TokenInfo
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public DateTime StampBegin { get; set; }
        public DateTime StampExpiration { get; set; }
    }
}
