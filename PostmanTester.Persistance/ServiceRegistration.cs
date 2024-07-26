using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostmanTester.Application.Interfaces.Repositories;
using PostmanTester.Application.Interfaces.Services;
using PostmanTester.Persistance.Contexts;
using PostmanTester.Persistance.Repositories;
using PostmanTester.Persistance.Services;

namespace PostmanTester.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PostmanTesterDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("PostmanTester"));
            });

            #region REPOSITORIES
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IApiKeyRepository, ApiKeyRepository>();
            #endregion

            #region SERVICES
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IApiKeyService, ApiKeyService>();
            #endregion
        }
    }
}
