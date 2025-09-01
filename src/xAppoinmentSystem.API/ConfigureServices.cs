namespace xAppoinmentSystem.API;

public static class ConfigureServices
{
    public static void AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}