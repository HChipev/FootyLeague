using System.Text;
using Data;
using Data.Models;
using Data.Repository;
using Data.Repository.Abstraction;
using Domain.Service.Abstraction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Services;

namespace Api.Infrastructure;

public static class ServiceExtensions
{
    private static IServiceCollection ConfigureDataRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();

        return services;
    }

    private static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ITeamService, TeamService>();
        services.AddTransient<IMatchService, MatchService>();
        services.AddTransient<IAdminService, AdminService>();
        services.AddTransient<ILoginService, LoginService>();
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

    private static IServiceCollection ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
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

        services.ConfigureJWT(configuration);
    }
}
