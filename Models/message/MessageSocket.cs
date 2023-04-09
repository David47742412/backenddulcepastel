namespace websocket_api.Models.message;

public struct MessageSocket<T>
{
    public string Message { get; set; }
    public int Status { get; set; }
    public T?[] Data { get; set; }
    public string Token { get; set; }
}