using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace ServiceExtensions;

public class LogUserNameMiddleware
{
    private readonly RequestDelegate _next;

    public LogUserNameMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        LogContext.PushProperty("UserName", context.User.Identity.Name == null ? "[No User]" : context.User.Identity.Name);

        return _next(context);
    }
}
