using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System.Globalization;

namespace SistemaVenta.BLL.Servicios
{
  public class VentaService : IVentaService
  {
    private readonly IVentaRepository _ventaRepository;
    private readonly IGenericRepository<DetalleVenta> _detalleVentaRepository;
    private readonly IMapper _mapper;

    public VentaService(IVentaRepository ventaRepository, IGenericRepository<DetalleVenta> detalleVentaRepository, IMapper mapper)
    {
      _ventaRepository = ventaRepository;
      _detalleVentaRepository = detalleVentaRepository;
      _mapper = mapper;
    }

    public async Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
    {
      IQueryable<Venta> query = await _ventaRepository.Consultar();
      List<Venta> ListaResultado = new();
      try
      {
        if(buscarPor == "fecha")
        {
          DateTime fecha_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-MX"));
          DateTime fecha_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-MX"));

          ListaResultado = await query.Where(v => v.FechaRegistro.Value.Date >= fecha_inicio.Date
                                               && v.FechaRegistro.Value.Date <= fecha_fin.Date)
                                      .Include(dv => dv.DetalleVenta)
                                      .ThenInclude(p => p.IdProductoNavigation)
                                      .ToListAsync();
        } 
        else
        {
          ListaResultado = await query.Where(v => v.NumeroDocumento == numeroVenta)
                                      .Include(dv => dv.DetalleVenta)
                                      .ThenInclude(p => p.IdProductoNavigation)
                                      .ToListAsync();
        }

        return _mapper.Map<List<VentaDTO>>(ListaResultado);
      }
      catch
      {
        throw new Exception("No se pudo consultar el historial de ventas");
      }
    }

    public async Task<VentaDTO> Registrar(VentaDTO modelo)
    {
      try
      {
        var ventaGenerada = await _ventaRepository.Registrar(_mapper.Map<Venta>(modelo));
        if (ventaGenerada.IdVenta == 0) throw new TaskCanceledException("No se pudo registrar la venta");

        return _mapper.Map<VentaDTO>(ventaGenerada);
      }
      catch
      {
        throw new Exception("No se pudo registrar la venta");
      }
    }

    public async Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin)
    {
      IQueryable<DetalleVenta> query = await _detalleVentaRepository.Consultar();
      List<DetalleVenta> listaResultado = new();

      try
      {
        DateTime fecha_inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-MX"));
        DateTime fecha_fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-MX"));

        listaResultado = await query.Include(p => p.IdProductoNavigation)
                                    .Include(v => v.IdVentaNavigation)
                                    .Where(dv => dv.IdVentaNavigation.FechaRegistro.Value.Date >= fecha_inicio.Date
                                              && dv.IdVentaNavigation.FechaRegistro.Value.Date <= fecha_fin.Date)
                                    .ToListAsync();

        return _mapper.Map<List<ReporteDTO>>(listaResultado);
      }
      catch
      {
        throw new Exception("No se pudo consultar el reporte de ventas");
      }
    }
  }
}
