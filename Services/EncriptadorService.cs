using BCryptNet = BCrypt.Net.BCrypt;

namespace presupuesto_api.Services;

public class EncriptadorService : IEncriptadorService
{
    public string HashPassword(string password)
    {
        return BCryptNet.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCryptNet.Verify(password, hashedPassword);
    }
}