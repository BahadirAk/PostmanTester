namespace PostmanTester.Application.Models.Results
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public DataResult(T? data, bool success) : base(success)
        {
            Data = data;
        }

        public DataResult(T? data, bool success, string message, string messageCode, int statusCode) : base(success, message, messageCode, statusCode)
        {
            Data = data;
        }

        public T? Data { get; set; }
    }
}
