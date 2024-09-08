using MercadoPago.Resource.Payment;

namespace MercadoPagoAPI.Services.Contracts;

public interface IMercadoPagoAPIService
{
    #region Methods

    Task<string> CreateCardTokenAsync();
    Task<Payment> CreatePaymentByHTTPRequestAsync(decimal transactionAmount);
    Task<Payment> CreatePaymentBySDKAsync(decimal transactionAmount);
    Task<Payment> GetPaymentByIdAsync(string paymentId);

    #endregion
}