using Microsoft.AspNetCore.Mvc;

namespace websocket_api.Controllers.Main;

[ApiController]
public class MainController : Controller
{
    [Route("/")]
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
