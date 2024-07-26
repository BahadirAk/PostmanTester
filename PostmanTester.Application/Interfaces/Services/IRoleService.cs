using PostmanTester.Application.Models.Requests.Role;
using PostmanTester.Application.Models.Responses.Role;
using PostmanTester.Application.Models.Results;
using PostmanTester.Domain.Entities;
using System.Linq.Expressions;

namespace PostmanTester.Application.Interfaces.Services
{
    public interface IRoleService
    {
        Task<IDataResult<bool>> AddAsync(RoleRequest roleRequest);
        Task<IDataResult<List<RoleResponse>>> GetListAsync(Expression<Func<Role, bool>> expression = null);
        Task<IDataResult<RoleResponse>> GetAsync(Expression<Func<Role, bool>> expression);
        Task<IDataResult<bool>> UpdateAsync(RoleRequest roleRequest);
        Task<IDataResult<bool>> ChangeStatusAsync(short id);
        Task<IDataResult<bool>> DeleteAsync(short id);
        Task<IDataResult<bool>> HardDeleteAsync(short id);
    }
}
