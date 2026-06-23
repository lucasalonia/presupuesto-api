using presupuesto_api.Models;
using presupuesto_api.Models.DTOs;

namespace presupuesto_api.Repositories;
public interface IUsuarioRepositorio
{
    Task<Usuario> BuscarPorCorreoAsync(string correo);
    Task<UsuarioDto> BuscarUsuarioDtoPorIdAsync(int id);
    Task<Usuario> BuscarPorIdAsync(int id);

    Task CrearAsync(Usuario usuario);
    Task ActualizarAsync(Usuario usuario);
    Task EliminarAsync(Usuario usuario);
}