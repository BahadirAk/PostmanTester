using Microsoft.Extensions.DependencyInjection;
using PostmanTester.Application.Interfaces.ExternalServices;
using PostmanTester.Infrastructure.Services;

namespace PostmanTester.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();

            services.AddTransient<IPostmanService, PostmanService>();
        }
    }
}
