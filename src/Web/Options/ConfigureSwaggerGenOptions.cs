using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors.Security;

namespace AlphaVisa.Web.Options;

public class ConfigureSwaggerGenOptions : IConfigureNamedOptions<AspNetCoreOpenApiDocumentGeneratorSettings>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(string? name, AspNetCoreOpenApiDocumentGeneratorSettings options)
    {
        Configure(options);
    }

    public void Configure(AspNetCoreOpenApiDocumentGeneratorSettings options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var documentInfo = new NSwag.OpenApiInfo
            {
                Title = $"AlphaVisa.Api v{description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = description.IsDeprecated ? "This API version has been deprecated." : "AlphaVisa API Documentation",
                Contact = new NSwag.OpenApiContact
                {
                    Name = "AlphaVisa Support",
                    Email = "support@alphavisa.com"
                }
            };

            // Add version-specific OpenAPI documents
            options.DocumentName = description.GroupName;
            options.PostProcess = document =>
            {
                document.Info = documentInfo;
                document.Servers.Clear(); // Remove the default server (which is http://)

                // Add the correct HTTPS URL for production
                document.Servers.Add(new NSwag.OpenApiServer
                {
                    Url = "https://api.alphavisa.vn",  // Replace with your actual domain
                    Description = "Production HTTPS"
                });
            };

            options.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        }
    }
}
