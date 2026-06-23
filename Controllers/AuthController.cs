using Microsoft.AspNetCore.Mvc;
using presupuesto_api.Repositories;
using presupuesto_api.Services;
using presupuesto_api.Models.DTOs;

namespace presupuesto_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IEncriptadorService _encriptadorService;
    private readonly ITokenService _tokenService;

    public AuthController(
        IUsuarioRepositorio usuarioRepositorio,
        IEncriptadorService encriptadorService,
        ITokenService tokenService)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _encriptadorService = encriptadorService;
        _tokenService = tokenService;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginSolicitudDto request)
    {
        var usuario = await _usuarioRepositorio.BuscarPorCorreoAsync(request.Correo);
    
        if (usuario == null)
            return Unauthorized(new { Mensaje = "Usuario o contraseña incorrectos" });

        bool ContraseñaValida = _encriptadorService.VerifyPassword(request.Contraseña, usuario.Contraseña);

        if (!ContraseñaValida)
            return Unauthorized(new { Mensaje = "Usuario o contraseña incorrectos" });

        var token = _tokenService.GenerarTokenJWT(usuario);
        return Ok(new { Token = token });
    }

}