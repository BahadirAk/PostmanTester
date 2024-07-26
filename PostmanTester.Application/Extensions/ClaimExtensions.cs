using System.Security.Claims;

namespace PostmanTester.Application.Extensions
{
    public static class ClaimExtensions
    {
        public static void AddEmail(this ICollection<Claim> claims, string email)
        {
            claims.Add(new Claim(ClaimTypes.Email, email));
        }

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
        }

        public static void AddRoles(this ICollection<Claim> claims, short[] roleList)
        {

            roleList.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role.ToString())));
        }

        public static void AddPhone(this ICollection<Claim> claims, string phone)
        {
            claims.Add(new Claim(ClaimTypes.MobilePhone, phone));
        }
    }
}
