using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PostmanTester.Application.Models.DataObjects;
using System.Security.Claims;

namespace PostmanTester.Application.Helpers
{
    public static class UserIdentityHelper
    {
        private static IHttpContextAccessor _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        public static void SetUserInfo(string userId, string roleId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, roleId)
            };

            var identity = new ClaimsIdentity(claims, "custom");
            var principal = new ClaimsPrincipal(identity);

            _httpContextAccessor.HttpContext.User = principal;
        }

        public static int GetUserId()
        {
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        public static string GetUserFullname()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value + " " + _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Surname)?.Value;
        }

        public static string GetUserEmail()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        }

        public static string GetUserPhoneNumber()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.MobilePhone)?.Value;
        }

        public static string GetUserRoleId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}
