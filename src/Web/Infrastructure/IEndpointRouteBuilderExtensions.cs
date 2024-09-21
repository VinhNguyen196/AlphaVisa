using System.Diagnostics.CodeAnalysis;

public static class IEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapGet(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "", int version = 1)
    {
        Guard.Against.AnonymousMethod(handler);

        var routeBuilder = builder.MapGet(pattern, handler)
            .WithName(handler.Method.Name)
            .MapToApiVersion(version);

        return builder;
    }

    public static IEndpointRouteBuilder MapPost(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern = "", int version = 1)
    {
        Guard.Against.AnonymousMethod(handler);

        var routeBuilder = builder.MapPost(pattern, handler)
            .WithName(handler.Method.Name)
            .MapToApiVersion(version);

        return builder;
    }

    public static IEndpointRouteBuilder MapPut(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern, int version = 1)
    {
        Guard.Against.AnonymousMethod(handler);

        var routeBuilder = builder.MapPut(pattern, handler)
            .WithName(handler.Method.Name)
            .MapToApiVersion(version);

        return builder;
    }

    public static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder builder, Delegate handler, [StringSyntax("Route")] string pattern, int version = 1)
    {
        Guard.Against.AnonymousMethod(handler);

        var routeBuilder = builder.MapDelete(pattern, handler)
            .WithName(handler.Method.Name)
            .MapToApiVersion(version);

        return builder;
    }
}
