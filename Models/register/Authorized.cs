using Microsoft.AspNetCore.Mvc.Filters;

namespace websocket_api.Models.register;

public class Authorized : ActionFilterAttribute
{
    public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Console.WriteLine("Hola");
        return base.OnActionExecutionAsync(context, next);
    }
}