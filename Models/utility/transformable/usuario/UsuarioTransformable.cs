using Microsoft.Data.SqlClient;
using websocket_api.Models.usuario;
using websocket_api.Models.utility.interfaces.transformable;
using websocket_api.Models.utility.structView;

namespace websocket_api.Models.utility.transformable.usuario;

public class UsuarioTransformable : IMappersTransformable<SqlDataReader, SqlCommand, Usuario, Usuario, Usuario>
{
    public GenericView ConvertUsuario(SqlDataReader objeto) => new()
    {
        Value1 = objeto["userId"] as string,
        Value2 = objeto["Nombre"] as string,
        Value3 = objeto["Apellido"] as string,
        Value4 = objeto["Email"] as string,
        Value5 = objeto["Celular"] as string,
        Value6 = objeto["direccion"] as string,
        Value7 = objeto["NroDoc"] as string,
        Value8 = objeto["DesTipoDoc"] as string,
        Value9 = objeto["EstadoDes"] as string,
        Value10 = objeto["Foto"] as string,
        Value11 = (DateTime?) objeto["f_nacimiento"]
    };
    
    public Usuario Convert(SqlDataReader objeto)
    {
        return new Usuario
        {
            Id = objeto["usuario_id"] as string,
            Nombre = objeto["usuario_nombre"] as string,
            Apellido = objeto["usuario_apellido"] as string,
            Email = objeto["usuario_email"] as string,
            Celular = objeto["usuario_celular"] as string,
            Foto = objeto["usuario_foto"] as string,
            FchNacimiento = objeto["f_nacimiento"] as string
        };
    }

    public void ConvertSqlCommand(SqlCommand command, Usuario objeto, Usuario user, char opc)
    {
        throw new NotImplementedException();
    }

    public GenericView ConvertUserById(SqlDataReader objeto)
    {
        return new()
        {
            Value1 = objeto["Nombre"] as string,
            Value2 = objeto["Apellido"] as string,
            Value3 = objeto["Email"] as string,
            Value4 = objeto["Celular"] as string,
            Value5 = objeto["Direccion"] as string,
            Value6 = objeto["NroDoc"] as string,
            Value7 = objeto["Tipo de Documento"] as string,
            Value8 = objeto["Estado_descripcion"] as string,
            Value9 = objeto["Foto"] as string,
            Value10 = objeto["OcuDescripcion"] as string,
            Value11 = objeto["FchNacimiento"]
        };
    }
}