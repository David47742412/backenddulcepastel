using System.Data;
using Microsoft.Data.SqlClient;
using websocket_api.Models.proveedor;
using websocket_api.Models.usuario;
using websocket_api.Models.utility.interfaces.transformable;
using websocket_api.Models.utility.structView;

namespace websocket_api.Models.utility.transformable.proveedor;

public class ProveedorTransformable : IMappersTransformable<SqlDataReader, SqlCommand, Proveedor, Usuario, GenericView>
{
    public GenericView Convert(SqlDataReader objeto)
    {
        return new()
        {
            Value1 = objeto["Codigo"] as string,
            Value2 = objeto["Ruc"] as string,
            Value3 = objeto["NComercial"] as string,
            Value4 = objeto["RazonSocial"] as string,
            Value5 = objeto["Celular"] as string,
            Value6 = objeto["Telefono"] as string,
            Value7 = objeto["Pagina"] as string,
            Value8 = objeto["Email"] as string,
            Value9 = objeto["Direccion"] as string,
            Value10 = objeto["Descripcion"] as string,
            Value11 = objeto["TipoDocDes"] as string
        };
    }

    public void ConvertSqlCommand(SqlCommand command, Proveedor? objeto, Usuario user, char opc)
    {
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@Opc", SqlDbType.Char).Value = opc;
        command.Parameters.Add("@id", SqlDbType.VarChar).Value = objeto?.Id;
        command.Parameters.Add("@Ncomercial", SqlDbType.VarChar).Value = objeto?.Ncomercial;
        command.Parameters.Add("@Ruc", SqlDbType.VarChar).Value = objeto?.Ruc;
        command.Parameters.Add("@RazonSocial", SqlDbType.VarChar).Value = objeto?.RazonSocial;
        command.Parameters.Add("@Celular", SqlDbType.VarChar).Value = objeto?.Celular;
        command.Parameters.Add("@Telefono", SqlDbType.VarChar).Value = objeto?.Telefono;
        command.Parameters.Add("@PaginaWeb", SqlDbType.VarChar).Value = objeto?.PaginaWeb;
        command.Parameters.Add("@Email", SqlDbType.VarChar).Value = objeto?.Email;
        command.Parameters.Add("@Direccion", SqlDbType.VarChar).Value = objeto?.Direccion;
        command.Parameters.Add("@Descripcion", SqlDbType.VarChar).Value = objeto?.Descripcion;
        command.Parameters.Add("@userIdCre", SqlDbType.VarChar).Value = user.Id;
        command.Parameters.Add("@userIdMod", SqlDbType.VarChar).Value = user.Id;
        command.Parameters.Add("@TipoDocId", SqlDbType.VarChar).Value = objeto?.TipoDocId;
        command.Parameters.Add("@Msj", SqlDbType.VarChar).Value = "";
    }
}