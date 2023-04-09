using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace websocket_api.Controllers.Customers;

[ApiController]
public class CustomersController : Controller
{
    private List<WebSocket> _sockets = new();


    [Route("/customers")]
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