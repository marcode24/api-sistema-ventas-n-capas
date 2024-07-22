using SistemaVenta.Model;

namespace SistemaVenta.DAL.Repositorios.Contrato
{
  public interface IVentaRepository : IGenericRepository<Venta>
  {
    Task<Venta> Registrar(Venta modelo);
  }
}
