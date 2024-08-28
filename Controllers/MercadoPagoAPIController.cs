using MercadoPagoAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MercadoPagoAPI.Controllers;

[ApiController, Route("mp")]
public class MercadoPagoAPIController : ControllerBase
{

    [HttpPost("webhook")]
    public IActionResult Webhook()
    {
        Console.WriteLine("Notification received");

        _ = ProcessPaymentNotification();

        return Ok();
    }

    private async Task ProcessPaymentNotification()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        var webhook = JsonConvert.DeserializeObject<Webhook>(json);

        if (webhook == null)
        {
            Console.WriteLine("Deserialization failed, webhook is null.");
            return;
        }

        var paymentId = webhook.Data.Id;
        Console.WriteLine($"Payment Id: {paymentId}");
    }
}
