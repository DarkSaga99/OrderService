using Orders.Application.Interfaces;
using System.Text;
using System.Text.Json;


namespace Orders.Infrastructure.External;


public class PaymentService : IPaymentService
{
    private readonly HttpClient _client;


    public PaymentService(HttpClient client)
    {
        _client = client;
    }


    public async Task ChargeAsync(Guid orderId, decimal amount)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(new { OrderId = orderId, Amount = amount }),
            Encoding.UTF8,
            "application/json"
        );
        var response = await _client.PostAsync("payments/charge", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task RefundAsync(Guid orderId)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(new { OrderId = orderId }),
            Encoding.UTF8,
            "application/json"
        );
        var response = await _client.PostAsync("payments/refund", content);
        response.EnsureSuccessStatusCode();
    }
}