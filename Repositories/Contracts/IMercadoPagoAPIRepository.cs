using MercadoPago.Resource.Payment;

namespace MercadoPagoAPI.Repositories.Contracts;

public interface IMercadoPagoAPIRepository
{
    #region Methods

    Task<string> CreateCardTokenAsync();
    Task<Payment> CreatePaymentByHTTPRequestAsync(decimal transactionAmount);
    Task<Payment> CreatePaymentBySDKAsync(decimal transactionAmount);
    Task<Payment> GetPaymentByIdAsync(string paymentId);

    #endregion
}