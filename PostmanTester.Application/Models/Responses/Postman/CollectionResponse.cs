namespace PostmanTester.Application.Models.Responses.Postman
{
    public class CollectionResponse
    {
        public List<Collection> collections { get; set; }
    }

    public class Collection
    {
        public string id { get; set; }
        public string name { get; set; }
        public string owner { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string uid { get; set; }
        public bool isPublic { get; set; }
    }

    public class CollectionDetailResponse
    {
        public CollectionDetail collection { get; set; }
    }

    public class CollectionDetail
    {
        public Info info { get; set; }
        public List<Item> item { get; set; }
        public List<Variable> variable { get; set; }
    }

    public class Info
    {
        public string _postman_id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public string schema { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime createdAt { get; set; }
        public string lastUpdatedBy { get; set; }
        public string uid { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public List<Item>? item { get; set; }
        public string id { get; set; }
        public string uid { get; set; }
        public ProtocolProfileBehavior? protocolProfileBehavior { get; set; }
        public Request? request { get; set; }
        public List<Response>? response { get; set; }
    }

    public class Request
    {
        public string method { get; set; }
        public List<Header> header { get; set; }
        public Body? body { get; set; }
        public Url? url { get; set; }
        public string? description { get; set; }
        public Auth? auth { get; set; }
    }

    public class Url
    {
        public string raw { get; set; }
        public string? protocol { get; set; }
        public List<string> host { get; set; }
        public List<string> path { get; set; }
        public List<Variable>? variable { get; set; }
        public List<Query>? query { get; set; }
    }

    public class Query
    {
        public string key { get; set; }
        public string value { get; set; }
        public string description { get; set; }
    }

    public class Auth
    {
        public string type { get; set; }
        public List<Bearer> bearer { get; set; }
    }

    public class Bearer
    {
        public string key { get; set; }
        public string value { get; set; }
        public string type { get; set; }
    }

    public class Body
    {
        public string? mode { get; set; }
        public List<Urlencoded> urlencoded { get; set; }
        public string raw { get; set; }
        public Options options { get; set; }
    }

    public class Options
    {
        public Raw raw { get; set; }
    }

    public class Raw
    {
        public string headerFamily { get; set; }
        public string language { get; set; }
    }

    public class Urlencoded
    {
        public string description { get; set; }
        public string key { get; set; }
        public string value { get; set; }
    }

    public class Header
    {
        public string key { get; set; }
        public string value { get; set; }
        public string description { get; set; }
    }

    public class ProtocolProfileBehavior
    {
        public bool disableBodyPruning { get; set; }
    }

    public class Response
    {
        public string id { get; set; }
        public string name { get; set; }
        public OriginalRequest originalRequest { get; set; }
        public string status { get; set; }
        public int code { get; set; }
        public string _postman_previewlanguage { get; set; }
        public List<Header> header { get; set; }
        public List<object> cookie { get; set; }
        public object responseTime { get; set; }
        public string body { get; set; }
        public string uid { get; set; }
    }

    public class OriginalRequest
    {
        public string method { get; set; }
        public List<Header> header { get; set; }
        public Body body { get; set; }
        public Url url { get; set; }
    }

    public class Variable
    {
        public string? id { get; set; }
        public string key { get; set; }
        public string value { get; set; }
        public string? description { get; set; }
    }
}
