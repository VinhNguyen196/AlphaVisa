using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.Builder;

namespace AlphaVisa.Web.Infrastructure;

public static class WebApplicationExtensions
{
    public static ApiVersionSet apiVersionSet = default!;

    public static RouteGroupBuilder MapGroup(this WebApplication app, EndpointGroupBase group)
    {
        var groupName = group.GetType().Name;

        return app
            .MapGroup($"/api/v{{apiVersion:apiVersion}}/{groupName}")
            .WithApiVersionSet(apiVersionSet)
            .WithGroupName(groupName)
            .WithTags(groupName)
            .WithOpenApi();
    }

    public static RouteGroupBuilder MapGroupForIdentity(this WebApplication app, EndpointGroupBase group)
    {
        var groupName = group.GetType().Name;
        var apiVersionOne = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        return app
            .MapGroup($"/api/v{{apiVersion:apiVersion}}/{groupName}")
            .WithApiVersionSet(apiVersionOne)
            .WithGroupName(groupName)
            .WithTags(groupName)
            .WithOpenApi();
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpointGroupType = typeof(EndpointGroupBase);

        var assembly = Assembly.GetExecutingAssembly();

        var endpointGroupTypes = assembly.GetExportedTypes()
            .Where(t => t.IsSubclassOf(endpointGroupType));

        apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        foreach (var type in endpointGroupTypes)
        {
            if (Activator.CreateInstance(type) is EndpointGroupBase instance)
            {
                instance.Map(app);
            }
        }

        return app;
    }
}
