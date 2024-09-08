using MercadoPagoAPI.Entities;
using MercadoPagoAPI.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MercadoPagoAPI.Controllers;

[ApiController, Route("mp")]
public class MercadoPagoAPIController : ControllerBase
{
    private readonly IMercadoPagoAPIService _service;

    public MercadoPagoAPIController(IMercadoPagoAPIService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> WebhookAsync()
    {
        Console.WriteLine("Notification received");
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        _ = Task.Run(async () =>
        {
            try
            {
                var webhook = JsonConvert.DeserializeObject<Webhook>(json);

                if (webhook == null)
                {
                    Console.WriteLine("Deserialization failed, webhook is null.");
                    return;
                }

                var paymentId = webhook.Data.Id;

                var payment = await _service.GetPaymentByIdAsync(paymentId);

                Console.WriteLine("Transaction Amount: " + payment.TransactionAmount.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing webhook: " + ex.Message);
            }
        });

        return Ok();
    }

    [HttpPost("create-payment-sdk")]
    public async Task<IActionResult> CreatePaymentBySDK(decimal transactionAmount)
    {
        var payment = await _service.CreatePaymentBySDKAsync(transactionAmount);
        return Ok(payment);
    }

    [HttpPost("create-payment-http")]
    public async Task<IActionResult> CreatePaymentByHTTPRequest(decimal transactionAmount)
    {
        var payment = await _service.CreatePaymentByHTTPRequestAsync(transactionAmount);
        return Ok(payment);
    }

    [HttpPost("create-card-token")]
    public async Task<string?> CreateCardToken()
    {
        return await _service.CreateCardTokenAsync();
    }

}
