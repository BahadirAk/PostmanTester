using System.Text.Json.Serialization;

namespace PostmanTester.Application.Models.Responses.Postman
{
    public class EnvironmentResponse
    {
        public List<Environment> environments { get; set; }
    }

    public class Environment
    {
        public string id { get; set; }
        public string name { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string owner { get; set; }
        public string uid { get; set; }
        public bool isPublic { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<EnvironmentDetail>? values { get; set; }
    }

    public class EnvironmentDetail
    {
        public string key { get; set; }
        public string value { get; set; }
        public bool enabled { get; set; }
        public string type { get; set; }
    }

    public class EnvironmentDetailResponse
    {
        public Environment environment { get; set; }
    }
}
