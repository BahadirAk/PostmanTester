using Microsoft.IdentityModel.Tokens;
using PostmanTester.Application.Models.DataObjects;
using PostmanTester.Application.Models.Results;
using PostmanTester.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace PostmanTester.Application.Interfaces.ExternalServices
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> CreateJwtSecurityToken(User user, SigningCredentials signingCredentials, List<Role> operationClaims);
        Task<AccessToken> CreateToken(User user, List<Role> roles);
        Task<IDataResult<TokenInfo>> GetTokenInfo(string token);
    }
}
