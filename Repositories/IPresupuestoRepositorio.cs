using presupuesto_api.Models;

namespace presupuesto_api.Repositories;
public interface IPresupuestoRepositorio
{
    Task<Presupuesto> BuscarPorIdAsync(int id);
    Task<IEnumerable<Presupuesto>> BuscarPorIdUsuarioAsync(int idUsuario);
    Task<IEnumerable<Presupuesto>> BuscarActivosPorIdUsuarioAsync(int idUsuario);
    Task<IEnumerable<Presupuesto>> BuscarInactivosPorIdUsuarioAsync(int idUsuario);
    Task CrearAsync(Presupuesto presupuesto);
    Task ActualizarAsync(Presupuesto presupuesto);
    Task EliminarAsync(Presupuesto presupuesto);
}
