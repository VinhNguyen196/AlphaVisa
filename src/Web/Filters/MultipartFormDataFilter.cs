using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaVisa.Web.Filters;

public class MultipartFormDataFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.HttpContext.Request;

        // Check if the request has a valid content type
        if (request.HasFormContentType &&
            (request.ContentType ?? string.Empty).StartsWith("multipart/form-data", StringComparison.OrdinalIgnoreCase))
        {
            // Continue to the next filter or endpoint handler
            return await next(context);
        }

        // Return a 415 Unsupported Media Type status if the content type is invalid
        return Results.StatusCode(StatusCodes.Status415UnsupportedMediaType);
    }
}
