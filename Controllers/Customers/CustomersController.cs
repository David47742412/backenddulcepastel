using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using websocket_api.Models.cliente;
using websocket_api.Models.message;
using websocket_api.Models.utility.jwt;
using websocket_api.Models.utility.structView;

namespace websocket_api.Controllers.Customers;

[ApiController]
public class CustomersController : Controller
{
    private static List<WebSocket> _sockets = new();
    private static Cliente _repository = new();

    private async Task Manager(WebSocket webSocket)
    {
        _sockets.Add(webSocket);

        try
        {
            var buffer = new byte[1024 * 4];

            while (webSocket.State == WebSocketState.Open)
            {
                var receiveResult =
                    await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                Console.WriteLine($"Received {receiveResult.Count} bytes from socket {webSocket.GetHashCode()}");

                var responseClient =
                    JsonConvert.DeserializeObject<MessageSocket<Cliente>?>(
                        Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, receiveResult.Count))) ??
                    new MessageSocket<Cliente> { Message = "disconnect" };

                var identity = new ClaimsIdentity();
                identity.AddClaim(new Claim("__Token", responseClient.Token ?? ""));
                var user = JwtConfig.ValidateToken(identity);

                if (user != null)
                {
                    switch (responseClient.Message.ToLower())
                    {
                        case "find":
                            await webSocket.SendAsync(
                                new ArraySegment<byte>(
                                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(
                                        new MessageSocket<GenericView>
                                        {
                                            Message = "correct",
                                            Data = _repository.Find("", ""),
                                            Status = 200
                                        }))),
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None);
                            break;
                        case "insert":
                            await _repository.Crud(responseClient.Data[0], user, 'N');
                            break;
                        case "update":
                            await _repository.Crud(responseClient.Data[0], user, 'M');
                            break;
                        case "delete":
                            await _repository.Crud(responseClient.Data[0], user, 'E');
                            break;
                    }

                    if (responseClient.Message.ToLower() == "find") continue;
                    var message = new ArraySegment<byte>(Encoding.UTF8.GetBytes(
                        JsonConvert.SerializeObject(new MessageSocket<GenericView>
                            { Message = "correct", Status = 200, Data = _repository.Find("", "") })));

                    foreach (var socket in _sockets.Where(s => s != webSocket && s.State == WebSocketState.Open))
                    {
                        await socket.SendAsync(message, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
                else
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    var message = new ArraySegment<byte>(Encoding.UTF8.GetBytes(
                        JsonConvert.SerializeObject(new MessageSocket<object>
                            { Message = "Unauthorized", Status = 401 })));
                    await webSocket.SendAsync(message, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Socket {webSocket.GetHashCode()} closed due to error: {ex.Message}");
        }
        finally
        {
            _sockets.Remove(webSocket);
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Socket closed", CancellationToken.None);
            Console.WriteLine($"Socket {webSocket.GetHashCode()} closed");
        }
    }

    [Route("/customers")]
    public async Task OnConnect()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var websocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await Manager(websocket);
        }
        else
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}