using Microsoft.AspNetCore.Mvc;

namespace MercadoPagoAPI.Controllers;

[ApiController, Route("mp")]
public class MercadoPagoAPIController : ControllerBase
{

    [HttpPost("payment-notification")]
    public async Task<IActionResult> PaymentNotification()
    {
        Console.WriteLine("Notification received");
        return Ok();
    }
}
