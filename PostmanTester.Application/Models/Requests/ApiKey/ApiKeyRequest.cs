using PostmanTester.Application.Models.Enums;

namespace PostmanTester.Application.Models.Requests.ApiKey
{
    public class ApiKeyRequest
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public byte Status { get; set; } = (byte)GeneralEnum.Active;
    }
}
