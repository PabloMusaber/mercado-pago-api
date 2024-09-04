using MercadoPagoAPI.Repositories.Contracts;
using MercadoPago.Resource.Payment;

namespace MercadoPagoAPI.Services;

public class MercadoPagoAPIRepository : IMercadoPagoAPIRepository
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