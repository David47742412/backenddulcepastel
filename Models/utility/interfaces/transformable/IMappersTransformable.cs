namespace websocket_api.Models.utility.interfaces.transformable;

public interface IMappersTransformable<in T, in TD, in TH, in TF, out TS>
{
    TS Convert(T objeto);
    void ConvertSqlCommand(TD command, TH objeto, TF user, char opc);
}