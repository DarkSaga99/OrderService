namespace Orders.Domain.Entities;


// Entidad raíz del agregado Order
public class Order
{
    public Guid Id { get; private set; }
    public string Customer { get; private set; }
    public decimal Total { get; private set; }
    public DateTime CreatedAt { get; private set; }


    // Constructor para EF Core
    private Order() { }


    public Order(string customer, decimal total)
    {
        if (string.IsNullOrWhiteSpace(customer))
            throw new ArgumentException("Cliente requerido");


        if (total <= 0)
            throw new ArgumentException("Total inválido");


        Id = Guid.NewGuid();
        Customer = customer;
        Total = total;
        CreatedAt = DateTime.UtcNow;
    }
}