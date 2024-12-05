using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;

public class RoleAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Dictionary<string, List<string>> _roleRestrictedRoutes;

    public RoleAuthorizationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _roleRestrictedRoutes = configuration.GetSection("RoleRestrictedRoutes")
                                             .Get<Dictionary<string, List<string>>>() ?? new Dictionary<string, List<string>>();
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Session.GetString("AuthToken");

        if (!string.IsNullOrEmpty(token))
        {
            // Se decodificar el token y se obtiene el rol
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            var userRoles = roleClaim != null ? new List<string> { roleClaim } : new List<string>();

            var requestPath = context.Request.Path.Value?.TrimEnd('/') ?? string.Empty;

            // Verifica si la ruta requiere roles específicos
            if (_roleRestrictedRoutes.TryGetValue(requestPath, out var allowedRoles))
            {
                if (!userRoles.Any(role => allowedRoles.Contains(role)))
                {
                    context.Session.SetString("ErrorMessage", "No tienes permisos para acceder a esta página, debes ser admin.");
                    context.Response.Redirect("/Account/UserProfile");
                    return;
                }
            }
        }

        await _next(context);
    }
}
