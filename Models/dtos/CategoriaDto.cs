namespace presupuesto_api.Models.DTOs;

public class CategoriaDto
{
    public int Id { get; set; }
    public int IdUsuario { get; set; }
    public string? Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    
}