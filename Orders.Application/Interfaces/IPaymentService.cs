namespace Orders.Application.Interfaces;


// Puerto para integración externa
public interface IPaymentService
{
    Task ChargeAsync(Guid orderId, decimal amount);
    Task RefundAsync(Guid orderId);
}