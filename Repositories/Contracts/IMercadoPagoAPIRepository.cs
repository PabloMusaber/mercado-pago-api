using MercadoPago.Resource.Payment;

namespace MercadoPagoAPI.Repositories.Contracts;

public interface IMercadoPagoAPIRepository
{
    #region Methods

    Task<string> CreateCardToken();
    Task<Payment> CreatePaymentByHTTPRequest(decimal transactionAmount);
    Task<Payment> CreatePaymentBySDK(decimal transactionAmount);
    Task<Payment> GetPaymentById(string paymentId);

    #endregion
}