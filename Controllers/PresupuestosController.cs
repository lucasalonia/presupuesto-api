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
    return Ok(MapearAPresupuestoDto(presupuestos ?? new List<Presupuesto>()));
}

[HttpGet("listar-activos")]
[Authorize]
public async Task<IActionResult> ListarPresupuestosActivos()
{
    var idClaim = User.FindFirst("idUsuario")?.Value;
    if (idClaim == null)
        return Unauthorized();

    var presupuestos = await _presupuestoRepositorio.BuscarActivosPorIdUsuarioAsync(int.Parse(idClaim));
    return Ok(MapearAPresupuestoDto(presupuestos ?? new List<Presupuesto>()));
}

[HttpGet("listar-inactivos")]
[Authorize]
public async Task<IActionResult> ListarPresupuestosInactivos()
{
    var idClaim = User.FindFirst("idUsuario")?.Value;
    if (idClaim == null)
        return Unauthorized();

    var presupuestos = await _presupuestoRepositorio.BuscarInactivosPorIdUsuarioAsync(int.Parse(idClaim));
    return Ok(MapearAPresupuestoDto(presupuestos ?? new List<Presupuesto>()));
}

private IEnumerable<PresupuestoDto> MapearAPresupuestoDto(IEnumerable<Presupuesto> presupuestos)
{
    return presupuestos.Select(p => new PresupuestoDto
    {
        Id = p.Id,
        IdUsuario = p.IdUsuario,
        Nombre = p.Nombre,
        Descripcion = p.Descripcion,
        Estado = p.Estado,
        FechaInicio = p.FechaInicio,
        FechaFin = p.FechaFin,
        FechaCreacion = p.FechaCreacion,
        FechaModificacion = p.FechaModificacion,
        Proyecciones = p.Proyecciones?.Select(pr => new ProyeccionDto
        {
            Id = pr.Id,
            IdPresupuesto = pr.IdPresupuesto,
            IdCategoria = pr.IdCategoria,
            Tipo = pr.Tipo,
            Nombre = pr.Nombre,
            Descripcion = pr.Descripcion,
            Monto = pr.Monto,
            FechaInicio = pr.FechaCreacion,
            FechaFin = pr.FechaModificacion
        }).ToList() ?? new List<ProyeccionDto>()
    });
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
            return NotFound("\"Presupuesto no encontrado.\"");

        presupuesto.Estado = estado;
        if (estado)
        {
            presupuesto.FechaFin = null;
        }
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
        return Ok("\"Estado del presupuesto actualizado correctamente.\"");
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
            return NotFound("\"Presupuesto no encontrado.\"");

        
        if (presupuestoDto.Nombre.Length > 100)
            return BadRequest("\"El nombre del presupuesto no puede exceder los 100 caracteres.\"");

        if (string.IsNullOrWhiteSpace(presupuestoDto.Nombre))
        {
            presupuestoDto.Nombre = presupuesto.Nombre; 
        }
        if (presupuestoDto.Descripcion != null && presupuestoDto.Descripcion.Length > 500)
            return BadRequest("\"La descripción del presupuesto no puede exceder los 500 caracteres.\"");
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
            return NotFound("\"Presupuesto no encontrado.\"");
        
        await _presupuestoRepositorio.EliminarAsync(presupuesto);
        return Ok("\"Presupuesto eliminado correctamente.\"");
    }

}