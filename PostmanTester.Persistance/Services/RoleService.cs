using PostmanTester.Application.Constants;
using PostmanTester.Application.Helpers;
using PostmanTester.Application.Interfaces.ExternalServices;
using PostmanTester.Application.Interfaces.Repositories;
using PostmanTester.Application.Interfaces.Services;
using PostmanTester.Application.Models.Enums;
using PostmanTester.Application.Models.Requests.Role;
using PostmanTester.Application.Models.Responses.Role;
using PostmanTester.Application.Models.Results;
using PostmanTester.Domain.Entities;
using System.Linq.Expressions;

namespace PostmanTester.Persistance.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;
        private readonly ITokenService tokenService;

        public RoleService(IRoleRepository roleRepository, ITokenService tokenService)
        {
            this.roleRepository = roleRepository;
            this.tokenService = tokenService;
        }

        public async Task<IDataResult<bool>> AddAsync(RoleRequest roleRequest)
        {
            try
            {
                var role = await roleRepository.GetAsync(r => r.Name.Replace(" ", "").ToLower() == roleRequest.Name.Replace(" ", "").ToLower() && r.Status != (byte)GeneralEnum.Deleted);
                if (role != null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataExists);
                }
                var addedRole = await roleRepository.AddAsync(new Role
                {
                    Name = roleRequest.Name,
                    CreatedBy = UserIdentityHelper.GetUserId(),
                    Status = roleRequest.Status
                });
                return addedRole.Id != 0 ? new SuccessDataResult<bool>(true) : new ErrorDataResult<bool>(false, Messages.AddFailed);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message);
            }
        }

        public async Task<IDataResult<bool>> ChangeStatusAsync(short id)
        {
            try
            {
                var role = await roleRepository.GetAsync(h => h.Id == id && h.Status != (byte)GeneralEnum.Deleted);
                if (role == null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataNotFound);
                }
                role.Status = role.Status == (byte)GeneralEnum.Active ? (byte)GeneralEnum.Passive : (byte)GeneralEnum.Active;
                role.UpdatedBy = UserIdentityHelper.GetUserId();
                await roleRepository.UpdateAsync(role);
                return new SuccessDataResult<bool>(true);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message);
            }
        }

        public async Task<IDataResult<bool>> DeleteAsync(short id)
        {
            try
            {
                var role = await roleRepository.GetAsync(r => r.Id == id && r.Status != (byte)GeneralEnum.Deleted);
                if (role == null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataNotFound);
                }
                role.UpdatedBy = UserIdentityHelper.GetUserId();
                await roleRepository.DeleteAsync(role);
                return new SuccessDataResult<bool>(true);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message);
            }
        }

        public async Task<IDataResult<RoleResponse>> GetAsync(Expression<Func<Role, bool>> expression)
        {
            try
            {
                var role = await roleRepository.GetAsync(expression);
                if (role == null)
                {
                    return new ErrorDataResult<RoleResponse>(null, Messages.DataNotFound);
                }
                var roleResponse = new RoleResponse
                {
                    Id = role.Id,
                    Name = role.Name,
                    CreatedDate = role.CreatedDate,
                    CreatedBy = role.CreatedBy,
                    UpdatedDate = role.UpdatedDate,
                    UpdatedBy = role.UpdatedBy,
                    DeletedDate = role.DeletedDate,
                    DeletedBy = role.DeletedBy,
                    Status = role.Status
                };
                return new SuccessDataResult<RoleResponse>(roleResponse);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<RoleResponse>(null, e.Message);
            }
        }

        public async Task<IDataResult<List<RoleResponse>>> GetListAsync(Expression<Func<Role, bool>> expression = null)
        {
            try
            {
                var roleList = await roleRepository.GetListAsync(expression);

                var roleListResponse = new List<RoleResponse>();
                foreach (var role in roleList)
                {
                    roleListResponse.Add(new RoleResponse
                    {
                        Id = role.Id,
                        Name = role.Name,
                        CreatedDate = role.CreatedDate,
                        CreatedBy = role.CreatedBy,
                        UpdatedDate = role.UpdatedDate,
                        UpdatedBy = role.UpdatedBy,
                        DeletedDate = role.DeletedDate,
                        DeletedBy = role.DeletedBy,
                        Status = role.Status
                    });
                }
                return new SuccessDataResult<List<RoleResponse>>(roleListResponse);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<List<RoleResponse>>(new List<RoleResponse>(), e.Message);
            }
        }

        public async Task<IDataResult<bool>> HardDeleteAsync(short id)
        {
            try
            {
                var role = await roleRepository.GetAsync(r => r.Id == id);
                if (role == null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataNotFound);
                }
                await roleRepository.RemoveAsync(role);
                return new SuccessDataResult<bool>(true);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message);
            }
        }

        public async Task<IDataResult<bool>> UpdateAsync(RoleRequest roleRequest)
        {
            try
            {
                var role = await roleRepository.GetAsync(r => r.Id == roleRequest.Id && r.Status != (byte)GeneralEnum.Deleted);
                if (role == null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataNotFound);
                }

                if (role.Name.Replace(" ", "").ToLower() != roleRequest.Name.Replace(" ", "").ToLower())
                {
                    var roleCheck = await roleRepository.GetAsync(r => r.Name.Replace(" ", "").ToLower() == roleRequest.Name.Replace(" ", "").ToLower() && r.Status != (byte)GeneralEnum.Deleted);
                    if (roleCheck != null)
                    {
                        return new ErrorDataResult<bool>(false, Messages.DataExists);
                    }
                    role.Name = roleRequest.Name;
                }
                role.Status = roleRequest.Status;
                role.UpdatedBy = UserIdentityHelper.GetUserId();
                await roleRepository.UpdateAsync(role);
                return new SuccessDataResult<bool>(true);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<bool>(false, e.Message);
            }
        }
    }
}
