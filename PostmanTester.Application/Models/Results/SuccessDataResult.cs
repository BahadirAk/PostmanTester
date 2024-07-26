using Microsoft.AspNetCore.Http;
using PostmanTester.Application.Constants;

namespace PostmanTester.Application.Models.Results
{
    public class SuccessDataResult<T> : DataResult<T>
    {
        //public SuccessDataResult(T? data) : base(data, true)
        //{
        //}

        public SuccessDataResult(T? data, string message = "Operation has successfully completed.", string messageCode = "success", int statusCode = StatusCodes.Status200OK) : base(data, true, message, messageCode, statusCode)
        {
        }

        public SuccessDataResult(T? data, string[] messageArray, int statusCode = StatusCodes.Status200OK) : base(data, true, messageArray[1], messageArray[0], statusCode)
        {
        }
    }
}
