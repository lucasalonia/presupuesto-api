using presupuesto_api.Models;
using presupuesto_api.Repositories;
using Microsoft.AspNetCore.Mvc;
using presupuesto_api.Services;
using Microsoft.AspNetCore.Authorization;
using presupuesto_api.Models.DTOs;

namespace presupuesto_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IEncriptadorService _encriptadorService;

    public UsuariosController(IUsuarioRepositorio usuarioRepositorio,
    IEncriptadorService encriptadorService)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _encriptadorService = encriptadorService;
    }


    [HttpPost("registro")]
    public async Task<IActionResult> Registro([FromBody] UsuarioDto request)
    {
        if (string.IsNullOrEmpty(request.Nickname) || string.IsNullOrEmpty(request.Contrasenia) || string.IsNullOrEmpty(request.Correo))
        {
            return BadRequest( "Todos los campos son obligatorios." );
        }
        var usuarioExistente = await _usuarioRepositorio.BuscarPorCorreoAsync(request.Correo);
        if (usuarioExistente != null)
        {
            return BadRequest( "El correo ya está registrado." );
        }

        var usuario = new Usuario
        {
            Nickname = request.Nickname,
            Correo = request.Correo,
            Contraseña = _encriptadorService.HashPassword(request.Contrasenia),
            Rol = "Persona",
            FechaCreacion = DateTime.UtcNow
        };
        await _usuarioRepositorio.CrearAsync(usuario);
        return Ok("\"Usuario creado correctamente.\"" );
    }


    [HttpGet("perfil")]
    [Authorize]
    public async Task<IActionResult> ObtenerPerfil()
    {
        var idClaim = User.FindFirst("idUsuario")?.Value;

        if (idClaim == null)
            return Unauthorized();

        var id = int.Parse(idClaim);
        var usuarioDto = await _usuarioRepositorio.BuscarUsuarioDtoPorIdAsync(id);

        if (usuarioDto == null)
            return NotFound();

        return Ok(usuarioDto);
    }

    [HttpPatch("actualizar")]
    [Authorize]
    public async Task<IActionResult> ActualizarPerfil([FromBody] UsuarioDto usuarioActualizado)
    {
        var idClaim = User.FindFirst("idUsuario")?.Value;

        if (idClaim == null)
            return Unauthorized();

        var id = int.Parse(idClaim);
        var usuario = await _usuarioRepositorio.BuscarPorIdAsync(id);
        if (usuario == null)
            return NotFound();

        if (string.IsNullOrEmpty(usuarioActualizado.Nickname) && string.IsNullOrEmpty(usuarioActualizado.Correo))
        {
            return BadRequest(new { mensaje = "Algun campo debe ser modificado." });
        }
        
        if (!string.IsNullOrWhiteSpace(usuarioActualizado.Correo))
        {
            var usuarioExistente = await _usuarioRepositorio.BuscarPorCorreoAsync(usuarioActualizado.Correo);
            if (usuarioExistente != null && usuarioExistente.Id != id)
            {
                return BadRequest(new { mensaje = "El correo ya está registrado por otro usuario." });
            }
            else
            {
                usuario.Correo = usuarioActualizado.Correo.Trim();
            }
        }

        if (!string.IsNullOrWhiteSpace(usuarioActualizado.Nickname))
        {
            usuario.Nickname = usuarioActualizado.Nickname.Trim();
        }

        usuario.FechaModificacion = DateTime.UtcNow;    
        await _usuarioRepositorio.ActualizarAsync(usuario);
        return Ok(new { mensaje = "Perfil actualizado correctamente." });
    }

    [HttpDelete("eliminar")]
    [Authorize]
    public async Task<IActionResult> EliminarPerfil()
    {
        var idClaim = User.FindFirst("idUsuario")?.Value;

        if (idClaim == null)
            return Unauthorized();

        var id = int.Parse(idClaim);
        var usuario = await _usuarioRepositorio.BuscarPorIdAsync(id);
        if (usuario == null)
            return NotFound();

        await _usuarioRepositorio.EliminarAsync(usuario);
        return Ok(new { mensaje = "Perfil eliminado correctamente." });
    }

}