using BonaoSells.Application.DTOs;
using BonaoSells.Application.Services;
using BonaoSells.ApplicationTest.Helpers;
using BonaoSells.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BonaoSells.ApplicationTest.Services;

public class ProductoServiceTests : SqliteServiceTestBase
{
    private readonly ProductoService _service;

    public ProductoServiceTests()
    {
        _service = new ProductoService(Context);
    }

    private async Task<(int CategoriaId, int MarcaId)> SembrarCatalogosAsync()
    {
        var categoria = new Categoria { Nombre = "Electrónica", Descripcion = "Equipos electrónicos" };
        var marca = new Marca { Nombre = "Samsung", PaisOrigen = "Corea del Sur" };

        Context.Categorias.Add(categoria);
        Context.Marcas.Add(marca);
        await Context.SaveChangesAsync();

        return (categoria.CategoriaId, marca.MarcaId);
    }

    [Fact]
    public async Task ObtenerTodos_CuandoNoHayProductos_RetornaListaVacia()
    {
        var resultado = await _service.ObtenerTodos();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ObtenerTodos_CuandoExistenProductos_RetornaTodosMapeados()
    {
        var (categoriaId, marcaId) = await SembrarCatalogosAsync();

        Context.Productos.AddRange(
            new Producto
            {
                CategoriaId = categoriaId,
                MarcaId = marcaId,
                Sku = "SKU-001",
                Nombre = "Monitor 24\"",
                Descripcion = "Monitor IPS Full HD",
                Costo = 5000m,
                Precio = 7500m,
                Stock = 10,
                PuntoReorden = 3,
                Garantia = "1 año",
                MargenGanancia = 50.00m,
                Itbis = 1350m
            },
            new Producto
            {
                CategoriaId = categoriaId,
                MarcaId = marcaId,
                Sku = "SKU-002",
                Nombre = "Teclado Mecánico",
                Costo = 1500m,
                Precio = 2400m,
                Stock = 20
            }
        );
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerTodos();

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, p => p.Sku == "SKU-001" && p.Precio == 7500m);
        Assert.Contains(resultado, p => p.Sku == "SKU-002" && p.Stock == 20);
    }

    [Fact]
    public async Task ObtenerPorId_IdExistente_RetornaProductoDtoCompleto()
    {
        var (categoriaId, marcaId) = await SembrarCatalogosAsync();

        var entidad = new Producto
        {
            CategoriaId = categoriaId,
            MarcaId = marcaId,
            Sku = "SKU-100",
            Nombre = "Laptop Pro",
            Descripcion = "16GB RAM, 512GB SSD",
            Costo = 35000m,
            Precio = 48000m,
            Stock = 5,
            PuntoReorden = 2,
            Garantia = "2 años",
            MargenGanancia = 37.14m,
            Itbis = 8640m
        };
        Context.Productos.Add(entidad);
        await Context.SaveChangesAsync();

        var resultado = await _service.ObtenerPorId(entidad.ProductoId);

        Assert.NotNull(resultado);
        Assert.Equal(entidad.ProductoId, resultado!.ProductoId);
        Assert.Equal(categoriaId, resultado.CategoriaId);
        Assert.Equal(marcaId, resultado.MarcaId);
        Assert.Equal("SKU-100", resultado.Sku);
        Assert.Equal("Laptop Pro", resultado.Nombre);
        Assert.Equal("16GB RAM, 512GB SSD", resultado.Descripcion);
        Assert.Equal(35000m, resultado.Costo);
        Assert.Equal(48000m, resultado.Precio);
        Assert.Equal(5, resultado.Stock);
        Assert.Equal(2, resultado.PuntoReorden);
        Assert.Equal("2 años", resultado.Garantia);
        Assert.Equal(37.14m, resultado.MargenGanancia);
        Assert.Equal(8640m, resultado.Itbis);
    }

