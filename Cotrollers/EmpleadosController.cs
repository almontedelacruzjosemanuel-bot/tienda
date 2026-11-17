
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using tienda.Models;

//namespace tienda.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class EmpleadosController : ControllerBase
//{
//    private readonly TiendaContext _context;

//    public EmpleadosController(TiendaContext context)
//    {
//        _context = context;
//    }

//    [HttpGet]
//    public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
//    {
//        return await _context.Empleados.ToListAsync();
//    }

//    [HttpGet("{id}")]
//    public async Task<ActionResult<Empleado>> GetEmpleado(int id)
//    {
//        var empleado = await _context.Empleados.FindAsync(id);

//        if (empleado == null)
//            return NotFound();

//        return empleado;
//    }

//    [HttpPost]
//    public async Task<ActionResult<Empleado>> CrearEmpleado(Empleado empleado)
//    {
//        _context.Empleados.Add(empleado);
//        await _context.SaveChangesAsync();

//        return CreatedAtAction(nameof(GetEmpleado),
//            new { id = empleado.IdEmpleado }, empleado);
//    }

//    [HttpPut("{id}")]
//    public async Task<IActionResult> ActualizarEmpleado(int id, Empleado empleado)
//    {
//        if (id != empleado.IdEmpleado)
//            return BadRequest();

//        var existente = await _context.Empleados.FindAsync(id);

//        if (existente == null)
//            return NotFound();

//        existente.Nombre = empleado.Nombre;
//        existente.Apellido = empleado.Apellido;
//        existente.Cargo = empleado.Cargo;
//        existente.Email = empleado.Email;
//        existente.Telefono = empleado.Telefono;
//        existente.FechaContratacion = empleado.FechaContratacion;

//        await _context.SaveChangesAsync();

//        return NoContent();
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> EliminarEmpleado(int id)
//    {
//        var empleado = await _context.Empleados.FindAsync(id);

//        if (empleado == null)
//            return NotFound();

//        _context.Empleados.Remove(empleado);
//        await _context.SaveChangesAsync();

//        return NoContent();
//    }
//}
