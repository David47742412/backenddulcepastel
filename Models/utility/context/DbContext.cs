namespace websocket_api.Models.utility.context;

public class DbContext
{
    public static string? Context;

    private static readonly DbContext? _credentials = null;

    private DbContext(string context)
    {
        Context = context;
    }

    public static DbContext GetInstance(string conexion)
    {
        return _credentials ?? new DbContext(conexion);
    }
}