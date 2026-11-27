// Service to call the BonaoSells API from Blazor
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using BonaoSells.Application.DTOs;

namespace BonaoSells.Blazor.Components.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private const string BasePath = "api"; // API root

    public ApiService(HttpClient http)
    {
        _http = http;
    }

    // Categorias
    public Task<List<CategoriaDto>> GetCategoriasAsync() =>
        _http.GetFromJsonAsync<List<CategoriaDto>>($"{BasePath}/Categorias");

    public Task<CategoriaDto?> GetCategoriaAsync(int id) =>
        _http.GetFromJsonAsync<CategoriaDto>($"{BasePath}/Categorias/{id}");

    public Task<HttpResponseMessage> CreateCategoriaAsync(CategoriaDto dto) =>
        _http.PostAsJsonAsync($"{BasePath}/Categorias", dto);

    public Task<HttpResponseMessage> UpdateCategoriaAsync(int id, CategoriaDto dto) =>
        _http.PutAsJsonAsync($"{BasePath}/Categorias/{id}", dto);

    public Task<HttpResponseMessage> DeleteCategoriaAsync(int id) =>
        _http.DeleteAsync($"{BasePath}/Categorias/{id}");

    // Productos (similar pattern, add as needed)
    public Task<List<ProductoDto>> GetProductosAsync() =>
        _http.GetFromJsonAsync<List<ProductoDto>>($"{BasePath}/Productos");

    public Task<ProductoDto?> GetProductoAsync(int id) =>
        _http.GetFromJsonAsync<ProductoDto>($"{BasePath}/Productos/{id}");

    // Add other entity methods (Clientes, Ventas, etc.) as required
}
