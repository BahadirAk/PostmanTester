using PostmanTester.Application.Models.DataObjects;
using PostmanTester.Application.Models.Requests.Auth;
using PostmanTester.Application.Models.Results;

namespace PostmanTester.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<IDataResult<bool>> RegisterAsync(RegisterRequest registerRequest);
        Task<IDataResult<AccessToken>> LoginAsync(LoginRequest loginRequest);
    }
}
