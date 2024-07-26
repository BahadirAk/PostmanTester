namespace PostmanTester.Application.Models.Results
{
    public interface IDataResult<out T> : IResult
    {
        T? Data { get; }
    }
}
