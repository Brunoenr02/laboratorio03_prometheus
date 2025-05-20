using ClienteAPI.Data;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// Configuración del DbContext con SQL Server
builder.Services.AddDbContext<BdClientesContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ClienteDB"))
);

// Configuración de Health Checks para SQL Server (sin parámetro 'name')
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("ClienteDB"),
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "sql" });

// Servicios necesarios para la API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware de Prometheus
app.UseMetricServer();                 // /metrics
app.UseHttpMetrics();                  // Métricas HTTP
app.UseHealthChecksPrometheusExporter(options => { }); // Exporta resultados health checks a Prometheus

// Middleware de Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Seguridad HTTPS y autorización (puedes omitir Authorization si no lo usas)
app.UseHttpsRedirection();
app.UseAuthorization();

// Rutas de controladores
app.MapControllers();

// Endpoint personalizado para estado de salud en formato JSON
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Endpoint raíz para verificar que la API está funcionando
app.MapGet("/", () => "API ClienteAPI funcionando correctamente");

// Ejecutar la aplicación
app.Run();
