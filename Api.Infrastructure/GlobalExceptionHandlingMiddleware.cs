using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid.Helpers.Errors.Model;

namespace Api.Infrastructure;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException e)
        {
            _logger.LogError(e, e.Message);
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await WriteProblemDetails(context, "Not found", "The requested resource could not be found.");
        }
        catch (ValidationException e)
        {
            _logger.LogError(e, e.Message);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await WriteProblemDetails(context, "Bad request", e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await WriteProblemDetails(context, "Server error", "An internal server error occurred.");
        }
    }

    private static async Task WriteProblemDetails(HttpContext context, string title, string detail)
    {
        var problem = new ProblemDetails
        {
            Status = context.Response.StatusCode,
            Title = title,
            Detail = detail
        };

        var json = JsonConvert.SerializeObject(problem);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(json);
    }
}
