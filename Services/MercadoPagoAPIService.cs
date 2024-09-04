using MercadoPago.Resource.Payment;
using MercadoPagoAPI.Services.Contracts;

namespace MercadoPagoAPI.Services;

public class MercadoPagoAPIService : IMercadoPagoAPIService
{
    public Task<string> CreateCardToken()
    {
        throw new NotImplementedException();
    }

    public Task CreatePayment(decimal transactionAmount)
    {
        throw new NotImplementedException();
    }

    public Task<Payment> GetPaymentById(string paymentId)
    {
        throw new NotImplementedException();
    }
}