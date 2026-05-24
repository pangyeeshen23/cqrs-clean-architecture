using System.Net;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Web.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger
        )
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError($"[Reqeust] : {httpContext.Request.Path}, [Message] : {exception.Message}, [Stack Trace] : {exception.StackTrace}");
            ProblemDetails problemDetails = new ProblemDetails
            {
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            };
            (problemDetails.Status, problemDetails.Title, problemDetails.Detail) = exception switch
            {
                DomainException domainEx =>
                    ((int)HttpStatusCode.BadRequest, "Bad Request", domainEx.Message),
                ValidationException validationEx =>
                    ((int)HttpStatusCode.BadRequest, "Validation Error", "One or more validation errors occurred."),
                UnauthorizedAccessException unauthorizedEx =>
                    ((int)HttpStatusCode.Unauthorized, "Unauthorized", unauthorizedEx.Message),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred.")
            };
            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions["errors"] = validationException.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage,
                    e.AttemptedValue
                });
            }
            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
