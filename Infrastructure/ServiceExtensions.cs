using Data;
using Data.Models;
using Data.Repository;
using Data.Repository.Abstraction;
using Domain.Service.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;

namespace ServiceExtensions;

public static class ServiceExtensions
{
    private static IServiceCollection ConfigureDataRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<ISettingsRepository, SettingsRepository>();

        return services;
    }

    private static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ISettingsService, SettingsService>();
        services.AddTransient<ITeamService, TeamService>();
        services.AddTransient<IMatchService, MatchService>();
        services.AddTransient<IAdminService, AdminService>();
        services.AddTransient<GlobalExceptionHandlingMiddleware>();

        services.AddScoped<IAuthService, AuthService>();

        return services;
    }

    private static IServiceCollection ConfigureMSSQLDB(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MainContext>(
            options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

        return services;
    }

    private static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<MainContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static void AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CustomCorsPolicy", builder =>
            {
                var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();
                builder.WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomCors(configuration);

        services.ConfigureMSSQLDB(configuration);

        services.ConfigureIdentity();

        services.AddSingleton(configuration);

        services.ConfigureDataRepositories();

        services.ConfigureApplicationServices(configuration);
    }
}
