using MercadoPago.Resource.Payment;

namespace MercadoPagoAPI.Services.Contracts;

public interface IMercadoPagoAPIService
{
    #region Methods

    Task<string> CreateCardToken();
    Task<Payment> CreatePaymentByHTTPRequest(decimal transactionAmount);
    Task<Payment> CreatePaymentBySDK(decimal transactionAmount);
    Task<Payment> GetPaymentById(string paymentId);

    #endregion
}