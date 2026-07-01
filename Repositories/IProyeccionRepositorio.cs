using presupuesto_api.Models;

namespace presupuesto_api.Repositories;

    public interface IProyeccionRepositorio
    {
       Task<IEnumerable<Proyeccion>> BuscarPorPresupuestoIdAsync(int presupuestoId);
        
        Task<Proyeccion?> BuscarPorIdAsync(int id);
        Task AgregarAsync(Proyeccion proyeccion);
        Task ActualizarAsync(Proyeccion proyeccion);
        Task EliminarAsync(Proyeccion proyeccion);
        
    }
