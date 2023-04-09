using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using websocket_api.Models.usuario;

namespace websocket_api.Models.utility.jwt;

public class JwtConfig
{
    private static JwtConfig? _configuration;

    private JwtConfig(IReadOnlyList<string?> data)
    {
        Key = data[0];
        Issuer = data[1];
        Audience = data[2];
        Subject = data[3];
    }

    private static string? Key { get; set; }
    private static string? Issuer { get; set; }
    private static string? Audience { get; set; }
    private static string? Subject { get; set; }

    public static JwtConfig GetInstance(List<string>? data = null)
    {
        return _configuration ??= new JwtConfig(data!);
    }

    public static string CreateToken(Usuario usuario)
    {
        try
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, Subject!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim("id", usuario.Id!),
                new Claim("usuarioEmail", usuario.Email!)
            };

            var token = new JwtSecurityToken(
                Issuer,
                Audience,
                claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key!)),
                    SecurityAlgorithms.HmacSha384)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static Usuario? ValidateToken(ClaimsIdentity identity)
    {
        var user = new Usuario();
        try
        {
            if (!identity.Claims.Any()) return null;
            var token = identity.Claims.First(u => u.Type == "__Token").Value;
            if (string.IsNullOrWhiteSpace(token)) return null;

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key!)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var utility = new JwtSecurityTokenHandler();
            ClaimsPrincipal? principal;
            try
            {
                principal = utility.ValidateToken(token, validationParameters, out _);
            }
            catch (SecurityTokenException)
            {
                return null;
            }

            if (principal == null) return null;
            user.Id = principal.FindFirst("id")?.Value;
            user.Email = principal.FindFirst("usuarioEmail")?.Value;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return user;
    }
}