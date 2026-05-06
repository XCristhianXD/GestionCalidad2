using Microsoft.EntityFrameworkCore;
using GestionCalidad.Backend.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔥 Puerto dinámico (Render / Docker)
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// 🔥 Connection string (local + Render)
var connectionString =
    builder.Configuration.GetConnectionString("GestionCalidadContext")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__GestionCalidadContext");

// DbContext
builder.Services.AddDbContext<GestionCalidadContext>(options =>
    options.UseNpgsql(connectionString));

// Services
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GestionCalidadContext>();
    context.Database.Migrate();
}

// Swagger solo en desarrollo

app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthorization();
app.MapControllers();

// 🔥 Migraciones automáticas (base de datos en cloud)


app.Run();