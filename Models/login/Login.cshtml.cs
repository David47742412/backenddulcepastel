using System.Data;
using Microsoft.Data.SqlClient;
using websocket_api.Models.usuario;
using websocket_api.Models.utility.context;
using websocket_api.Models.utility.transformable.usuario;

namespace websocket_api.Models.login;

public class Login
{
    private readonly UsuarioTransformable _transformable = new();

    public string? Email { get; set; }

    public string? Password { get; set; }

    public Usuario? IsValidUser(Login user)
    {
        try
        {
            using var connection = new SqlConnection(DbContext.Context);
            using var command = new SqlCommand("SP_LOGIN", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Email", SqlDbType.VarChar).Value = user.Email ?? "";
            connection.Open();
            using var result = command.ExecuteReader();
            result.Read();
            if (!result.HasRows) return null;
            var usuario = _transformable.Convert(result);
            var password = result["usuario_password"] as string;
            return BCrypt.Net.BCrypt.Verify(user.Password, password) ? usuario : null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}