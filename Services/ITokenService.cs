using presupuesto_api.Models;

namespace presupuesto_api.Services;

public interface ITokenService
{
    string GenerarTokenJWT(Usuario usuario);
}