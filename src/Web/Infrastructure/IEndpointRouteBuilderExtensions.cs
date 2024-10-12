using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Routing;

public static class IEndpointRouteBuilderExtensions
{
    public static void MapActions(this RouteHandlerBuilder routeBuilder, IList<Action<RouteHandlerBuilder>> actions)
    {
        if (actions is { })
        {
            foreach (var action in actions)
            {
                action(routeBuilder);
            }
        }
    }
    public static IEndpointRouteBuilder MapGet(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "", IList<Action<RouteHandlerBuilder>> actions = default!, int version = 1)
    {
        Guard.Against.AnonymousMethod(handler);

        var routeBuilder = builder.MapGet(pattern, handler)
            .WithName(handler.Method.Name)
            .MapToApiVersion(version);

        MapActions(routeBuilder, actions);

        return builder;
    }

    public static IEndpointRouteBuilder MapPost(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "", IList<Action<RouteHandlerBuilder>> actions = default!, int version = 1)
    {
        Guard.Against.AnonymousMethod(handler);

        var routeBuilder = builder.MapPost(pattern, handler)
            .WithName(handler.Method.Name)
            .MapToApiVersion(version);

        MapActions(routeBuilder, actions);

        return builder;
    }

    public static IEndpointRouteBuilder MapPut(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "", IList<Action<RouteHandlerBuilder>> actions = default!, int version = 1)
    {
        Guard.Against.AnonymousMethod(handler);

        var routeBuilder = builder.MapPut(pattern, handler)
            .WithName(handler.Method.Name)
            .MapToApiVersion(version);

        MapActions(routeBuilder, actions);

        return builder;
    }

    public static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "", IList<Action<RouteHandlerBuilder>> actions = default!, int version = 1)
    {
        Guard.Against.AnonymousMethod(handler);

        var routeBuilder = builder.MapDelete(pattern, handler)
            .WithName(handler.Method.Name)
            .MapToApiVersion(version);

        MapActions(routeBuilder, actions);

        return builder;
    }
}
