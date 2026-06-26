
namespace presupuesto_api.Models.DTOs;

public class ProyeccionDto
{
    public int Id { get; set; }
    public int IdPresupuesto { get; set; }
    public PresupuestoDto? Presupuesto { get; set; }
    public int IdCategoria { get; set; }
    public CategoriaDto? Categoria { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}