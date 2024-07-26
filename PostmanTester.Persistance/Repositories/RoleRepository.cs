using PostmanTester.Application.Interfaces.Repositories;
using PostmanTester.Domain.Entities;
using PostmanTester.Persistance.Contexts;

namespace PostmanTester.Persistance.Repositories
{
    public class RoleRepository : Repository<Role, short>, IRoleRepository
    {
        public RoleRepository(PostmanTesterDbContext context) : base(context)
        {
        }
    }
}
