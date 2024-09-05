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

        var webhook = JsonConvert.DeserializeObject<Webhook>(json);

        if (webhook == null)
        {
            Console.WriteLine("Deserialization failed, webhook is null.");
        }

        var paymentId = webhook.Data.Id;

        _ = _service.GetPaymentById(paymentId);

        return Ok();
    }

    [HttpPost("create-payment-sdk")]
    public async Task<IActionResult> CreatePaymentBySDK(decimal transactionAmount)
    {
        var payment = await _service.CreatePaymentBySDK(transactionAmount);
        return Ok(payment);
    }

    [HttpPost("create-payment-http")]
    public async Task<IActionResult> CreatePaymentByHTTPRequest(decimal transactionAmount)
    {
        var payment = await _service.CreatePaymentByHTTPRequest(transactionAmount);
        return Ok(payment);
    }

    [HttpPost("create-card-token")]
    public async Task<string?> CreateCardToken()
    {
        return await _service.CreateCardToken();
    }

}
