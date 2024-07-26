using Microsoft.AspNetCore.Http;
using PostmanTester.Application.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace PostmanTester.Application.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                await ValidatioExceptionAsync(httpContext, ex);
            }

            catch (UnauthorizedAccessException ex)
            {
                await ExceptionHandlerAsync(httpContext, ex);
            }
            catch (Exception e)
            {
                await ExceptionHandlerAsync(httpContext, e);
            }
        }

        private async Task ExceptionHandlerAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            string message = ex.Message;

            ErrorResultValidation err = new()
            {
                Data = null,
                Success = false,
                Message = Messages.UnknownError[1],
                MessageCode = Messages.UnknownError[0]
            };

            if (message == Messages.TokenError[1])
            {
                err.MessageCode = Messages.TokenExpire[0];
            }

            var json = JsonSerializer.Serialize(err);
            await httpContext.Response.WriteAsync(json);
        }
        private async Task ValidatioExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            string message = ex.Message;

            ErrorResultValidation err = new()
            {
                Data = null,
                Success = false,
                Message = message,
                MessageCode = message,
            };
            var json = JsonSerializer.Serialize(err);
            await httpContext.Response.WriteAsync(json);
        }

        private class ErrorResultValidation
        {
            public List<object>? Data { get; set; }
            public bool Success { get; set; }
            public string Message { get; set; }
            public string MessageCode { get; set; }
        }
    }
}
