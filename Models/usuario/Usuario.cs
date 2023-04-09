using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using websocket_api.Models.message;
using websocket_api.Models.utility.context;
using websocket_api.Models.utility.cookie;
using websocket_api.Models.utility.interfaces;
using websocket_api.Models.utility.structView;
using websocket_api.Models.utility.transformable.usuario;

namespace websocket_api.Models.usuario;

public class Usuario : IGeneric<Usuario, GenericView>
{
    private readonly UsuarioTransformable _transformable = new();

    public string? Id { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Celular { get; set; }

    public string? NroDoc { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? IdTipoDoc { get; set; }

    public string? IdEstado { get; set; }

    public string? Foto { get; set; }

    public string? Ocupacion { get; set; }

    public string? FchNacimiento { get; set; }

    public List<GenericView> Find(string data, string param, bool isFecha = false)
    {
        var lts = new List<GenericView>();
        try
        {
            using var connection = new SqlConnection(DbContext.Context);
            using var command = new SqlCommand("SP_VIEW_USUARIO", connection);
            command.CommandType = CommandType.StoredProcedure;
            
            if (!data.IsNullOrEmpty() && !param.IsNullOrEmpty() && isFecha == false)
                command.Parameters.Add($"@{param}", SqlDbType.VarChar).Value = data;
            else if (!data.IsNullOrEmpty() && !param.IsNullOrEmpty() && isFecha)
                command.Parameters.Add($"@{param}", SqlDbType.DateTime).Value = data;

            connection.Open();
            using var result = command.ExecuteReader();
            while (result.Read())
                lts.Add(_transformable.ConvertUsuario(result));

            connection.Close();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return lts;
    }

    public Task<string?> Crud(Usuario? objecto, Usuario user, char opc)
    {
        throw new NotImplementedException();
    }
}