using MercadoPago.Resource.Payment;

namespace MercadoPagoAPI.Services.Contracts;

public interface IMercadoPagoAPIService
{
    #region Methods

    Task<string> CreateCardToken();
    Task CreatePayment(decimal transactionAmount);
    Task<Payment> GetPaymentById(string paymentId);

    #endregion
}