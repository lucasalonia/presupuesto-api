using Microsoft.EntityFrameworkCore;
using presupuesto_api.Models; 
using presupuesto_api.Data; 

namespace presupuesto_api.Repositories;

public class PresupuestoRepositorio : IPresupuestoRepositorio
{
    private readonly DataContext _context;

    public PresupuestoRepositorio(DataContext context)
    {
        _context = context;
    }

    public async Task<Presupuesto> BuscarPorIdAsync(int id)
    {
        return await _context.Presupuestos.FindAsync(id);
    }

    public async Task<IEnumerable<Presupuesto>> BuscarPorIdUsuarioAsync(int usuarioId)
    {
           return await _context.Presupuestos
            .Where(p => p.IdUsuario == usuarioId)
            .ToListAsync();
    }

    public async Task CrearAsync(Presupuesto presupuesto)
    {
        await _context.Presupuestos.AddAsync(presupuesto);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Presupuesto presupuesto)
    {
        _context.Presupuestos.Update(presupuesto);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Presupuesto presupuesto)
    {
        _context.Presupuestos.Remove(presupuesto);
        await _context.SaveChangesAsync();
    }
}