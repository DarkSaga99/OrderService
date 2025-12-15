using Orders.Domain.Entities;
using Xunit;


public class OrderTests
{
    [Fact]
    public void Create_Order_Should_Work()
    {
        var order = new Order("David", 100);
        Assert.True(order.Total > 0);
    }
}