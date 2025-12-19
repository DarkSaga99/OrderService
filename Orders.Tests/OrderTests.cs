using Orders.Domain.Entities;


public class OrderTests
{

    // 1. EL CAMINO FELIZ (Sube la cobertura de las propiedades)
    [Fact]
    public void CrearOrden_ConDatosValidos_DebeInicializarCorrectamente()
    {
        // Arrange (Preparar)
        var clienteEsperado = "David";
        var totalEsperado = 150;

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
