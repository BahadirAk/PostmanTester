using PostmanTester.Application.Interfaces.Repositories;
using PostmanTester.Domain.Entities;
using PostmanTester.Persistance.Contexts;

namespace PostmanTester.Persistance.Repositories
{
    public class UserRepository : Repository<User, int>, IUserRepository
    {
        public UserRepository(PostmanTesterDbContext context) : base(context)
        {
        }
    }
}
