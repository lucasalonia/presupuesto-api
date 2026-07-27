using presupuesto_api.Models;
using presupuesto_api.Repositories;
using Microsoft.AspNetCore.Mvc;
using presupuesto_api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

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
    [Authorize]
    public async Task<IActionResult> ListarCategoria()
    {
        var idClaim = User.FindFirst("idUsuario")?.Value;
        if (idClaim == null)
            return Unauthorized();

        var id = int.Parse(idClaim);
        var categorias = await _categoriaRepositorio.BuscarPorUsuarioIdAsync(id);

        return Ok(categorias ?? new List<Categoria>());
    }
    
    [HttpPost("crear")]
    [Authorize]
    public async Task<IActionResult> CrearCategoria([FromBody] CategoriaDto categoriaDto)
    {
        
        var idClaim = User.FindFirst("idUsuario")?.Value;
         if (idClaim == null)
            return Unauthorized();
            
        var id = int.Parse(idClaim);
        
        var categoria = new Categoria
        {
            Nombre = categoriaDto.Nombre?.Trim(),
            Descripcion = categoriaDto.Descripcion?.Trim(),
            IdUsuario = id,
            FechaCreacion = DateTime.UtcNow
        };
        
       

        if (categoria == null || string.IsNullOrEmpty(categoria.Nombre))
            return BadRequest( "\"El nombre de la categoría es obligatorio.\"" );

        if (categoria.Nombre.Length > 100)
            return BadRequest( "\"El nombre de la categoría no puede exceder los 100 caracteres.\"" );
        if (categoria.Descripcion != null && categoria.Descripcion.Length > 500)
            return BadRequest( "\"La descripción de la categoría no puede exceder los 500 caracteres.\"" );

       
        await _categoriaRepositorio.AgregarAsync(categoria);
        return Ok("\"Categoría creada correctamente.\"");
    }
    [HttpGet("mostrar/{id}")]
    [Authorize]
    public async Task<IActionResult> MostrarCategoria(int id)
    {
        var categoria = await _categoriaRepositorio.BuscarPorIdAsync(id);
        if (categoria == null)
            return NotFound( "\"Categoría no encontrada.\"" );

        return Ok(categoria);
    }
    [HttpPatch("actualizar/{id}")]
    [Authorize]
    public async Task<IActionResult> ActualizarCategoria(int id, [FromBody] CategoriaDto categoriaDto)
    {

        var idClaim = User.FindFirst("idUsuario")?.Value;
        if (idClaim == null)
            return Unauthorized();

        var categoriaExistente = await _categoriaRepositorio.BuscarPorIdAsync(id);

        if (categoriaExistente == null)
            return NotFound( "\"Categoría no encontrada.\"" );

        if (string.IsNullOrWhiteSpace(categoriaDto.Nombre) && string.IsNullOrWhiteSpace(categoriaDto.Descripcion))
        {
            return BadRequest( "\"Algun campo debe ser modificado.\"" );
        }
        
        if (!string.IsNullOrWhiteSpace(categoriaDto.Nombre))
        {
            categoriaExistente.Nombre = categoriaDto.Nombre.Trim();
            categoriaExistente.FechaModificacion = DateTime.UtcNow;
        }
        if (!string.IsNullOrWhiteSpace(categoriaDto.Descripcion))
        {
            categoriaExistente.Descripcion = categoriaDto.Descripcion.Trim();
            categoriaExistente.FechaModificacion = DateTime.UtcNow;
        }
        

        
        await _categoriaRepositorio.ActualizarAsync(categoriaExistente);
        return Ok("\"Categoría actualizada correctamente.\"");
        
    }

    [HttpDelete("eliminar/{id}")]
    [Authorize]
    public async Task<IActionResult> EliminarCategoria(int id)
    {
        var categoria = await _categoriaRepositorio.BuscarPorIdAsync(id);
        if (categoria == null)
            return NotFound( "\"Categoría no encontrada.\"" );

        await _categoriaRepositorio.EliminarAsync(categoria);
        return Ok( "\"Categoría eliminada correctamente.\"" );
    }

}