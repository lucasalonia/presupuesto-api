namespace presupuesto_api.Models.DTOs;

public class ReporteDto
{
    public decimal TotalIngresos { get; set; }
    public decimal TotalGastos { get; set; }
    public decimal Balance { get; set; }
    public bool TieneAhorro { get; set; }
    public decimal PorcentajeAhorro { get; set; }
    public decimal PorcentajeGasto { get; set; }
}