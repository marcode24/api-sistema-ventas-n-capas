using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utility;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class VentaController : ControllerBase
  {
    private readonly IVentaService _ventaService;

    public VentaController(IVentaService ventaService)
    {
      _ventaService = ventaService;
    }

    [HttpGet]
    [Route("Registrar")]
    public async Task<IActionResult> Registrar([FromBody] VentaDTO venta)
    {
      var response = new Response<VentaDTO>();
      try
      {
        response.status = true;
        response.value = await _ventaService.Registrar(venta);
        response.message = response.status ? "Venta registrada" : "Error al registrar venta";
      }
      catch (Exception ex)
      {
        response.status = false;
        response.message = ex.Message;
      }
      return Ok(response);
    }

    [HttpGet]
    [Route("Historial")]
    public async Task<IActionResult> Historial(string buscarPor, string? numeroVenta, string? fechaInicio, string? fechaFin)
    {
      var response = new Response<List<VentaDTO>>();
      numeroVenta ??= "";
      fechaInicio ??= "";
      fechaFin ??= "";

      try
      {
        response.status = true;
        response.value = await _ventaService.Historial(buscarPor, numeroVenta, fechaInicio, fechaFin);
        response.message = "Historial de ventas";
      }
      catch (Exception ex)
      {
        response.status = false;
        response.message = ex.Message;
      }
      return Ok(response);
    }

    [HttpGet]
    [Route("Reporte")]
    public async Task<IActionResult> Reporte(string? fechaInicio, string? fechaFin)
    {
      var response = new Response<List<ReporteDTO>>();
      fechaInicio ??= "";
      fechaFin ??= "";

      try
      {
        response.status = true;
        response.value = await _ventaService.Reporte(fechaInicio, fechaFin);
        response.message = "Reporte de ventas";
      }
      catch (Exception ex)
      {
        response.status = false;
        response.message = ex.Message;
      }
      return Ok(response);
    }
  }
}
