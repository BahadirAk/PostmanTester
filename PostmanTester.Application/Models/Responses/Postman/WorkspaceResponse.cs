using System.Text.Json.Serialization;

namespace PostmanTester.Application.Models.Responses.Postman
{
    public class WorkspaceResponse
    {
        public List<Workspace> workspaces { get; set; }
    }

    public class Workspace
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? description { get; set; }
        public string visibility { get; set; }
        public string createdBy { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? updatedBy { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? createdAt { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? updatedAt { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Collection>? collections { get; set; }
    }

    public class WorkspaceDetailResponse
    {
        public Workspace workspace { get; set; }
    }
}
