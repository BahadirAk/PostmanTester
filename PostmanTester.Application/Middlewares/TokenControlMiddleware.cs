using Microsoft.AspNetCore.Http;
using PostmanTester.Application.Interfaces.ExternalServices;

namespace PostmanTester.Application.Middlewares
{
    public class TokenControlMiddleware
    {
        private RequestDelegate _next;
        private readonly ITokenService _tokenHandler;

        public TokenControlMiddleware(RequestDelegate next, ITokenService tokenHandler)
        {
            _next = next;
            _tokenHandler = tokenHandler;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            //var tokenInfo = _tokenHandler.GetTokenInfo();

            await _next(httpContext);
        }
    }
}
