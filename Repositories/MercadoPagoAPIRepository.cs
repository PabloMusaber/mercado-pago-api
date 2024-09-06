using MercadoPagoAPI.Repositories.Contracts;
using MercadoPago.Resource.Payment;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using MercadoPagoAPI.Entities;
using MercadoPago.Client;
using MercadoPago.Client.Payment;

namespace MercadoPagoAPI.Services;

public class MercadoPagoAPIRepository : IMercadoPagoAPIRepository
{
    #region Configuration

    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public MercadoPagoAPIRepository(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    #endregion

    #region Methods

    public async Task<Payment> CreatePaymentByHTTPRequest(decimal transactionAmount)
    {
        string url = "https://api.mercadopago.com/v1/payments";
        string bearerToken = _configuration.GetSection("MercadoPago:AccessToken").Get<string>(); // Read token from configuration

        var requestBody = new
        {
            transaction_amount = transactionAmount,
            token = await CreateCardToken(),
            description = "Test Payment",
            notification_url = "https://9891-170-79-180-30.ngrok-free.app/mp/webhook",
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

        return payment;
    }

    public async Task<Payment> CreatePaymentBySDK(decimal transactionAmount)
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
            NotificationUrl = "https://9891-170-79-180-30.ngrok-free.app/mp/webhook", // Endpoint to receive webhook
            Token = await CreateCardToken() // You need to generate token card
        };

        var client = new PaymentClient();
        Payment payment = await client.CreateAsync(request, requestOptions);

        return payment;
    }

    public async Task<Payment> GetPaymentById(string paymentId)
    {
        var client = new PaymentClient();
        try
        {
            Payment payment = await client.GetAsync((long)Convert.ToDouble(paymentId));
            return payment;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error: " + ex.Message);
            return null;
        }
    }

    public async Task<string> CreateCardToken()
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

        return tokenCardId;
    }

    #endregion
}