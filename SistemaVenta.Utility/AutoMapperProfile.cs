using AutoMapper;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.Utility
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      #region Rol
      CreateMap<Rol, RolDTO>().ReverseMap();
      #endregion Rol

      #region Menu
      CreateMap<Menu, MenuDTO>().ReverseMap();
      #endregion Menu

      #region Usuario
      CreateMap<Usuario, UsuarioDTO>()
        .ForMember(dest => dest.RolDescripcion, opt => opt.MapFrom(src => src.IdRolNavigation.Nombre))
        .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(src => src.EsActivo == true ? 1 : 0));

      CreateMap<Usuario, SesionDTO>()
        .ForMember(dest => dest.RolDescripcion, opt => opt.MapFrom(src => src.IdRolNavigation.Nombre));

      CreateMap<UsuarioDTO, Usuario>()
        .ForMember(dest => dest.IdRolNavigation, opt => opt.Ignore())
        .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(src => src.EsActivo == 1 ? true : false));
      #endregion Usuario

      #region Categoria
      CreateMap<Categoria, CategoriaDTO>().ReverseMap();
      #endregion Categoria

      #region Producto
      CreateMap<Producto, ProductoDTO>()
        .ForMember(dest => dest.DescipcionCategoria, opt => opt.MapFrom(src => src.IdCategoriaNavigation.Nombre))
        .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => Convert.ToString(src.Precio.Value, new CultureInfo("es-MX"))))
        .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(src => src.EsActivo == true ? 1 : 0));

      CreateMap<ProductoDTO, Producto>()
        .ForMember(dest => dest.IdCategoriaNavigation, opt => opt.Ignore())
        .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => Convert.ToDecimal(src.Precio, new CultureInfo("es-MX"))))
        .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(src => src.EsActivo == 1 ? true : false));
      #endregion Producto

      #region Venta
      CreateMap<Venta, VentaDTO>()
        .ForMember(dest => dest.TotalTexto, opt => opt.MapFrom(opt => Convert.ToString(opt.Total.Value, new CultureInfo("es-MX"))))
        .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(opt => opt.FechaRegistro.Value.ToString("dd/MM/yyyy")));

      CreateMap<VentaDTO, Venta>()
        .ForMember(dest => dest.Total, opt => opt.MapFrom(opt => Convert.ToDecimal(opt.TotalTexto, new CultureInfo("es-MX"))));
      #endregion Venta

      #region DetalleVenta
      CreateMap<DetalleVenta, DetalleVentaDTO>()
        .ForMember(dest => dest.DescripcionProducto, opt => opt.MapFrom(src => src.IdProductoNavigation.Nombre))
        .ForMember(dest => dest.PrecioTexto, opt => opt.MapFrom(src => Convert.ToString(src.Precio.Value, new CultureInfo("es-MX"))))
        .ForMember(dest => dest.TotalTexto, opt => opt.MapFrom(src => Convert.ToString(src.Total.Value, new CultureInfo("es-MX"))));

      CreateMap<DetalleVentaDTO, DetalleVenta>()
        .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => Convert.ToDecimal(src.PrecioTexto, new CultureInfo("es-MX"))))
        .ForMember(dest => dest.Total, opt => opt.MapFrom(src => Convert.ToDecimal(src.TotalTexto, new CultureInfo("es-MX"))));
      #endregion DetalleVenta

      #region Reporte
      CreateMap<DetalleVenta, ReporteDTO>()
        .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(opt => opt.IdVentaNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy")))
        .ForMember(dest => dest.NumeroDocumento, opt => opt.MapFrom(opt => opt.IdVentaNavigation.NumeroDocumento))
        .ForMember(dest => dest.TipoPago, opt => opt.MapFrom(opt => opt.IdVentaNavigation.TipoPago))
        .ForMember(dest => dest.TotalVenta, opt => opt.MapFrom(opt => Convert.ToString(opt.IdVentaNavigation.Total.Value, new CultureInfo("es-MX"))))
        .ForMember(dest => dest.Producto, opt => opt.MapFrom(opt => opt.IdProductoNavigation.Nombre))
        .ForMember(dest => dest.Precio, opt => opt.MapFrom(opt => Convert.ToString(opt.Precio.Value, new CultureInfo("es-MX"))))
        .ForMember(dest => dest.Total, opt => opt.MapFrom(opt => Convert.ToString(opt.Total.Value, new CultureInfo("es-MX"))));
      #endregion Reporte
    }
  }
}
