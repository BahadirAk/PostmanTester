using PostmanTester.Application.Constants;
using PostmanTester.Application.Helpers;
using PostmanTester.Application.Interfaces.ExternalServices;
using PostmanTester.Application.Interfaces.Repositories;
using PostmanTester.Application.Interfaces.Services;
using PostmanTester.Application.Models.DataObjects;
using PostmanTester.Application.Models.Enums;
using PostmanTester.Application.Models.Requests.Auth;
using PostmanTester.Application.Models.Results;
using PostmanTester.Domain.Entities;

namespace PostmanTester.Persistance.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRoleRepository roleRepository;
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;

        public AuthService(IRoleRepository roleRepository, IUserRepository userRepository, ITokenService tokenService)
        {
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
            this.tokenService = tokenService;
        }

        public async Task<IDataResult<bool>> RegisterAsync(RegisterRequest registerRequest)
        {
            try
            {
                var roleName = Enum.GetName(typeof(RolesEnum), RolesEnum.User);
                var role = await roleRepository.GetAsync(r => r.Name == roleName);
                if (role == null) return new ErrorDataResult<bool>(false, Messages.DataNotFound);

                var checkEmail = await userRepository.GetAsync(x => x.Email == registerRequest.Email && x.Status != (byte)GeneralEnum.Deleted);
                if (checkEmail != null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataExists);
                }

                var checkPhone = await userRepository.GetAsync(x => x.Phone == registerRequest.Phone && x.RoleId == role.Id && x.Status != (byte)GeneralEnum.Deleted);
                if (checkPhone != null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataExists);
                }

                string name = registerRequest.FirstName;
                string[] passwordValidation = name.Split(' ');
                foreach (var item in passwordValidation)
                {
                    if (registerRequest.Password.ToLower().Contains(item.Replace(" ", "").ToLower()) && item != "")
                    {
                        return new ErrorDataResult<bool>(false, Messages.InvalidPassword);
                    }
                }

                if (registerRequest.Password.ToLower().Contains(registerRequest.LastName.ToLower()))
                {
                    return new ErrorDataResult<bool>(false, Messages.InvalidPassword);
                }

                byte[] passwordsalt, passwordhash;
                HashingHelper.CreatePasswordHash(registerRequest.Password, out passwordsalt, out passwordhash);

                var user = new User()
                {
                    FirstName = registerRequest.FirstName.Trim(),
                    LastName = registerRequest.LastName.Trim(),
                    Address = registerRequest.Address,
                    CreatedBy = null,
                    Email = registerRequest.Email.Replace(" ", ""),
                    Phone = registerRequest.Phone,
                    PasswordSalt = passwordsalt,
                    PasswordHash = passwordhash,
                    RoleId = registerRequest.RoleId
                };
                var addUser = await userRepository.AddAsync(user);
                return addUser.Id != 0 ? new SuccessDataResult<bool>(true) : new ErrorDataResult<bool>(false, Messages.AddFailed);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, Messages.UnknownError);
            }
        }

        public async Task<IDataResult<AccessToken>> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var roleName = Enum.GetName(typeof(RolesEnum), RolesEnum.User);
                var role = await roleRepository.GetAsync(r => r.Name == roleName);
                if (role == null)
                {
                    return new ErrorDataResult<AccessToken>(null, Messages.DataNotFound);
                }

                var getUser = await userRepository.GetAsync(x => x.Email.Trim() == loginRequest.Email && x.Status != (byte)GeneralEnum.Deleted);
                if (getUser == null)
                {
                    return new ErrorDataResult<AccessToken>(null, Messages.DataNotFound);
                }

                if (getUser.Status != (byte)GeneralEnum.Active)
                {
                    return new ErrorDataResult<AccessToken>(null, Messages.DataNotActive);
                }

                if (!HashingHelper.VerifyPasswordHash(loginRequest.Password, getUser.PasswordSalt, getUser.PasswordHash))
                {
                    return new ErrorDataResult<AccessToken>(null, Messages.InvalidPassword);
                }

                List<Role>? roleOfUser = new List<Role> { await roleRepository.GetAsync(x => x.Id == getUser.RoleId) };

                var token = await tokenService.CreateToken(getUser, roleOfUser);
                if (token == null) return new ErrorDataResult<AccessToken>(null, Messages.TokenError);

                return new SuccessDataResult<AccessToken>(token);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<AccessToken>(null, e.Message);
            }
        }
    }
}
