using AutoMapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cadena de conexión con soporte para /data
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=/data/LolaCredits.db";

// Auto-crear el directorio /data si no existe (para Fly.io)
var dataDir = Path.GetDirectoryName(connectionString.Replace("Data Source=", ""));
if (!string.IsNullOrEmpty(dataDir) && !Directory.Exists(dataDir))
{
    Directory.CreateDirectory(dataDir);
}

// EF Core + SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString)
);

// Automapper
builder.Services.AddAutoMapper(typeof(Program));

// Controllers
builder.Services.AddControllers();

// Services
builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<LoanService>();
builder.Services.AddScoped<InstallmentService>();
builder.Services.AddScoped<PaymentService>();

// Swagger (solo en desarrollo)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS con soporte para producción
var allowedOrigins = new List<string>
{
    "http://localhost:5173",
    "http://localhost:3000"
};

// Agregar URL de producción desde variable de entorno
var productionUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");
if (!string.IsNullOrEmpty(productionUrl))
{
    allowedOrigins.Add(productionUrl);
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins(allowedOrigins.ToArray())
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Auto-migración en producción
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Swagger habilitado en producción (proyecto de portafolio)
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");

// Auto-migrate database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.MapControllers();

app.Run();
