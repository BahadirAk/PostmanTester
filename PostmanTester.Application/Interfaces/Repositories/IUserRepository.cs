using PostmanTester.Domain.Entities;

namespace PostmanTester.Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User, int>
    {
    }
}
