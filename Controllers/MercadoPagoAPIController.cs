using MercadoPago.Client.Payment;
using MercadoPago.Resource.Payment;
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

        var client = new PaymentClient();
        try
        {
            Payment payment = await client.GetAsync(1319381982);
            //Payment payment = await client.GetAsync((long)Convert.ToDouble(paymentId));
            Console.WriteLine("Payment obtained correctly");
            Console.WriteLine("TRANSACTION AMOUNT: " + payment.TransactionAmount);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error: " + ex.Message);
            Console.WriteLine("Error details: " + ex.StackTrace);
        }
    }
}
