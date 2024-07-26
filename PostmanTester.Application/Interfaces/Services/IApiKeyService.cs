using PostmanTester.Application.Models.Requests.ApiKey;
using PostmanTester.Application.Models.Responses.ApiKey;
using PostmanTester.Application.Models.Results;
using PostmanTester.Domain.Entities;
using System.Linq.Expressions;

namespace PostmanTester.Application.Interfaces.Services
{
    public interface IApiKeyService
    {
        Task<IDataResult<bool>> AddAsync(ApiKeyRequest apiKeyRequest);
        Task<IDataResult<List<ApiKeyResponse>>> GetListAsync(Expression<Func<ApiKey, bool>> expression = null);
        Task<IDataResult<ApiKeyResponse>> GetAsync(Expression<Func<ApiKey, bool>> expression);
        Task<IDataResult<bool>> UpdateAsync(ApiKeyRequest apiKeyRequest);
        Task<IDataResult<bool>> ChangeStatusAsync(int id);
        Task<IDataResult<bool>> DeleteAsync(int id);
        Task<IDataResult<bool>> HardDeleteAsync(int id);
    }
}
