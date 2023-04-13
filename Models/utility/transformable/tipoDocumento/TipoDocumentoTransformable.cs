using System.Data;
using Microsoft.Data.SqlClient;
using websocket_api.Models.tipoDocumento;
using websocket_api.Models.usuario;
using websocket_api.Models.utility.interfaces.transformable;
using websocket_api.Models.utility.structView;

namespace websocket_api.Models.utility.transformable.tipoDocumento;

public class
    TipoDocumentoTransformable : IMappersTransformable<SqlDataReader, SqlCommand, TipoDocumento, Usuario, GenericView>
{
    public GenericView Convert(SqlDataReader objeto) => new()
    { 
        Value1 = objeto["tipo_documento_id"] as string, 
        Value2 = objeto["tipo_documento_descripcion"] as string
    };


    public virtual void ConvertSqlCommand(SqlCommand command, TipoDocumento? objeto, Usuario user, char opc)
    {
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@Opc", SqlDbType.Char).Value = opc;
        command.Parameters.Add("@id", SqlDbType.VarChar).Value = objeto?.TipoDocId ?? "";
        command.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = objeto?.Descripcion ?? "";
        command.Parameters.Add("@UseridCre", SqlDbType.VarChar).Value = user.Id;
        command.Parameters.Add("@UseridUpd", SqlDbType.VarChar).Value = user.Id;
        command.Parameters.Add("@Msj", SqlDbType.VarChar).Value = "";
    }
}