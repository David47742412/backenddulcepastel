using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using websocket_api.Models.message;
using websocket_api.Models.tipoDocumento;
using websocket_api.Models.utility.jwt;
using websocket_api.Models.utility.structView;

namespace websocket_api.Controllers.document;

[ApiController]
public class DocumentController : Controller
{

    private static List<WebSocket> _sockets = new();
    private readonly TipoDocumento _tipoDocumento = new();

    private async Task Manager(WebSocket webSocket)
    {
        while (true)
        {
            if (_sockets.All(e => e.GetHashCode() != webSocket.GetHashCode())) _sockets.Add(webSocket); 
            try
            {
                var buffer = new byte[1024 * 4];
                var receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
                if (receiveResult.CloseStatus != null)
                {
                    break;
                }
                var responseClient =
                    JsonConvert.DeserializeObject<MessageSocket<TipoDocumento>>
                        (Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, receiveResult.Count)));
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
                                            Data = _tipoDocumento.Find("", ""),
                                            Status = 200
                                        }))),
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None);
                            break;
                        case "insert":
                            await _tipoDocumento.Crud(responseClient.Data[0], user, 'N');
                            break;
                        case "update":
                            await _tipoDocumento.Crud(responseClient.Data[0], user, 'M');
                            break;
                        case "delete":
                            await _tipoDocumento.Crud(responseClient.Data[0], user, 'E');
                            break;
                    }
                    if (responseClient.Message.ToLower() != "find")
                    {
                        foreach (var socket in _sockets)
                        {
                            await socket.SendAsync(
                                new ArraySegment<byte>(Encoding.UTF8.GetBytes(
                                    JsonConvert.SerializeObject(new MessageSocket<GenericView>
                                        { Message = "correct", Status = 200, Data = _tipoDocumento.Find("", "")}))),
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None
                            );
                        }
                    }
                }
                else
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(
                            Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new MessageSocket<object>
                                { Message = "Unauthorized", Status = 401 }))),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    
    [Route("/document")]
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