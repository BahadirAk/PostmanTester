using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PostmanTester.Application.Constants;
using PostmanTester.Application.Extensions;
using PostmanTester.Application.Helpers;
using PostmanTester.Application.Interfaces.ExternalServices;
using PostmanTester.Application.Models.DataObjects;
using PostmanTester.Application.Models.Results;
using PostmanTester.Domain.Entities;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PostmanTester.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private TokenOptions tokenOptions;
        private IHttpContextAccessor httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this.httpContextAccessor = httpContextAccessor;
            tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }

        public async Task<JwtSecurityToken> CreateJwtSecurityToken(User user, SigningCredentials signingCredentials, List<Role> roleList)
        {
            var jwt = new JwtSecurityToken
                (
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: DateTime.UtcNow.AddHours(tokenOptions.AccessTokenExpiration),
                notBefore: DateTime.UtcNow,
                claims: SetClaims(user, roleList),
            signingCredentials: signingCredentials
            );
            return jwt;
        }

        public async Task<AccessToken> CreateToken(User user, List<Role> roles)
        {
            var securityKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialHelper.CreateSigningCredentials(securityKey);
            var jwt = await CreateJwtSecurityToken(user, signingCredentials, roles);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            token = TokenEncryptionHelper.EncryptString(token);

            return new AccessToken
            {
                Expiration = jwt.ValidTo,
                StampExpiration = new DateTimeOffset(jwt.ValidTo).ToUnixTimeSeconds().ToString(),
                Token = token
            };
        }

        public async Task<IDataResult<TokenInfo>> GetTokenInfo(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var decryptedToken = new JwtSecurityToken(jwtEncodedString: token);
                var userId = decryptedToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var roleId = decryptedToken.Claims.First(c => c.Type == ClaimTypes.Role).Value;

                TokenInfo tokenInfo = new()
                {
                    RoleId = roleId,
                    StampBegin = decryptedToken.ValidFrom,
                    StampExpiration = decryptedToken.ValidTo,
                    UserId = userId,
                };

                if (tokenInfo.StampExpiration < DateTime.UtcNow)
                {
                    return new ErrorDataResult<TokenInfo>(null, Messages.TokenExpire, StatusCodes.Status401Unauthorized);
                }

                UserIdentityHelper.SetUserInfo(tokenInfo.UserId, tokenInfo.RoleId);

                return new SuccessDataResult<TokenInfo>(tokenInfo);
            }
            return new ErrorDataResult<TokenInfo>(null, Messages.TokenError);
        }

        private string HashString(string token, string salt)
        {
            using (var sha = new System.Security.Cryptography.HMACSHA256())
            {
                byte[] tokenBytes = Encoding.UTF8.GetBytes(token + salt);
                byte[] hasBytes = sha.ComputeHash(tokenBytes);

                string hash = BitConverter.ToString(hasBytes).Replace("-", string.Empty);
                return hash;
            }
        }

        private IEnumerable<Claim> SetClaims(User user, List<Role> roleList)
        {
            var claimList = new List<Claim>();
            claimList.AddNameIdentifier(user.Id.ToString());
            claimList.AddRoles(roleList.Select(c => c.Id).ToArray());
            return claimList;
        }
    }
}
