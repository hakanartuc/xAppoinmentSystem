using Microsoft.Extensions.DependencyInjection;
using xAppoinmentSystem.Application.Services;

namespace xAppoinmentSystem.Application;

public static class ConfigureServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IBusinessService, BusinessService>();
    }
}