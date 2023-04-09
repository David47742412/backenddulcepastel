using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using websocket_api.Models.usuario;
using websocket_api.Models.utility.jwt;

namespace websocket_api.Models.utility.cookie;

public static class GetCookie
{
    public static async Task<Usuario?> GetData(HttpContext context)
    {
        try
        {
            var authenticationTicket =
                await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var tokenValue = authenticationTicket.Principal?.FindFirst(ClaimTypes.Name)?.Value;
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim("__Token", tokenValue!));
            return JwtConfig.ValidateToken(identity)!;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}