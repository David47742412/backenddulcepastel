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


    public void ConvertSqlCommand(SqlCommand command, TipoDocumento objeto, Usuario user, char opc)
    {
        throw new NotImplementedException();
    }
}