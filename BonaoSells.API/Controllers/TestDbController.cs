
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using tienda.Models;

//namespace tienda.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class TestDbController : ControllerBase
//{
//    private readonly BonaoSellsContext _context;

//    public TestDbController(BonaoSellsContext context)
//    {
//        _context = context;
//    }

//    [HttpGet]
//    public async Task<IActionResult> Get()
//    {
//        try
//        {
//            var conectado = await _context.Database.CanConnectAsync();

//            if (conectado)
//                return Ok("Conexión exitosa con gestion_inventario");

//            return StatusCode(500, "No se pudo conectar con la base de datos");
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, ex.Message);
//        }
//    }
//}
