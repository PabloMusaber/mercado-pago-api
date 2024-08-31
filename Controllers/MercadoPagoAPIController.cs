using MercadoPago.Client;
using MercadoPago.Client.Common;
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

    [HttpPost("create-payment-api")]
    public async Task<IActionResult> CreatePayment()
    {
        var requestOptions = new RequestOptions();
        requestOptions.CustomHeaders.Add("x-idempotency-key", Guid.NewGuid().ToString());

        var paymentPayerRequest = new PaymentPayerRequest
        {
            Email = "test_user_123@testuser.com"
        };

        var request = new PaymentCreateRequest
        {
            Installments = 1,
            Payer = paymentPayerRequest,
            TransactionAmount = (decimal?)2323.98,
            Token = "00ec8b59ea2bf33fa13c073be165592f" // You need to generate token card
        };

        var client = new PaymentClient();
        Payment payment = await client.CreateAsync(request, requestOptions);
        Console.WriteLine("PAYMENT ID: " + payment.Id);
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

        decimal? TransactionAmount = await GetPaymentById(paymentId);

        Console.WriteLine("Transaction Amount: " + TransactionAmount.ToString());

    }

    private async Task<decimal?> GetPaymentById(string paymentId)
    {
        var client = new PaymentClient();
        try
        {
            Payment payment = await client.GetAsync(1319381982);
            //Payment payment = await client.GetAsync((long)Convert.ToDouble(paymentId));
            return payment.TransactionAmount;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error: " + ex.Message);
            Console.WriteLine("Error details: " + ex.StackTrace);
            return null;
        }
    }
}
