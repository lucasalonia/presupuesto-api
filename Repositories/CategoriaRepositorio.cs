using Microsoft.EntityFrameworkCore;
using presupuesto_api.Models; // Ajustá según tu namespace real
using presupuesto_api.Data;  // Ajustá al namespace de tu ApplicationDbContext

namespace presupuesto_api.Repositories;

public class CategoriaRepositorio : ICategoriaRepositorio
{
    private readonly DataContext _context;

    public CategoriaRepositorio(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Categoria>> BuscarPorUsuarioIdAsync(int usuarioId)
    {
        return await _context.Categorias
            .Where(c => c.IdUsuario == usuarioId)
            .ToListAsync();
    }

    public async Task<Categoria?> BuscarPorIdAsync(int id)
    {
        return await _context.Categorias.FindAsync(id);
    }

    public async Task AgregarAsync(Categoria categoria)
    {
        await _context.Categorias.AddAsync(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Categoria categoria)
    {
        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Categoria categoria)
    {
        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
    }
}