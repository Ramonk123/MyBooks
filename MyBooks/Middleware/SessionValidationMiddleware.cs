namespace MyBooks.Middleware;

public class SessionValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SessionValidationMiddleware> _logger;

    public SessionValidationMiddleware(RequestDelegate next, ILogger<SessionValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/Account"))
        {
            await _next(context);
            return;
        }
        
        if (!context.User.Identity.IsAuthenticated)
        {
            _logger.LogWarning($"User {context.User.Identity.Name} is not authenticated");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.Redirect($"/Account/Login/");
            return;
        }
        
        if (context.Session.GetString("UserSession") == null)
        {
            _logger.LogWarning("User session is missing or expired.");

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.Redirect($"/Account/Login/");
            return;
        }

        await _next(context);
    }
}