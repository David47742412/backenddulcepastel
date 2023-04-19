using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using websocket_api.Models.message;
using websocket_api.Models.usuario;
using websocket_api.Models.utility.jwt;

namespace websocket_api.Controllers.Login;

[ApiController]
public class LoginController : Controller
{
    private static async Task ValidateUser(WebSocket webSocket)
    {
        try
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
            var login = new Models.login.Login();
            var loginResult =
                JsonConvert.DeserializeObject<Models.login.Login>(
                    Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, receiveResult.Count)));
            var usuario = login.IsValidUser(loginResult ?? new Models.login.Login());
            var token = "";
            if (usuario is not null)
            {
                usuario.Password = "";
                token = JwtConfig.CreateToken(usuario);
            }
            var message = new MessageSocket<Usuario>
            {
                Status = usuario != null ? 200 : 400,
                Message = usuario != null ? "Correct" : "Incorrect",
                Data = new List<Usuario?>{ usuario },
                Token = token
            };

            await webSocket.SendAsync(
                new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message))),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
                );
        }
        catch (Exception)
        {
            // ignored
        }
    }

    [Route("/auth/login")]
    public async Task Login()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync(new WebSocketAcceptContext { DangerousEnableCompression = true });
            await ValidateUser(webSocket);
        }
        else
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
        }
    }
}