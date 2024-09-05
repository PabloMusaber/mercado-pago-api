using MercadoPago.Resource.Payment;
using MercadoPagoAPI.Repositories.Contracts;
using MercadoPagoAPI.Services.Contracts;

namespace MercadoPagoAPI.Services;

public class MercadoPagoAPIService : IMercadoPagoAPIService
{
    #region Configuration

    private readonly IMercadoPagoAPIRepository _repository;

    public MercadoPagoAPIService(IMercadoPagoAPIRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    #endregion

    #region Methods
    public async Task<string> CreateCardToken()
    {
        return await _repository.CreateCardToken();
    }

    public async Task<Payment> CreatePaymentBySDK(decimal transactionAmount)
    {
        return await _repository.CreatePaymentBySDK(transactionAmount);
    }

    public async Task<Payment> CreatePaymentByHTTPRequest(decimal transactionAmount)
    {
        return await _repository.CreatePaymentByHTTPRequest(transactionAmount);
    }

    public async Task<Payment> GetPaymentById(string paymentId)
    {
        return await _repository.GetPaymentById(paymentId);
    }

    #endregion
}