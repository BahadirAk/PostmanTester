using PostmanTester.Application.Interfaces.Repositories;
using PostmanTester.Domain.Entities;
using PostmanTester.Persistance.Contexts;

namespace PostmanTester.Persistance.Repositories
{
    public class ApiKeyRepository : Repository<ApiKey, int>, IApiKeyRepository
    {
        public ApiKeyRepository(PostmanTesterDbContext context) : base(context)
        {
        }
    }
}
