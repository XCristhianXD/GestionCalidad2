using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GestionCalidad.Backend.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("GestionCalidadContext")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__GestionCalidadContext");

builder.Services.AddDbContext<GestionCalidadContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración del puerto igual a tu código
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Migraciones directas sin try-catch (como el tuyo)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DiagnosticoMedicoContext>();

    context.Database.EnsureDeleted();   // 💣 BORRA TODO
    context.Database.EnsureCreated();   // 🧱 CREA BIEN
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