    [Fact]
    public async Task ObtenerPorId_IdInexistente_RetornaNull()
    {
        var resultado = await _service.ObtenerPorId(999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Crear_ProductoValido_GuardaEnBaseYAsignaId()
    {
        var (categoriaId, marcaId) = await SembrarCatalogosAsync();
        var dto = new ProductoDto
        {
            CategoriaId = categoriaId,
            MarcaId = marcaId,
            Sku = "SKU-200",
            Nombre = "Mouse Inalámbrico",
            Descripcion = "Ergonómico",
            Costo = 400m,
            Precio = 750m,
            Stock = 30,
            PuntoReorden = 5,
            Garantia = "6 meses",
            MargenGanancia = 87.50m,
            Itbis = 135m
        };

        var resultado = await _service.Crear(dto);

        Assert.True(resultado.ProductoId > 0);
        Assert.Equal("SKU-200", resultado.Sku);
        Assert.Equal(1, await Context.Productos.CountAsync());
    }

    [Fact]
    public async Task Crear_CategoriaOMarcaInexistente_LanzaDbUpdateException()
    {
        var dto = new ProductoDto
        {
            CategoriaId = 9999,
            MarcaId = 9999,
            Nombre = "Producto Inválido",
            Costo = 100m,
            Precio = 150m
        };

        await Assert.ThrowsAsync<DbUpdateException>(() => _service.Crear(dto));
    }

    [Fact]
    public async Task Actualizar_IdExistente_ActualizaCamposYRetornaTrue()
    {
        var (categoriaId, marcaId) = await SembrarCatalogosAsync();
        var nuevaCategoria = new Categoria { Nombre = "Accesorios" };
        var nuevaMarca = new Marca { Nombre = "Logitech" };
        Context.Categorias.Add(nuevaCategoria);
        Context.Marcas.Add(nuevaMarca);
        await Context.SaveChangesAsync();

        var entidad = new Producto
        {
            CategoriaId = categoriaId,
            MarcaId = marcaId,
            Sku = "OLD-SKU",
            Nombre = "Nombre Viejo",
            Descripcion = "Desc vieja",
            Costo = 100m,
            Precio = 150m,
            Stock = 1,
            PuntoReorden = 1,
            Garantia = "3 meses",
            MargenGanancia = 50m,
            Itbis = 27m
        };
        Context.Productos.Add(entidad);
        await Context.SaveChangesAsync();

        var dtoActualizado = new ProductoDto
        {
            ProductoId = entidad.ProductoId,
            CategoriaId = nuevaCategoria.CategoriaId,
            MarcaId = nuevaMarca.MarcaId,
            Sku = "NEW-SKU",
            Nombre = "Nombre Nuevo",
            Descripcion = "Desc nueva",
            Costo = 200m,
            Precio = 320m,
            Stock = 15,
            PuntoReorden = 4,
            Garantia = "1 año",
            MargenGanancia = 60m,
            Itbis = 57.6m
        };

        var actualizado = await _service.Actualizar(entidad.ProductoId, dtoActualizado);
        var enDb = await Context.Productos.FindAsync(entidad.ProductoId);

        Assert.True(actualizado);
        Assert.NotNull(enDb);
        Assert.Equal(nuevaCategoria.CategoriaId, enDb!.CategoriaId);
        Assert.Equal(nuevaMarca.MarcaId, enDb.MarcaId);
        Assert.Equal("NEW-SKU", enDb.Sku);
        Assert.Equal("Nombre Nuevo", enDb.Nombre);
        Assert.Equal("Desc nueva", enDb.Descripcion);
        Assert.Equal(200m, enDb.Costo);
        Assert.Equal(320m, enDb.Precio);
        Assert.Equal(15, enDb.Stock);
        Assert.Equal(4, enDb.PuntoReorden);
        Assert.Equal("1 año", enDb.Garantia);
        Assert.Equal(60m, enDb.MargenGanancia);
        Assert.Equal(57.6m, enDb.Itbis);
    }

    [Fact]
    public async Task Actualizar_IdInexistente_RetornaFalse()
    {
        var dto = new ProductoDto { Nombre = "Inexistente", Costo = 10m, Precio = 20m };

        var actualizado = await _service.Actualizar(999, dto);

        Assert.False(actualizado);
    }

    [Fact]
    public async Task Eliminar_IdExistente_EliminaRegistroYRetornaTrue()
    {
        var (categoriaId, marcaId) = await SembrarCatalogosAsync();
        var entidad = new Producto
        {
            CategoriaId = categoriaId,
            MarcaId = marcaId,
            Nombre = "Temporal",
            Costo = 100m,
            Precio = 150m
        };
        Context.Productos.Add(entidad);
        await Context.SaveChangesAsync();

        var eliminado = await _service.Eliminar(entidad.ProductoId);

        Assert.True(eliminado);
        Assert.Equal(0, await Context.Productos.CountAsync());
    }

    [Fact]
    public async Task Eliminar_IdInexistente_RetornaFalse()
    {
        var eliminado = await _service.Eliminar(999);

        Assert.False(eliminado);
    }
}
