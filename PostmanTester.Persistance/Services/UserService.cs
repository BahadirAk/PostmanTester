using PostmanTester.Application.Constants;
using PostmanTester.Application.Helpers;
using PostmanTester.Application.Interfaces.ExternalServices;
using PostmanTester.Application.Interfaces.Repositories;
using PostmanTester.Application.Interfaces.Services;
using PostmanTester.Application.Models.Enums;
using PostmanTester.Application.Models.Requests.User;
using PostmanTester.Application.Models.Responses.ApiKey;
using PostmanTester.Application.Models.Responses.Role;
using PostmanTester.Application.Models.Responses.User;
using PostmanTester.Application.Models.Results;
using PostmanTester.Domain.Entities;
using System.Linq.Expressions;

namespace PostmanTester.Persistance.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
        }

        public async Task<IDataResult<bool>> AddAsync(UserRequest userRequest)
        {
            try
            {
                var user = await userRepository.GetAsync(u => u.Email == userRequest.Email && u.Status != (byte)GeneralEnum.Deleted);
                if (user != null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataExists);
                }

                byte[] passwordsalt, passwordhash;
                HashingHelper.CreatePasswordHash(userRequest.Password, out passwordsalt, out passwordhash);

                var addedUser = await userRepository.AddAsync(new User
                {
                    Email = userRequest.Email,
                    FirstName = userRequest.FirstName,
                    LastName = userRequest.LastName,
                    PasswordSalt = passwordsalt,
                    PasswordHash = passwordhash,
                    Phone = userRequest.Phone,
                    Address = userRequest.Address,
                    RoleId = userRequest.RoleId,
                    CreatedBy = UserIdentityHelper.GetUserId(),
                });
                return addedUser.Id != 0 ? new SuccessDataResult<bool>(true) : new ErrorDataResult<bool>(false, Messages.AddFailed);
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
                var user = await userRepository.GetAsync(h => h.Id == id && h.Status != (byte)GeneralEnum.Deleted);
                if (user == null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataNotFound);
                }
                user.Status = user.Status == (byte)GeneralEnum.Active ? (byte)GeneralEnum.Passive : (byte)GeneralEnum.Active;
                user.UpdatedBy = UserIdentityHelper.GetUserId();
                await userRepository.UpdateAsync(user);
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
                var user = await userRepository.GetAsync(u => u.Id == id && u.Status != (byte)GeneralEnum.Deleted);
                if (user == null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataNotFound);
                }
                user.UpdatedBy = UserIdentityHelper.GetUserId();
                await userRepository.DeleteAsync(user);
                return new SuccessDataResult<bool>(true);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message);
            }
        }

        public async Task<IDataResult<UserResponse>> GetAsync(Expression<Func<User, bool>> expression)
        {
            try
            {
                var user = await userRepository.GetAsync(expression);
                if (user == null)
                {
                    return new ErrorDataResult<UserResponse>(null, Messages.DataNotFound);
                }
                var userResponse = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    Address = user.Address,
                    CreatedDate = user.CreatedDate,
                    CreatedBy = user.CreatedBy,
                    UpdatedDate = user.UpdatedDate,
                    UpdatedBy = user.UpdatedBy,
                    DeletedDate = user.DeletedDate,
                    DeletedBy = user.DeletedBy,
                    Status = user.Status
                };
                return new SuccessDataResult<UserResponse>(userResponse);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<UserResponse>(null, e.Message);
            }
        }

        public async Task<IDataResult<UserResponse>> GetByTokenAsync()
        {
            try
            {
                var user = await userRepository.GetAsync(x => x.Id == UserIdentityHelper.GetUserId(), false, u => u.Role, u => u.ApiKeys);
                if (user == null)
                {
                    return new ErrorDataResult<UserResponse>(null, Messages.DataNotFound);
                }

                var userResponse = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    Address = user.Address,
                    Role = new RoleResponse
                    {
                        Id = user.Role.Id,
                        Name = user.Role.Name
                    },
                    ApiKeys = user.ApiKeys.Select(ak => new ApiKeyResponse
                    {
                        Id = ak.Id,
                        UserId = ak.Id,
                        Name = ak.Name
                    }).ToList()
                };

                return new SuccessDataResult<UserResponse>(userResponse);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<UserResponse>(null, e.Message);
            }
        }

        public async Task<IDataResult<List<UserResponse>>> GetListAsync(Expression<Func<User, bool>> expression = null)
        {
            try
            {
                var userList = await userRepository.GetListAsync(expression);

                var userListResponse = new List<UserResponse>();
                foreach (var user in userList)
                {
                    userListResponse.Add(new UserResponse
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Phone = user.Phone,
                        Address = user.Address,
                        CreatedDate = user.CreatedDate,
                        CreatedBy = user.CreatedBy,
                        UpdatedDate = user.UpdatedDate,
                        UpdatedBy = user.UpdatedBy,
                        DeletedDate = user.DeletedDate,
                        DeletedBy = user.DeletedBy,
                        Status = user.Status
                    });
                }
                return new SuccessDataResult<List<UserResponse>>(userListResponse);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<List<UserResponse>>(new List<UserResponse>(), e.Message);
            }
        }

        public async Task<IDataResult<bool>> HardDeleteAsync(int id)
        {
            try
            {
                var user = await userRepository.GetAsync(u => u.Id == id);
                if (user == null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataNotFound);
                }
                await userRepository.RemoveAsync(user);
                return new SuccessDataResult<bool>(true);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message);
            }
        }

        public async Task<IDataResult<bool>> UpdateAsync(UserRequest userRequest)
        {
            try
            {
                var user = await userRepository.GetAsync(u => u.Id == userRequest.Id && u.Status != (byte)GeneralEnum.Deleted);
                if (user == null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataNotFound);
                }

                if (user.Email != userRequest.Email)
                {
                    var userCheck = await userRepository.GetAsync(u => u.Email == userRequest.Email && u.Status != (byte)GeneralEnum.Deleted);
                    if (userCheck != null)
                    {
                        return new ErrorDataResult<bool>(false, Messages.DataExists);
                    }
                    user.Email = userRequest.Email;
                }
                user.FirstName = userRequest.FirstName;
                user.LastName = userRequest.LastName;
                user.Phone = userRequest.Phone;
                user.Address = userRequest.Address;
                user.RoleId = userRequest.RoleId;
                user.UpdatedBy = UserIdentityHelper.GetUserId();
                user.Status = userRequest.Status ?? user.Status;
                await userRepository.UpdateAsync(user);
                return new SuccessDataResult<bool>(true);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message);
            }
        }
    }
}
