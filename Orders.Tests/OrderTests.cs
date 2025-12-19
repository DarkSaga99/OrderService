using Orders.Domain.Entities;


public class OrderTests
{
    [Fact]
    public void Create_Order_Should_Work()
    {
        var order = new Order("David", 100);
        Assert.True(order.Total > 0);
    }

    [Fact]
    public void Cliente_Requerido()
    {
        var order = new Order("", 100);
        Assert.True(order.Total > 0);
    }

    [Fact]
    public void CreateOrder_WithValidData_ShouldInitializeProperties()
    {
        // Arrange
        var customer = "David";
        var total = 100m;

        // Act
        var order = new Order(customer, total);

        // Assert
        Assert.Equal(customer, order.Customer);
        Assert.Equal(total, order.Total);
        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.True(order.CreatedAt <= DateTime.UtcNow);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void CreateOrder_WithInvalidCustomer_ShouldThrowException(string invalidCustomer)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Order(invalidCustomer, 100m));
        Assert.Equal("Cliente requerido", exception.Message);
    }

    [Fact]
    public void CreateOrder_WithZeroTotal_ShouldThrowException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Order("David", 0));
        Assert.Equal("Total inválido", exception.Message);
    }

    [Fact]
    public void CreateOrder_WithNegativeTotal_ShouldThrowException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Order("David", -50m));
        Assert.Equal("Total inválido", exception.Message);
    }

    [Fact]
    public void CreateOrder_ShouldSetCreatedAtInUtc()
    {
        // Act
        var order = new Order("David", 100m);

        // Assert
        Assert.Equal(DateTimeKind.Utc, order.CreatedAt.Kind);
    }

    [Theory]
    [InlineData("", 100, "Cliente requerido")]      // Nombre vacío
    [InlineData("David", 0, "Total inválido")]      // Total cero
    [InlineData("David", -5, "Total inválido")]     // Total negativo
    public void CreateOrder_WithInvalidData_ShouldThrowException(string customer, decimal total, string expectedMessage)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Order(customer, total));
        Assert.Equal(expectedMessage, ex.Message);
    }

    // 1. EL CAMINO FELIZ (Sube la cobertura de las propiedades)
    [Fact]
    public void CrearOrden_ConDatosValidos_DebeInicializarCorrectamente()
    {
        // Arrange (Preparar)
        var clienteEsperado = "David";
        var totalEsperado = 150.50m;

        // Act (Actuar)
        var orden = new Order(clienteEsperado, totalEsperado);

        // Assert (Afirmar)
        Assert.Multiple(
            () => Assert.Equal(clienteEsperado, orden.Customer),
            () => Assert.Equal(totalEsperado, orden.Total),
            () => Assert.NotEqual(Guid.Empty, orden.Id),
            () => Assert.True((DateTime.UtcNow - orden.CreatedAt).TotalSeconds < 5)
        );
    }

    // 2. LAS VALIDACIONES (Aquí cubres todos los 'if' y excepciones de un solo golpe)
    [Theory]
    [InlineData("", 100, "Cliente requerido")]      // Caso: Nombre vacío
    [InlineData(" ", 100, "Cliente requerido")]     // Caso: Espacios en blanco
    [InlineData(null, 100, "Cliente requerido")]    // Caso: Nulo
    [InlineData("David", 0, "Total inválido")]      // Caso: Total cero
    [InlineData("David", -1, "Total inválido")]     // Caso: Total negativo
    public void CrearOrden_ConDatosInvalidos_DebeLanzarExcepcion(string cliente, decimal total, string mensajeEsperado)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Order(cliente, total));
        Assert.Equal(mensajeEsperado, ex.Message);
    }
}
