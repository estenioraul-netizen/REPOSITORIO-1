
using Microsoft.EntityFrameworkCore;

using SistemaPrestamosLibros.Application.Contract;
using SistemaPrestamosLibros.Application.Services;

using SistemaPrestamosLibros.Infrastructure.Context;
using SistemaPrestamosLibros.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// CONEXION A SQL SERVER
// ========================================

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CadenaSQL")
    ));

// ========================================
// REGISTRO DE REPOSITORIOS
// ========================================

builder.Services.AddScoped(
    typeof(IGenericRepository<>),
    typeof(GenericRepository<>)
);

builder.Services.AddScoped<LibroRepository>();
builder.Services.AddScoped<PrestamoRepository>();

// ========================================
// REGISTRO DE SERVICIOS DE APPLICATION
// ========================================

builder.Services.AddScoped<ILibroService, LibroService>();
builder.Services.AddScoped<IEstudianteService, EstudianteService>();
builder.Services.AddScoped<IPrestamoService, PrestamoService>();
builder.Services.AddScoped<IDevolucionService, DevolucionService>();

// ========================================
// CONTROLADORES
// ========================================

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// ========================================
// SWAGGER
// ========================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ========================================
// MIDDLEWARE
// ========================================

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
