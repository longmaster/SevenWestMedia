using Flurl.Http.Configuration;
using Infrastructure.UserData;
using Microsoft.Extensions.DependencyInjection;
using Common;
using Application.Interface;
using Infrastructure.Caching;

namespace Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInsfrastructure(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();
            services.AddTransient<IUserClient, UserClient>();
            services.AddTransient<IUserEngine, UserEngine>();


            services.AddScoped<ICacheManager, CachingManager>();
            return services;
        }
    }
}
