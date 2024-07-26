namespace PostmanTester.Application.Models.Results
{
    public interface IResult
    {
        bool Success { get; }
        string? Message { get; }
        string? MessageCode { get; }
        int StatusCode { get; }
    }
}
