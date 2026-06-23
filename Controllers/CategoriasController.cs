using presupuesto_api.Models;
using presupuesto_api.Repositories;
using presupuesto_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace presupuesto_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaRepositorio _categoriaRepositorio;


    public CategoriasController(ICategoriaRepositorio categoriaRepositorio)
    {
        _categoriaRepositorio = categoriaRepositorio;
    }

    [HttpGet("listar")]
    public async Task<IActionResult> ListarCategoria()
    {
        var idClaim = User.FindFirst("idUsuario")?.Value;
        if (idClaim == null)
            return Unauthorized();

        var id = int.Parse(idClaim);
        var categorias = await _categoriaRepositorio.BuscarPorUsuarioIdAsync(id);

        if (categorias == null || !categorias.Any())
            return NotFound(new { mensaje = "No se encontraron categorías para este usuario." });

        return Ok(categorias);
    }
    [HttpPost("crear")]
    public async Task<IActionResult> CrearCategoria([FromBody] Categoria categoria)
    {
        var idClaim = User.FindFirst("idUsuario")?.Value;
        if (idClaim == null)
            return Unauthorized();

        if(categoria == null || string.IsNullOrEmpty(categoria.Nombre))
            return BadRequest(new { mensaje = "El nombre de la categoría es obligatorio." });

        var id = int.Parse(idClaim);
        categoria.IdUsuario = id;

        await _categoriaRepositorio.AgregarAsync(categoria);
        return Ok(new { mensaje = "Categoría creada correctamente." });
    }

}