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
    public async Task<string> CreateCardTokenAsync()
    {
        return await _repository.CreateCardTokenAsync();
    }

    public async Task<Payment> CreatePaymentBySDKAsync(decimal transactionAmount)
    {
        return await _repository.CreatePaymentBySDKAsync(transactionAmount);
    }

    public async Task<Payment> CreatePaymentByHTTPRequestAsync(decimal transactionAmount)
    {
        return await _repository.CreatePaymentByHTTPRequestAsync(transactionAmount);
    }

    public async Task<Payment> GetPaymentByIdAsync(string paymentId)
    {
        return await _repository.GetPaymentByIdAsync(paymentId);
    }

    #endregion
}