namespace Orders.Application.DTOs;


// DTO que entra desde la API
public record CreateOrderDto(string Customer, decimal Total);