using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using xAppoinmentSystem.Application.Interfaces;
using xAppoinmentSystem.Infrastructure.Data;
using xAppoinmentSystem.Infrastructure.Services;

namespace xAppoinmentSystem.Infrastructure;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
        
       services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
       
       services.AddStackExchangeRedisCache(options =>
       {
           options.Configuration = configuration.GetConnectionString("Redis");
       });
       
       
       services.AddScoped<ITieredCacheService, TieredCacheService>();
       services.AddMemoryCache();
    }
}