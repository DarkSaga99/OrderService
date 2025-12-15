using Orders.Application.DTOs;
using Orders.Application.Interfaces;
using Orders.Domain.Entities;


namespace Orders.Application.Sagas;


// Orquestador explícito
public class CreateOrderSaga
{
    private readonly IOrderRepository _repository;
    private readonly IPaymentService _paymentService;


    public CreateOrderSaga(
    IOrderRepository repository,
    IPaymentService paymentService)
    {
        _repository = repository;
        _paymentService = paymentService;
    }


    public async Task<Guid> ExecuteAsync(CreateOrderDto dto)
    {
        var order = new Order(dto.Customer, dto.Total);


        await _repository.AddAsync(order);


        try
        {
            await _paymentService.ChargeAsync(order.Id, order.Total);
        }
        catch
        {
            // Compensación (SAGA)
            await _paymentService.RefundAsync(order.Id);
            throw;
        }


        return order.Id;
    }
}