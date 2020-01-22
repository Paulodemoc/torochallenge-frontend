using Contracts;
using Entities;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Repository;
using StocksService;

namespace StocksBackend.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureStocksService(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<DependenciesManager>(config.GetSection(nameof(DependenciesManager)));
            services.AddSingleton<IDependenciesManager>(dpd => dpd.GetRequiredService<IOptions<DependenciesManager>>().Value);
            services.AddSingleton<IStocksManager, StocksManager>();
        }

        public static void ConfigureMongoDB(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<DatabaseManager>(config.GetSection(nameof(DatabaseManager)));

            services.AddSingleton<IDatabaseManager>(dbm => dbm.GetRequiredService<IOptions<DatabaseManager>>().Value);
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
    }
}