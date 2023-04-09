using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace websocket_api.Controllers.document;

[ApiController]
public class DocumentController : Controller
{

    private List<WebSocket> _sockets = new();

    private static async Task Manager(WebSocket webSocket)
    {
        try
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
        }
        catch (Exception ex)
        {
            
        }
    }
    
    [Route("/document")]
    public async Task OnConnect()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            Console.WriteLine("es websocket");
        }
        else
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}