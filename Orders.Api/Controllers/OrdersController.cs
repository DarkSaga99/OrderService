using Microsoft.AspNetCore.Mvc;
using Orders.Application.DTOs;
using Orders.Application.Sagas;


namespace Orders.Api.Controllers;


[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly CreateOrderSaga _saga;


    public OrdersController(CreateOrderSaga saga)
    {
        _saga = saga;
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDto dto)
    {
        var id = await _saga.ExecuteAsync(dto);
        return Ok(id);
    }
}