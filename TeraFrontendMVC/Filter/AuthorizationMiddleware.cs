using Microsoft.AspNetCore.Mvc.ViewFeatures;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly List<string> _authenticatedRoutes;
    private readonly List<string> _unauthenticatedRoutes;

    public AuthorizationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _authenticatedRoutes = configuration.GetSection("AuthenticatedRoutes").Get<List<string>>() ?? new List<string>();
        _unauthenticatedRoutes = configuration.GetSection("UnauthenticatedRoutes").Get<List<string>>() ?? new List<string>();
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Session.GetString("AuthToken");
        var requestPath = context.Request.Path.Value?.TrimEnd('/') ?? string.Empty;

        // Redirección para usuarios no autenticados
        if (string.IsNullOrEmpty(token) && _authenticatedRoutes.Contains(requestPath))
        {
            context.Session.SetString("ErrorMessage", "Debes iniciar sesión para acceder a esta página/sección.");
            context.Response.Redirect("/Account/Login");
            return;
        }

        // Redirección para usuarios autenticados
        if (!string.IsNullOrEmpty(token) && _unauthenticatedRoutes.Contains(requestPath))
        {
            context.Session.SetString("ErrorMessage", "No se puede acceder a esta página mientras se está autenticado.");
            context.Response.Redirect("/Home");
            return;
        }

        await _next(context);
    }
}
