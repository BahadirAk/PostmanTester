using Microsoft.AspNetCore.Http;
using PostmanTester.Application.Constants;

namespace PostmanTester.Application.Models.Results
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        //public ErrorDataResult(T? data) : base(data, false)
        //{
        //}

        public ErrorDataResult(T? data, string message = "An unkown error has occured.", string messageCode = "unknown_error", int statusCode = StatusCodes.Status400BadRequest) : base(data, false, message, messageCode, statusCode)
        {
        }

        public ErrorDataResult(T? data, string[] messageArray, int statusCode = StatusCodes.Status400BadRequest) : base(data, false, messageArray[1], messageArray[0], statusCode)
        {
        }
    }
}
