using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System.Globalization;

namespace SistemaVenta.BLL.Servicios
{
  public class DashBoardService : IDashBoardService
  {
    private readonly IVentaRepository _ventaRepository;
    private readonly IGenericRepository<Producto> _productoRepository;
    private readonly IMapper _mapper;

    public DashBoardService(IVentaRepository ventaRepository, IGenericRepository<Producto> productoRepository, IMapper mapper)
    {
      _ventaRepository = ventaRepository;
      _productoRepository = productoRepository;
      _mapper = mapper;
    }

    private IQueryable<Venta> RetornarVentas(IQueryable<Venta> tablaVenta, int cantidadDiasRestar)
    {
      DateTime? ultimaFecha = tablaVenta.OrderByDescending(x => x.FechaRegistro).FirstOrDefault()?.FechaRegistro;
      ultimaFecha = ultimaFecha?.AddDays(cantidadDiasRestar);

      return tablaVenta.Where(v => v.FechaRegistro.Value.Date >= ultimaFecha.Value.Date);
    }

    private async Task<int> TotalVentasUltimaSemana()
    {
      int total = 0;
      IQueryable<Venta> _ventaQuery = await _ventaRepository.Consultar();
      if (_ventaQuery.Any())
      {
        var tablaVenta = RetornarVentas(_ventaQuery, -7);
        total = tablaVenta.Count();
      }

      return total;
    }

    private async Task<string> TotalIngresosUltimaSemana()
    {
      decimal resultado = 0;
      IQueryable<Venta> _ventaQuery = await _ventaRepository.Consultar();
      if (_ventaQuery.Any())
      {
        var tablaVenta = RetornarVentas(_ventaQuery, -7);
        resultado = tablaVenta.Select(v => v.Total).Sum(v => v.Value);
      }

      return Convert.ToString(resultado, new CultureInfo("es-MX"));
    }

    private async Task<int> TotalProductos()
    {
      IQueryable<Producto> _productoQuery = await _productoRepository.Consultar();
      int total = _productoQuery.Count();
      return total;
    }

    private async Task<Dictionary<string, int>> VentasUltimaSemana()
    {
      Dictionary<string, int> resultado = new();
      IQueryable<Venta> _ventaQuery = await _ventaRepository.Consultar();
      if (_ventaQuery.Any())
      {
        var tablaVenta = RetornarVentas(_ventaQuery, -7);
        resultado = tablaVenta.GroupBy(v => v.FechaRegistro.Value.Date)
                              .OrderBy(v => v.Key)
                              .Select(v => new { Fecha = v.Key.ToString("dd/MM/yyyy"), Total = v.Count() })
                              .ToDictionary(keySelector: r => r.Fecha, elementSelector: r => r.Total);
      }
      return resultado;
    }

    public async Task<DashBoardDTO> Resumen()
    {
      DashBoardDTO resultado = new();
      try
      {
        resultado.TotalVentas = await TotalVentasUltimaSemana();
        resultado.TotalIngresos = await TotalIngresosUltimaSemana();
        resultado.TotalProductos = await TotalProductos();
        
        List<VentaSemanaDTO> listaVentasSemana = new();
        foreach (KeyValuePair<string, int> item in await VentasUltimaSemana())
        {
          listaVentasSemana.Add(new VentaSemanaDTO { Fecha = item.Key, Total = item.Value });
        }

        resultado.VentaUltimaSemana = listaVentasSemana;

        return resultado;
      }
      catch
      {
        throw new Exception("No se pudo consultar el resumen");
      }
    }
  }
}
