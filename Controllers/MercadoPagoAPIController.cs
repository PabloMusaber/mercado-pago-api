using System.Net.Http.Headers;
using System.Text;
using MercadoPago.Client;
using MercadoPago.Client.Payment;
using MercadoPago.Resource.Payment;
using MercadoPagoAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MercadoPagoAPI.Controllers;

[ApiController, Route("mp")]
public class MercadoPagoAPIController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public MercadoPagoAPIController(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    [HttpPost("webhook")]
    public IActionResult Webhook()
    {
        Console.WriteLine("Notification received");

        _ = ProcessPaymentNotification();

        return Ok();
    }

    [HttpPost("create-payment-api")]
    public async Task<IActionResult> CreatePayment(decimal transactionAmount)
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
            TransactionAmount = transactionAmount,
            NotificationUrl = "https://f2ca-170-79-180-30.ngrok-free.app/mp/webhook", // Endpoint to receive webhook
            Token = await CreateCardToken() // You need to generate token card
        };

        var client = new PaymentClient();
        Payment payment = await client.CreateAsync(request, requestOptions);
        Console.WriteLine("PAYMENT ID: " + payment.Id);
        return Ok();
    }

    [HttpPost("create-payment-http")]
    public async Task<IActionResult> CreatePaymentRequest(decimal transactionAmount)
    {
        string url = "https://api.mercadopago.com/v1/payments";
        string bearerToken = _configuration.GetSection("MercadoPago:AccessToken").Get<string>(); // Read token from configuration

        var requestBody = new
        {
            transaction_amount = transactionAmount,
            token = await CreateCardToken(),
            description = "Test Payment",
            notification_url = "https://f2ca-170-79-180-30.ngrok-free.app/mp/webhook",
            installments = 1,
            payer = new
            {
                email = "test_user_123@testuser.com"
            }
        };

        string jsonBody = JsonConvert.SerializeObject(requestBody);

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        _httpClient.DefaultRequestHeaders.Add("X-Idempotency-Key", Guid.NewGuid().ToString());

        HttpResponseMessage response = await _httpClient.SendAsync(request);

        string responseContent = await response.Content.ReadAsStringAsync();

        var payment = JsonConvert.DeserializeObject<Payment>(responseContent);

        return Ok();
    }

    [HttpPost("create-card-token")]
    public async Task<string?> CreateCardToken()
    {
        string url = "https://api.mercadopago.com/v1/card_tokens";
        string bearerToken = _configuration.GetSection("MercadoPago:AccessToken").Get<string>(); // Read token from configuration

        var requestBody = new
        {
            card_number = "5031755734530604",
            expiration_month = 11,
            expiration_year = 2025,
            security_code = "123",
            cardholder = new
            {
                name = "APRO",
                identification = new
                {
                    type = "DNI",
                    number = "12345678"
                }
            }
        };

        string jsonBody = JsonConvert.SerializeObject(requestBody);

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

        HttpResponseMessage response = await _httpClient.SendAsync(request);

        string responseContent = await response.Content.ReadAsStringAsync();

        var tokenCard = JsonConvert.DeserializeObject<CardTokenResponse>(responseContent);

        if (tokenCard == null)
        {
            Console.WriteLine("Deserialization failed, tokenCard is null.");
            return null;
        }

        var tokenCardId = tokenCard.Id;
        Console.WriteLine($"Token Id: {tokenCardId}");

        return tokenCardId;
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
            Payment payment = await client.GetAsync((long)Convert.ToDouble(paymentId));
            return payment.TransactionAmount;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error: " + ex.Message);
            return null;
        }
    }

}
