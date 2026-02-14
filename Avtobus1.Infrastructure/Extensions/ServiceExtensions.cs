using Avtobus1.Application.Validation;
using Avtobus1.Domain.Entities;
using Avtobus1.Domain.Interfaces;
using Avtobus1.Infrastructure.DbContexts;
using Avtobus1.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace Avtobus1.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<UrlDbContext>(opts =>
            opts.UseMySql(connectionString, 
                ServerVersion.AutoDetect(connectionString),
                b => b.MigrationsAssembly("Avtobus1.Infrastructure"))); 
    }
    
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo { Title = "Avtobus1 API", Version = "v1" });
        });
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<UrlRecord>, UrlValidator>();
    }
    
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UrlDbContext>();
        if (dbContext.Database.IsRelational())
        {
            dbContext.Database.Migrate();
        }
    }
}