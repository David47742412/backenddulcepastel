using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using websocket_api.Models.usuario;
using websocket_api.Models.utility.context;
using websocket_api.Models.utility.interfaces;
using websocket_api.Models.utility.structView;
using websocket_api.Models.utility.transformable.tipoDocumento;

namespace websocket_api.Models.tipoDocumento;

public class TipoDocumento : IGeneric<TipoDocumento, GenericView>
{
    private readonly TipoDocumentoTransformable _transformable = new();

    public string? TipoDocId { get; set; }

    public string? IdUserCre { get; set; }

    public DateTime? Create { get; set; }

    public string? IdUserUpd { get; set; }

    public DateTime? Tupdate { get; set; }

    public string? Descripcion { get; set; }

    public List<GenericView> Find(string data, string param, bool isFecha = false)
    {
        var genericList = new List<GenericView>();
        try
        {
            using var connection = new SqlConnection(DbContext.Context);
            using var command = new SqlCommand("SP_VIEW_TIPO_DOCUMENTO", connection);
            command.CommandType = CommandType.StoredProcedure;

            if (!data.IsNullOrEmpty() && !param.IsNullOrEmpty() && isFecha == false)
                command.Parameters.Add($"@{param}", SqlDbType.VarChar).Value = data;
            else if (!data.IsNullOrEmpty() && !param.IsNullOrEmpty() && isFecha)
                command.Parameters.Add($"@{param}", SqlDbType.DateTime).Value = data;

            connection.Open();
            using var response = command.ExecuteReader();
            while (response.Read())
                genericList.Add(_transformable.Convert(response));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return genericList;
    }

    public Task<string?> Crud(TipoDocumento? objecto, Usuario user, char opc)
    {
        throw new NotImplementedException();
    }
}