using PostmanTester.Application.Constants;
using PostmanTester.Application.Extensions;
using PostmanTester.Application.Helpers;
using PostmanTester.Application.Interfaces.ExternalServices;
using PostmanTester.Application.Interfaces.Repositories;
using PostmanTester.Application.Interfaces.Services;
using PostmanTester.Application.Models.Enums;
using PostmanTester.Application.Models.Requests.ApiKey;
using PostmanTester.Application.Models.Responses.ApiKey;
using PostmanTester.Application.Models.Results;
using PostmanTester.Domain.Entities;
using System.Linq.Expressions;

namespace PostmanTester.Persistance.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly IApiKeyRepository _apiKeyRepository;
        private readonly ITokenService tokenService;

        public ApiKeyService(IApiKeyRepository apiKeyRepository, ITokenService tokenService)
        {
            _apiKeyRepository = apiKeyRepository;
            this.tokenService = tokenService;
        }

        public async Task<IDataResult<bool>> AddAsync(ApiKeyRequest apiKeyRequest)
        {
            try
            {
                var apiKey = await _apiKeyRepository.GetAsync(a => a.UserId == apiKeyRequest.UserId && a.Name == apiKeyRequest.Name && a.Status != (byte)GeneralEnum.Deleted);
                if (apiKey != null) return new ErrorDataResult<bool>(false, Messages.DataExists);

                var addResult = await _apiKeyRepository.AddAsync(new ApiKey
                {
                    UserId = UserIdentityHelper.GetUserRoleId().ParseEnum<RolesEnum>() is RolesEnum.User ? UserIdentityHelper.GetUserId() : apiKeyRequest.UserId,
                    Name = apiKeyRequest.Name,
                    CreatedBy = UserIdentityHelper.GetUserId(),
                });
                return addResult.Id != 0 ? new SuccessDataResult<bool>(true) : new ErrorDataResult<bool>(false, Messages.AddFailed);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message);
            }
        }

        public async Task<IDataResult<bool>> ChangeStatusAsync(int id)
        {
            try
            {
                var apiKey = await _apiKeyRepository.GetAsync(h => h.Id == id && h.Status != (byte)GeneralEnum.Deleted);
                if (apiKey == null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataNotFound);
                }
                apiKey.Status = apiKey.Status == (byte)GeneralEnum.Active ? (byte)GeneralEnum.Passive : (byte)GeneralEnum.Active;
                apiKey.UpdatedBy = UserIdentityHelper.GetUserId();
                await _apiKeyRepository.UpdateAsync(apiKey);
                return new SuccessDataResult<bool>(true);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message);
            }
        }

        public async Task<IDataResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var apiKey = await _apiKeyRepository.GetAsync(r => r.Id == id && r.Status != (byte)GeneralEnum.Deleted);
                if (apiKey == null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataNotFound);
                }
                apiKey.UpdatedBy = UserIdentityHelper.GetUserId();
                await _apiKeyRepository.DeleteAsync(apiKey);
                return new SuccessDataResult<bool>(true);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message);
            }
        }

        public async Task<IDataResult<ApiKeyResponse>> GetAsync(Expression<Func<ApiKey, bool>> expression)
        {
            try
            {
                var apiKey = await _apiKeyRepository.GetAsync(expression);
                if (apiKey == null)
                {
                    return new ErrorDataResult<ApiKeyResponse>(null, Messages.DataNotFound);
                }
                var apiKeyResponse = new ApiKeyResponse
                {
                    Id = apiKey.Id,
                    Name = apiKey.Name,
                    CreatedDate = apiKey.CreatedDate,
                    CreatedBy = apiKey.CreatedBy,
                    UpdatedDate = apiKey.UpdatedDate,
                    UpdatedBy = apiKey.UpdatedBy,
                    DeletedDate = apiKey.DeletedDate,
                    DeletedBy = apiKey.DeletedBy,
                    Status = apiKey.Status
                };
                return new SuccessDataResult<ApiKeyResponse>(apiKeyResponse);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<ApiKeyResponse>(null, e.Message);
            }
        }

        public async Task<IDataResult<List<ApiKeyResponse>>> GetListAsync(Expression<Func<ApiKey, bool>> expression = null)
        {
            try
            {
                var apiKeyList = await _apiKeyRepository.GetListAsync(expression);

                var apiKeyListResponse = new List<ApiKeyResponse>();
                foreach (var role in apiKeyList)
                {
                    apiKeyListResponse.Add(new ApiKeyResponse
                    {
                        Id = role.Id,
                        Name = role.Name,
                        UserId = role.UserId,
                        CreatedDate = role.CreatedDate,
                        CreatedBy = role.CreatedBy,
                        UpdatedDate = role.UpdatedDate,
                        UpdatedBy = role.UpdatedBy,
                        DeletedDate = role.DeletedDate,
                        DeletedBy = role.DeletedBy,
                        Status = role.Status
                    });
                }
                return new SuccessDataResult<List<ApiKeyResponse>>(apiKeyListResponse);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<List<ApiKeyResponse>>(new(), e.Message);
            }
        }

        public async Task<IDataResult<bool>> HardDeleteAsync(int id)
        {
            try
            {
                var apiKey = await _apiKeyRepository.GetAsync(r => r.Id == id);
                if (apiKey == null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataNotFound);
                }
                await _apiKeyRepository.RemoveAsync(apiKey);
                return new SuccessDataResult<bool>(true);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message);
            }
        }

        public async Task<IDataResult<bool>> UpdateAsync(ApiKeyRequest apiKeyRequest)
        {
            try
            {
                var apiKey = await _apiKeyRepository.GetAsync(r => r.Id == apiKeyRequest.Id && r.Status != (byte)GeneralEnum.Deleted);
                if (apiKey == null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataNotFound);
                }

                if (apiKey.Name.Replace(" ", "").ToLower() != apiKey.Name.Replace(" ", "").ToLower())
                {
                    var apiKeyCheck = await _apiKeyRepository.GetAsync(r => r.Name.Replace(" ", "").ToLower() == apiKeyRequest.Name.Replace(" ", "").ToLower() && r.Status != (byte)GeneralEnum.Deleted);
                    if (apiKeyCheck != null)
                    {
                        return new ErrorDataResult<bool>(false, Messages.DataExists);
                    }
                    apiKey.Name = apiKeyRequest.Name;
                }
                apiKey.Status = apiKeyRequest.Status;
                apiKey.UpdatedBy = UserIdentityHelper.GetUserId();
                await _apiKeyRepository.UpdateAsync(apiKey);
                return new SuccessDataResult<bool>(true);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message);
            }
        }
    }
}
