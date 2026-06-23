namespace presupuesto_api.Services;
public interface IEncriptadorService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}