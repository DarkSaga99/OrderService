using Microsoft.EntityFrameworkCore;
using Orders.Application.Interfaces;
using Orders.Application.Sagas;
using Orders.Infrastructure.Data;
using Orders.Infrastructure.External;
using Orders.Infrastructure.Repositories;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
builder.Services.AddControllers();

// 1. Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de EF Core
builder.Services.AddDbContext<OrdersDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Registro de servicios de Repositorio y Saga (Patrón Saga) david2
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<CreateOrderSaga>();

// Configuración de HttpClient con Polly para Resiliencia (Retries y Circuit Breaker)

builder.Services.AddHttpClient<IPaymentService, PaymentService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5116"); // localhost con el puerto correcto
})
.AddPolicyHandler(HttpPolicyExtensions
    .HandleTransientHttpError()
    .WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: retry => TimeSpan.FromSeconds(Math.Pow(2, retry))
    ))
.AddPolicyHandler(HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(
        handledEventsAllowedBeforeBreaking: 3,
        durationOfBreak: TimeSpan.FromSeconds(30)
    ));


var app = builder.Build();


// 2. Middlewares (La parte que hace funcionar la interfaz de Swagger)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // La clave es asegurar que el cliente de la UI sepa dónde buscar el JSON de Swagger
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders API V1");
    });
}
// else { app.UseExceptionHandler("/Error"); } // Recomendado para producción

// app.UseHttpsRedirection(); // Puedes añadir esto si quieres forzar HTTPS
app.MapControllers();

app.Run();