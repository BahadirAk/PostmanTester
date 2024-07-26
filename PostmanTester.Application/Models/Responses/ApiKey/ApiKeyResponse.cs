namespace PostmanTester.Application.Models.Responses.ApiKey
{
    public class ApiKeyResponse : BaseResponse
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
    }
}
