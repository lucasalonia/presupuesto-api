using presupuesto_api.Models;
using presupuesto_api.Repositories;
using Microsoft.AspNetCore.Mvc;
using presupuesto_api.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace presupuesto_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportesController : ControllerBase
{
    private readonly IProyeccionRepositorio _proyeccionRepositorio;
    private readonly IPresupuestoRepositorio _presupuestoRepositorio;
    private readonly ICategoriaRepositorio _categoriaRepositorio;

    public ReportesController(IProyeccionRepositorio proyeccionRepositorio, 
                                    IPresupuestoRepositorio presupuestoRepositorio, 
                                    ICategoriaRepositorio categoriaRepositorio)
    {
        _proyeccionRepositorio = proyeccionRepositorio;
        _presupuestoRepositorio = presupuestoRepositorio;
        _categoriaRepositorio = categoriaRepositorio;
    }
[HttpGet("ahorro/{presupuestoId}")]
[Authorize]
public async Task<IActionResult> GenerarReporteAhorro(int presupuestoId)
{
    var presupuesto = await _presupuestoRepositorio.BuscarPorIdAsync(presupuestoId);
    if (presupuesto == null)
        return NotFound(new { mensaje = "Presupuesto no encontrado." });

    var proyecciones = await _proyeccionRepositorio.BuscarPorPresupuestoIdAsync(presupuestoId) ?? new List<Proyeccion>();

    decimal totalIngresos = proyecciones.Where(p => p.Tipo).Sum(p => p.Monto);
    decimal totalGastos = proyecciones.Where(p => !p.Tipo).Sum(p => p.Monto);

    decimal balance = totalIngresos - totalGastos;
    decimal porcentajeGasto = totalIngresos == 0 ? 0 : Math.Round((totalGastos / totalIngresos) * 100, 2);
    decimal porcentajeAhorro = totalIngresos == 0 ? 0 : Math.Round((balance / totalIngresos) * 100, 2);
    bool tieneAhorro = balance >= 0;
    if(porcentajeGasto < 0)
        porcentajeGasto = 0;


    var reporte = new ReporteDto
    {
        TotalIngresos = totalIngresos,
        TotalGastos = totalGastos,
        Balance = balance,
        PorcentajeGasto = porcentajeGasto,
        PorcentajeAhorro = porcentajeAhorro,
        TieneAhorro = tieneAhorro
    };

    return Ok(reporte);
}
}