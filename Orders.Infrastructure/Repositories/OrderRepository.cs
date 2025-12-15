using Orders.Application.Interfaces;
using Orders.Domain.Entities;
using Orders.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Orders.Infrastructure.Repositories;


public class OrderRepository : IOrderRepository
{
    private readonly OrdersDbContext _context;


    public OrderRepository(OrdersDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }


    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
    }
}