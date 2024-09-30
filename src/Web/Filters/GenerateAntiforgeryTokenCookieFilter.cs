using Microsoft.AspNetCore.Antiforgery;

namespace AlphaVisa.Web.Filters;

public class GenerateAntiforgeryTokenCookieFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var antiforgery = context.HttpContext.RequestServices.GetService<IAntiforgery>();

        // Generate and store the antiforgery tokens
        var tokens = antiforgery!.GetAndStoreTokens(context.HttpContext);

        // Send the request token as a JavaScript-readable cookie
        context.HttpContext.Response.Cookies.Append(
            "RequestVerificationToken",
            tokens.RequestToken!,
            new CookieOptions { HttpOnly = false });

        // Proceed with the next filter or endpoint handler
        return await next(context);
    }
}
