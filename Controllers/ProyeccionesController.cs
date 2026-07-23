using presupuesto_api.Models;
using presupuesto_api.Repositories;
using Microsoft.AspNetCore.Mvc;
using presupuesto_api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace presupuesto_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProyeccionesController : ControllerBase
{
    private readonly IProyeccionRepositorio _proyeccionRepositorio;
    private readonly IPresupuestoRepositorio _presupuestoRepositorio;
    private readonly ICategoriaRepositorio _categoriaRepositorio;

    public ProyeccionesController(IProyeccionRepositorio proyeccionRepositorio, 
                                    IPresupuestoRepositorio presupuestoRepositorio, 
                                    ICategoriaRepositorio categoriaRepositorio)
    {
        _proyeccionRepositorio = proyeccionRepositorio;
        _presupuestoRepositorio = presupuestoRepositorio;
        _categoriaRepositorio = categoriaRepositorio;
    }

    [HttpGet("listar/{presupuestoId}")]
    [Authorize]
    public async Task<IActionResult> ListarProyecciones(int presupuestoId)
    {
        
        var proyecciones = await _proyeccionRepositorio.BuscarPorPresupuestoIdAsync(presupuestoId);
        if (proyecciones == null || !proyecciones.Any())
            return NotFound(new { mensaje = "No se encontraron proyecciones para este presupuesto." });
        return Ok(proyecciones);
    }

    [HttpGet("mostrar/{id}")]
    [Authorize]
    public async Task<IActionResult> MostrarProyeccion(int id)
    {
        var proyeccion = await _proyeccionRepositorio.BuscarPorIdAsync(id);
        if (proyeccion == null)
            return NotFound(new { mensaje = "Proyección no encontrada." });
        return Ok(proyeccion);
    }

    [HttpPost("crear")]
    [Authorize]
    public async Task<IActionResult> CrearProyeccion([FromBody] ProyeccionDto proyeccionDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
    
        if(proyeccionDto.IdPresupuesto == 0)
            return BadRequest(new { mensaje = "El ID del presupuesto es obligatorio." });
        var presupuesto = await _presupuestoRepositorio.BuscarPorIdAsync(proyeccionDto.IdPresupuesto);

        if (presupuesto == null)
            return BadRequest(new { mensaje = "El presupuesto especificado no existe." });
        if(proyeccionDto.IdCategoria != null)
        {
            var categoria = await _categoriaRepositorio.BuscarPorIdAsync(proyeccionDto.IdCategoria.Value);
            if (categoria == null)
                return BadRequest(new { mensaje = "La categoría especificada no existe." });
        }
        
        if(string.IsNullOrWhiteSpace(proyeccionDto.Nombre))
            return BadRequest(new { mensaje = "El nombre de la proyección es obligatorio." });
        
        if(proyeccionDto.Nombre.Length > 100)
            return BadRequest(new { mensaje = "El nombre de la proyección no puede exceder los 100 caracteres." });
        if(proyeccionDto.Descripcion != null && proyeccionDto.Descripcion.Length > 500)
            return BadRequest(new { mensaje = "La descripción de la proyección no puede exceder los 500 caracteres." });
        if(proyeccionDto.Monto <= 0)
            return BadRequest(new { mensaje = "El monto de la proyección debe ser un valor positivo y distinto de cero." });

        var proyeccion = new Proyeccion
        {
            IdPresupuesto = proyeccionDto.IdPresupuesto,
            IdCategoria = proyeccionDto.IdCategoria,
            Tipo = proyeccionDto.Tipo,
            Nombre = proyeccionDto.Nombre,
            Descripcion = proyeccionDto.Descripcion,
            Monto = proyeccionDto.Monto,
            FechaCreacion = DateTime.UtcNow
        };

        await _proyeccionRepositorio.AgregarAsync(proyeccion);
        return CreatedAtAction(nameof(MostrarProyeccion), new { id = proyeccion.Id }, proyeccionDto);
    }

    [HttpPatch("actualizar/{id}")]
    [Authorize]
    public async Task<IActionResult> ActualizarProyeccion(int id, [FromBody] ProyeccionDto proyeccionDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var proyeccion = await _proyeccionRepositorio.BuscarPorIdAsync(id);
        if (proyeccion == null)
            return NotFound(new { mensaje = "Proyección no encontrada." });

        if(proyeccionDto.IdCategoria != null)
        {
            var categoria = await _categoriaRepositorio.BuscarPorIdAsync(proyeccionDto.IdCategoria.Value);
            if (categoria == null)
                return BadRequest(new { mensaje = "La categoría especificada no existe." });
        }else
        {
            proyeccionDto.IdCategoria = proyeccion.IdCategoria; 
        }

        if (string.IsNullOrWhiteSpace(proyeccionDto.Nombre))
        {
            proyeccionDto.Nombre = proyeccion.Nombre; 
        }
        if(string.IsNullOrWhiteSpace(proyeccionDto.Descripcion))
        {
            proyeccionDto.Descripcion = proyeccion.Descripcion; 
        }

        if(proyeccionDto.Monto == 0)
        {
            proyeccionDto.Monto = proyeccion.Monto; 
        }
        proyeccion.IdCategoria = proyeccionDto.IdCategoria;
        proyeccion.Tipo = proyeccionDto.Tipo;
        proyeccion.Nombre = proyeccionDto.Nombre;
        proyeccion.Descripcion = proyeccionDto.Descripcion;
        proyeccion.Monto = proyeccionDto.Monto;
        proyeccion.FechaModificacion = DateTime.UtcNow;

        await _proyeccionRepositorio.ActualizarAsync(proyeccion);
        return Ok(proyeccion);
    }
}