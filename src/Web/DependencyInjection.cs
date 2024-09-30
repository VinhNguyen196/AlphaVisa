using Azure.Identity;
using AlphaVisa.Application.Common.Interfaces;
using AlphaVisa.Infrastructure.Data;
using AlphaVisa.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using AlphaVisa.Web.Options;
using Microsoft.Extensions.Options;
using NSwag.Generation.AspNetCore;
using Asp.Versioning.ApiExplorer;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddRazorPages();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddScoped<ISharedLocalizer, SharedLocalizer>();
        services.AddScoped<IHostService, HostService>();
        services.ConfigureOptions<EmailOptionsSetup>();
        services.AddTransient<IConfigureOptions<AspNetCoreOpenApiDocumentGeneratorSettings>, ConfigureSwaggerGenOptions>();
        services.ConfigureOptions<ConfigureSwaggerGenOptions>();

        services.AddEndpointsApiExplorer();

        services.AddApiVersioning(opts =>
        {
            opts.DefaultApiVersion = new(1);
            opts.ApiVersionReader = new UrlSegmentApiVersionReader();
            opts.ReportApiVersions = true; // Includes API versions in response headers
            opts.AssumeDefaultVersionWhenUnspecified = false; // Throws 400 if version is missing
        }).AddApiExplorer(opts =>
        {
            opts.GroupNameFormat = "'v'V";
            opts.SubstituteApiVersionInUrl = true;
        });

        // Register NSwag services
        services.AddOpenApiDocument(configure =>
        {
            // Customize your document here or use the ConfigureSwaggerGenOptions
            var serviceProvider = services.BuildServiceProvider();
            var apiVersionDescriptionProvider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
            var optionsConfigurator = new ConfigureSwaggerGenOptions(apiVersionDescriptionProvider);

            optionsConfigurator.Configure(configure);
        });

        return services;
    }

    public static IServiceCollection AddKeyVaultIfConfigured(this IServiceCollection services, ConfigurationManager configuration)
    {
        var keyVaultUri = configuration["KeyVaultUri"];
        if (!string.IsNullOrWhiteSpace(keyVaultUri))
        {
            configuration.AddAzureKeyVault(
                new Uri(keyVaultUri),
                new DefaultAzureCredential());
        }

        return services;
    }
}
