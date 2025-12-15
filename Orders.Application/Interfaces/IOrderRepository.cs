using Orders.Domain.Entities;


namespace Orders.Application.Interfaces;


// Puerto: Application define lo que necesita
public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(Guid id);
}