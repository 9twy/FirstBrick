using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AccountService.Exceptions;
namespace AccountService.Handlers;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(exception.Message, "Unhandled exception occurred");
        ProblemDetails problem = exception switch
        {
            AppException ex => HandleApplicationException(ex),
            _ => HandleUnhandledException(exception)
        };
        context.Response.StatusCode = problem.Status ?? 500;
        await context.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
    private ProblemDetails HandleApplicationException(AppException ex)
    {
        return new ProblemDetails
        {
            Title = "Application Error",
            Status = ex.StatusCode,
            Detail = ex.Message,
            Extensions = { ["code"] = ex.ErrorCode }
        };
    }

    private ProblemDetails HandleUnhandledException(Exception ex)
    {
        return new ProblemDetails
        {
            Title = "Internal Server Error",
            Status = StatusCodes.Status500InternalServerError,
            Detail = ex.Message ?? "An unexpected error occurred.",
            Extensions = {
                ["code"] = "INTERNAL_ERROR",
                ["traceId"] = Guid.NewGuid().ToString()
            }
        };
    }
}