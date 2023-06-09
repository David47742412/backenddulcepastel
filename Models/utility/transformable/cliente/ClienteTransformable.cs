using System.Data;
using Microsoft.Data.SqlClient;
using websocket_api.Models.cliente;
using websocket_api.Models.usuario;
using websocket_api.Models.utility.interfaces.transformable;
using websocket_api.Models.utility.structView;

namespace websocket_api.Models.utility.transformable.cliente;

public class ClienteTransformable : IMappersTransformable<SqlDataReader, SqlCommand, Cliente, Usuario, GenericView>
{
    public GenericView Convert(SqlDataReader objecto)
    {
        return new GenericView
        {
            Value1 = objecto["cliente_id"],
            Value2 = objecto["cliente_nombre"],
            Value3 = objecto["cliente_apellido"],
            Value4 = objecto["tipo_documento_descripcion"],
            Value5 = objecto["cliente_nroDoc"],
            Value6 = objecto["cliente_direccion"],
            Value7 = objecto["cliente_celular"],
            Value8 = objecto["cliente_telefonoFijo"],
            Value9 = objecto["cliente_email"],
            Value10 = objecto["f_nacimiento"]
        };
    }

    public void ConvertSqlCommand(SqlCommand command, Cliente? objeto, Usuario user, char opc)
    {
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@Opc", SqlDbType.Char).Value = opc;
        command.Parameters.Add("@Cliente_id", SqlDbType.VarChar).Value = objeto?.Id ?? "";
        command.Parameters.Add("@Cliente_nombre", SqlDbType.VarChar).Value = objeto?.Nombre ?? "";
        command.Parameters.Add("@Cliente_apellido", SqlDbType.VarChar).Value = objeto?.Apellido ?? "";
        command.Parameters.Add("@TipoDocId", SqlDbType.VarChar).Value = objeto?.TipoDocId ?? "";
        command.Parameters.Add("@NroDoc", SqlDbType.VarChar).Value = objeto?.NroDoc ?? "";
        command.Parameters.Add("@Direccion", SqlDbType.VarChar).Value = objeto?.Direccion ?? "";
        command.Parameters.Add("@Celular", SqlDbType.VarChar).Value = objeto?.Celular ?? "";
        command.Parameters.Add("@TelfFijo", SqlDbType.VarChar).Value = objeto?.TelFijo ?? "";
        command.Parameters.Add("@email", SqlDbType.VarChar).Value = objeto?.Email ?? "";
        command.Parameters.Add("@f_nacimiento", SqlDbType.DateTime).Value = objeto?.FNacimiento;
        command.Parameters.Add("@Usuario_id_create", SqlDbType.VarChar).Value = user.Id;
        command.Parameters.Add("@Usuario_id_update", SqlDbType.VarChar).Value = user.Id;
        command.Parameters.Add("@Msj", SqlDbType.VarChar).Value = "";
    }
}