namespace PostmanTester.Application.Models.DataObjects
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string StampExpiration { get; set; }
    }
}
