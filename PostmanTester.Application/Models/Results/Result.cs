using Microsoft.AspNetCore.Http;
using PostmanTester.Application.Constants;
using System.Net;

namespace PostmanTester.Application.Models.Results
{
    public class Result : IResult
    {
        public Result(bool success)
        {
            Success = success;
            Message = success ? Messages.Success[1] : Messages.UnknownError[1];
            MessageCode = success ? Messages.Success[0] : Messages.UnknownError[0];
            StatusCode = success ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest;
        }

        public Result(bool success, string message, string messageCode, int statusCode) : this(success)
        {
            Message = message;
            MessageCode = messageCode;
            StatusCode = statusCode;
        }

        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? MessageCode { get; set; }
        public int StatusCode { get; set; }
    }
}
