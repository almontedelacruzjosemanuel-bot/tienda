using BonaoSells.Application.Services;
using BonaoSells.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<BonaoSellsContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BonaoSellsConnection")));

// Servicios de Application
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<CiudadService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<CompraService>();
builder.Services.AddScoped<CompraDetalleService>();
builder.Services.AddScoped<EmpleadoService>();
builder.Services.AddScoped<EspecificacionTecnicaService>();
builder.Services.AddScoped<MarcaService>();
builder.Services.AddScoped<MetodoPagoService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ProveedorService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<VentaService>();
builder.Services.AddScoped<VentaDetalleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();