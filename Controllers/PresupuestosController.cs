using presupuesto_api.Models;
using presupuesto_api.Repositories;
using Microsoft.AspNetCore.Mvc;
using presupuesto_api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace presupuesto_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PresupuestosController : ControllerBase
{
    private readonly IPresupuestoRepositorio _presupuestoRepositorio;


    public PresupuestosController(IPresupuestoRepositorio presupuestoRepositorio)
    {
        _presupuestoRepositorio = presupuestoRepositorio;
    }

   [HttpGet("mostrar/{id}")]
   [Authorize]
    public async Task<IActionResult> ObtenerPresupuesto(int id)
    {
        var presupuesto = await _presupuestoRepositorio.BuscarPorIdAsync(id);
        if (presupuesto == null)
            return NotFound(new { mensaje = "Presupuesto no encontrado." });

        var presupuestoDto = new PresupuestoDto
        {
            Id = presupuesto.Id,
            IdUsuario = presupuesto.IdUsuario,
            Nombre = presupuesto.Nombre,
            Descripcion = presupuesto.Descripcion,
            Estado = presupuesto.Estado,
            FechaInicio = presupuesto.FechaInicio,
            FechaFin = presupuesto.FechaFin,
            FechaCreacion = presupuesto.FechaCreacion,
            FechaModificacion = presupuesto.FechaModificacion
        };
        return Ok(presupuestoDto);
    } 

    [HttpGet("listar")]
    [Authorize]
    public async Task<IActionResult> ListarPresupuestos()
    {
        var idClaim = User.FindFirst("idUsuario")?.Value;
        if (idClaim == null)
            return Unauthorized();

        var presupuestos = await _presupuestoRepositorio.BuscarPorIdUsuarioAsync(int.Parse(idClaim));
        if (presupuestos == null || !presupuestos.Any())
            return NotFound(new { mensaje = "No se encontraron presupuestos para este usuario." });
        return Ok(presupuestos);
    }

    [HttpPost("crear")]
    [Authorize]
    public async Task<IActionResult> CrearPresupuesto([FromBody] PresupuestoDto presupuestoDto)
    {
        var idClaim = User.FindFirst("idUsuario")?.Value;
        if (idClaim == null)
            return Unauthorized();

        if(presupuestoDto ==null)
            return BadRequest(new { mensaje = "Los datos del presupuesto son obligatorios." });

        if(string.IsNullOrWhiteSpace(presupuestoDto.Nombre))
            return BadRequest(new { mensaje = "El nombre del presupuesto es obligatorio." });
        if(presupuestoDto.Nombre.Length > 100)
            return BadRequest(new { mensaje = "El nombre del presupuesto no puede exceder los 100 caracteres." });
        if(presupuestoDto.Descripcion != null && presupuestoDto.Descripcion.Length > 500)
            return BadRequest(new { mensaje = "La descripción del presupuesto no puede exceder los 500 caracteres." });
        
        var presupuesto = new Presupuesto
        {
            IdUsuario = int.Parse(idClaim),
            Nombre = presupuestoDto.Nombre,
            Descripcion = presupuestoDto.Descripcion,
            Estado = true,
            FechaInicio = presupuestoDto.FechaInicio,
            FechaFin = presupuestoDto.FechaFin,
            FechaCreacion = DateTime.UtcNow
        };
        await _presupuestoRepositorio.CrearAsync(presupuesto);
        return CreatedAtAction(nameof(ObtenerPresupuesto), new { id = presupuesto.Id }, presupuesto);
    }

    [HttpPatch("actualizar-estado/{id}")]
    [Authorize]
    public async Task<IActionResult> ActualizarEstadoPresupuesto(int id, [FromBody] bool estado)
    {
        var idClaim = User.FindFirst("idUsuario")?.Value;
        if (idClaim == null)
            return Unauthorized();

        var presupuesto = await _presupuestoRepositorio.BuscarPorIdAsync(id);
        if (presupuesto == null)
            return NotFound(new { mensaje = "Presupuesto no encontrado." });

        presupuesto.Estado = estado;
        await _presupuestoRepositorio.ActualizarAsync(presupuesto);

        var presupuestoDto = new PresupuestoDto
        {
            Id = presupuesto.Id,
            IdUsuario = presupuesto.IdUsuario,
            Nombre = presupuesto.Nombre,
            Descripcion = presupuesto.Descripcion,
            Estado = presupuesto.Estado,
            FechaInicio = presupuesto.FechaInicio,
            FechaFin = presupuesto.FechaFin,
            FechaCreacion = presupuesto.FechaCreacion,
            FechaModificacion = presupuesto.FechaModificacion
        };
        return Ok(presupuestoDto);
    }

    [HttpPatch("actualizar/{id}")]
    [Authorize]
    public async Task<IActionResult> ActualizarPresupuesto(int id, [FromBody] PresupuestoDto presupuestoDto)
    {
        var idClaim = User.FindFirst("idUsuario")?.Value;
        if (idClaim == null)
            return Unauthorized();

        var presupuesto = await _presupuestoRepositorio.BuscarPorIdAsync(id);
        if (presupuesto == null)
            return NotFound(new { mensaje = "Presupuesto no encontrado." });

        
        if (presupuestoDto.Nombre.Length > 100)
            return BadRequest(new { mensaje = "No puede exceder los 100 caracteres." });

        if (string.IsNullOrWhiteSpace(presupuestoDto.Nombre))
        {
            presupuestoDto.Nombre = presupuesto.Nombre; 
        }
        if (presupuestoDto.Descripcion != null && presupuestoDto.Descripcion.Length > 500)
            return BadRequest(new { mensaje = "La descripción del presupuesto no puede exceder los 500 caracteres." });
        if (string.IsNullOrWhiteSpace(presupuestoDto.Descripcion))
        {
            presupuestoDto.Descripcion = presupuesto.Descripcion; 
        }

        presupuesto.Nombre = presupuestoDto.Nombre;
        presupuesto.Descripcion = presupuestoDto.Descripcion;
        presupuesto.FechaInicio = presupuestoDto.FechaInicio;
        presupuesto.FechaFin = presupuestoDto.FechaFin;
        presupuesto.FechaModificacion = DateTime.UtcNow;
        await _presupuestoRepositorio.ActualizarAsync(presupuesto);
        return Ok(presupuestoDto);
    }

    [HttpDelete("eliminar/{id}")]
    [Authorize]
    public async Task<IActionResult> EliminarPresupuesto(int id)
    {
        var idClaim = User.FindFirst("idUsuario")?.Value;
        if (idClaim == null)
            return Unauthorized();

        var presupuesto = await _presupuestoRepositorio.BuscarPorIdAsync(id);
        if (presupuesto == null)
            return NotFound(new { mensaje = "Presupuesto no encontrado." });
        
        await _presupuestoRepositorio.EliminarAsync(presupuesto);
        return Ok(new { mensaje = "Presupuesto eliminado correctamente." });
    }

}