namespace presupuesto_api.Models.DTOs;


public class UsuarioDto
{
    public int Id { get; set; }
    public string? Nickname { get; set; }
    public string? Correo { get; set; }
    public string? Rol { get; set; }
    public string? Contraseña { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaModificacion { get; set; }
}