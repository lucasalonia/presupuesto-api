using presupuesto_api.Data;
using presupuesto_api.Models;
using Microsoft.EntityFrameworkCore;
using presupuesto_api.Models.DTOs;
namespace presupuesto_api.Repositories;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly DataContext _context;

    public UsuarioRepositorio(DataContext context)
    {
        _context = context;
    }

    public async Task<Usuario> BuscarPorCorreoAsync(string correo)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
    }

    public async Task<UsuarioDto> BuscarUsuarioDtoPorIdAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return null;

        return new UsuarioDto
        {
            Id = usuario.Id,
            Nickname = usuario.Nickname,
            Correo = usuario.Correo
        };
    }

    public async Task<Usuario> BuscarPorIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task CrearAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Usuario usuario)
    {
        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
    }
}