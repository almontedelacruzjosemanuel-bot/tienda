using BonaoSells.Application.DTOs;
using BonaoSells.Application.Services;
using BonaoSells.ApplicationTest.Helpers;
using BonaoSells.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BonaoSells.ApplicationTest.Services;

public class EspecificacionTecnicaServiceTests : SqliteServiceTestBase
{
    private readonly EspecificacionTecnicaService _service;

    public EspecificacionTecnicaServiceTests()
    {
        _service = new EspecificacionTecnicaService(Context);
    }

    private async Task<int> SembrarProductoAsync(string nombre = "Laptop")
    {
        var categoria = new Categoria { Nombre = "Cómputo" };
        var marca = new Marca { Nombre = "Asus" };
        Context.Categorias.Add(categoria);
        Context.Marcas.Add(marca);
        await Context.SaveChangesAsync();

        var producto = new Producto
        {
            CategoriaId = categoria.CategoriaId,
            MarcaId = marca.MarcaId,
            Nombre = nombre,
            Costo = 20000m,
            Precio = 28000m
        };
        Context.Productos.Add(producto);
        await Context.SaveChangesAsync();

        return producto.ProductoId;
    }

    [Fact]
    public async Task ObtenerTodas_CuandoNoHayEspecificaciones_RetornaListaVacia()
    {
        var resultado = await _service.ObtenerTodas();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerTodas_CuandoExistenEspecificaciones_RetornaTodasMapeadas()
    {
        var productoId = await SembrarProductoAsync();

        Context.EspecificacionesTecnicas.AddRange(
            new EspecificacionTecnica { ProductoId = productoId, Clave = "RAM", Valor = "16 GB" },
            new EspecificacionTecnica { ProductoId = productoId, Clave = "Almacenamiento", Valor = "512 GB SSD" }
        );
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerTodas();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, e => e.Clave == "RAM" && e.Valor == "16 GB");
        Assert.Contains(resultado, e => e.Clave == "Almacenamiento" && e.Valor == "512 GB SSD");
    }

    [Fact]
    public async Task ObtenerPorId_IdExistente_RetornaEspecificacionDto()
    {
        var productoId = await SembrarProductoAsync();
        var entidad = new EspecificacionTecnica
        {
            ProductoId = productoId,
            Clave = "Procesador",
            Valor = "Intel Core i7"
        };
        Context.EspecificacionesTecnicas.Add(entidad);
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerPorId(entidad.EspecificacionId);

        Assert.NotNull(resultado);
        Assert.Equal(entidad.EspecificacionId, resultado!.EspecificacionId);
        Assert.Equal(productoId, resultado.ProductoId);
        Assert.Equal("Procesador", resultado.Clave);
        Assert.Equal("Intel Core i7", resultado.Valor);
    }

    [Fact]
    public async Task ObtenerPorId_IdInexistente_RetornaNull()
    {
        var resultado = await _service.ObtenerPorId(999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Crear_EspecificacionValida_GuardaEnBaseYAsignaId()
    {
        var productoId = await SembrarProductoAsync();
        var dto = new EspecificacionTecnicaDto
        {
            ProductoId = productoId,
            Clave = "Pantalla",
            Valor = "15.6 pulgadas FHD"
        };

        var resultado = await _service.Crear(dto);

        Assert.True(resultado.EspecificacionId > 0);
        Assert.Equal("Pantalla", resultado.Clave);
        Assert.Equal(1, await Context.EspecificacionesTecnicas.CountAsync());
    }

    [Fact]
    public async Task Crear_ProductoInexistente_LanzaDbUpdateException()
    {
        var dto = new EspecificacionTecnicaDto
        {
            ProductoId = 9999,
            Clave = "RAM",
            Valor = "8 GB"
        };

        await Assert.ThrowsAsync<DbUpdateException>(() => _service.Crear(dto));
    }

    [Fact]
    public async Task Actualizar_IdExistente_ActualizaCamposYRetornaTrue()
    {
        var producto1 = await SembrarProductoAsync("Laptop A");
        var producto2 = await SembrarProductoAsync("Laptop B");
        var entidad = new EspecificacionTecnica
        {
            ProductoId = producto1,
            Clave = "RAM",
            Valor = "8 GB"
        };
        Context.EspecificacionesTecnicas.Add(entidad);
        await Context.SaveChangesAsync();

        var dtoActualizado = new EspecificacionTecnicaDto
        {
            EspecificacionId = entidad.EspecificacionId,
            ProductoId = producto2,
            Clave = "Memoria RAM",
            Valor = "32 GB DDR5"
        };

        var actualizado = await _service.Actualizar(entidad.EspecificacionId, dtoActualizado);
        var enDb = await Context.EspecificacionesTecnicas.FindAsync(entidad.EspecificacionId);

        Assert.True(actualizado);
        Assert.NotNull(enDb);
        Assert.Equal(producto2, enDb!.ProductoId);
        Assert.Equal("Memoria RAM", enDb.Clave);
        Assert.Equal("32 GB DDR5", enDb.Valor);
    }

    [Fact]
    public async Task Actualizar_IdInexistente_RetornaFalse()
    {
        var dto = new EspecificacionTecnicaDto { Clave = "No existe", Valor = "N/A" };

        var actualizado = await _service.Actualizar(999, dto);

        Assert.False(actualizado);
    }

    [Fact]
    public async Task Eliminar_IdExistente_EliminaRegistroYRetornaTrue()
    {
        var productoId = await SembrarProductoAsync();
        var entidad = new EspecificacionTecnica
        {
            ProductoId = productoId,
            Clave = "Color",
            Valor = "Negro"
        };
        Context.EspecificacionesTecnicas.Add(entidad);
        await Context.SaveChangesAsync();

        var eliminado = await _service.Eliminar(entidad.EspecificacionId);

        Assert.True(eliminado);
        Assert.Equal(0, await Context.EspecificacionesTecnicas.CountAsync());
    }

    [Fact]
    public async Task Eliminar_IdInexistente_RetornaFalse()
    {
        var eliminado = await _service.Eliminar(999);

        Assert.False(eliminado);
    }
}
