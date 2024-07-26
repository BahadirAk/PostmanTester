using PostmanTester.Application.Models.Requests.User;
using PostmanTester.Application.Models.Responses.User;
using PostmanTester.Application.Models.Results;
using PostmanTester.Domain.Entities;
using System.Linq.Expressions;

namespace PostmanTester.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<IDataResult<bool>> AddAsync(UserRequest userRequest);
        Task<IDataResult<bool>> UpdateAsync(UserRequest userRequest);
        Task<IDataResult<bool>> ChangeStatusAsync(int id);
        Task<IDataResult<UserResponse>> GetAsync(Expression<Func<User, bool>> expression);
        Task<IDataResult<UserResponse>> GetByTokenAsync();
        Task<IDataResult<List<UserResponse>>> GetListAsync(Expression<Func<User, bool>> expression = null);
        Task<IDataResult<bool>> DeleteAsync(int id);
        Task<IDataResult<bool>> HardDeleteAsync(int id);
    }
}
