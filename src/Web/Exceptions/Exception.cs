using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AlphaVisa.Web.Exceptions;

public static class ExceptionExtension
{
    public static void RegisterAppExceptions(this IServiceCollection svc)
    {
        svc.AddProblemDetails();
        svc.AddExceptionHandler<FluentValidationExceptionHandler>();
        svc.AddExceptionHandler<GlobalExceptionHandler>();
    }
}

public interface IBaseException
{
    public HttpStatusCode StatusCode { get; init; }

    public string Message { get; init; }
}

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails();
        problemDetails.Instance = httpContext.Request.Path;

        // Customized exceptions
        if (exception is IBaseException ex)
        {
            httpContext.Response.StatusCode = (int)ex.StatusCode;
            problemDetails.Title = ex.Message;
        }
        else
        {
            problemDetails.Title = exception.Message;
        }

        _logger.LogError("Exception occurred: {ProblemDetailTitle}", problemDetails.Title);
        problemDetails.Status = httpContext.Response.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
        return true;
    }
}

public class FluentValidationExceptionHandler : IExceptionHandler
{
    private readonly ILogger _logger;

    public FluentValidationExceptionHandler(ILogger<FluentValidationExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not FluentValidation.ValidationException fluentException)
        {
            return false;
        }

        var problemDetails = new ProblemDetails();
        problemDetails.Instance = httpContext.Request.Path;
        problemDetails.Title = "One or more validation errors occurred.";
        problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
        problemDetails.Extensions.Add("errors", fluentException.Errors.ToList());
        problemDetails.Status = StatusCodes.Status400BadRequest;
        _logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
        return true;
    }
}
