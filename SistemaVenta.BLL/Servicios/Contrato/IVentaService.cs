using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios.Contrato
{
  public interface IVentaService
  {
    Task<VentaDTO> Registrar(VentaDTO modelo);
    Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin);
    Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin);
  }
}
