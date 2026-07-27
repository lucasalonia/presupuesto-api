using Microsoft.EntityFrameworkCore;
using presupuesto_api.Models; 
using presupuesto_api.Data; 

namespace presupuesto_api.Repositories;

public class ProyeccionRepositorio : IProyeccionRepositorio
{
    private readonly DataContext _context;

    public ProyeccionRepositorio(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Proyeccion>> BuscarPorPresupuestoIdAsync(int presupuestoId)
    {
        return await _context.Proyecciones
            .Include(p => p.Categoria)
            .Where(p => p.IdPresupuesto == presupuestoId)
            .ToListAsync();
    }

    public async Task<Proyeccion?> BuscarPorIdAsync(int id)
    {
        return await _context.Proyecciones
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AgregarAsync(Proyeccion proyeccion)
    {
        await _context.Proyecciones.AddAsync(proyeccion);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Proyeccion proyeccion)
    {
        _context.Proyecciones.Update(proyeccion);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Proyeccion proyeccion)
    {
        _context.Proyecciones.Remove(proyeccion);
        await _context.SaveChangesAsync();
    }
}