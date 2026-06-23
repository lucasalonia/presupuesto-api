using presupuesto_api.Models;

namespace presupuesto_api.Repositories;

    public interface ICategoriaRepositorio
    {
       
        Task<IEnumerable<Categoria>> BuscarPorUsuarioIdAsync(int usuarioId);
        
        Task<Categoria?> BuscarPorIdAsync(int id);
        Task AgregarAsync(Categoria categoria);
        Task ActualizarAsync(Categoria categoria);
        Task EliminarAsync(Categoria categoria);
    }
