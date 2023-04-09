using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using websocket_api.Models.usuario;
using websocket_api.Models.utility.context;
using websocket_api.Models.utility.interfaces;
using websocket_api.Models.utility.structView;
using websocket_api.Models.utility.transformable.proveedor;

namespace websocket_api.Models.proveedor;

public class Proveedor : IGeneric<Proveedor, GenericView>
{
    private readonly ProveedorTransformable _transformable = new();

    public string? Id { get; set; }

    public string? Ruc { get; set; }

    public string? Ncomercial { get; set; }

    public string? RazonSocial { get; set; }

    public string? Celular { get; set; }

    public string? Telefono { get; set; }

    public string? PaginaWeb { get; set; }

    public string? Email { get; set; }

    public string? Direccion { get; set; }

    public string? Descripcion { get; set; }

    public string? TipoDocId { get; set; }

    public List<GenericView> Find(string data, string param, bool isFecha = false)
    {
        var genericList = new List<GenericView>();
        try
        {
            using var connection = new SqlConnection(DbContext.Context);
            using var command = new SqlCommand("SP_VIEW_PROVEEDOR", connection);
            command.CommandType = CommandType.StoredProcedure;

            if (!data.IsNullOrEmpty() && !param.IsNullOrEmpty() && isFecha == false)
                command.Parameters.Add($"@{param}", SqlDbType.VarChar).Value = data;
            else if (!data.IsNullOrEmpty() && !param.IsNullOrEmpty() && isFecha)
                command.Parameters.Add($"@{param}", SqlDbType.DateTime).Value = data;

            connection.Open();
            using var result = command.ExecuteReader();
            while (result.Read())
                genericList.Add(_transformable.Convert(result));

            connection.Close();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return genericList;
    }

    public async Task<string?> Crud(Proveedor? objecto, Usuario user, char opc)
    {
        var message = "";
        try
        {
            await using var connection = new SqlConnection(DbContext.Context);
            await using var command = new SqlCommand("SP_PROVEEDOR", connection);
            _transformable.ConvertSqlCommand(command, objecto, user, opc);
            connection.Open();
            await using var response = command.ExecuteReader();
            response.Read();
            if (response.HasRows) message = response["Msj"] as string;
            connection.Close();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        return message;
    }
}