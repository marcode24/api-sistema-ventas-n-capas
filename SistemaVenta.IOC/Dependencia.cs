using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVenta.BLL.Servicios;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Utility;

namespace SistemaVenta.IOC
{
  public static class Dependencia
  {
    public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<DbventaContext>(options =>
      {
        options.UseSqlServer(configuration.GetConnectionString("cadenaSQL"));    
      });

      services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
      services.AddScoped<IVentaRepository, VentaRepository>();

      services.AddAutoMapper(typeof (AutoMapperProfile));

      services.AddScoped<IRolService, RolService>();
      services.AddScoped<IUsuarioService, UsuarioService>();
      services.AddScoped<IVentaService, VentaService>();
      services.AddScoped<ICategoriaService, CategoriaService>();
      services.AddScoped<IProductoService, ProductoService>();
      services.AddScoped<IDashBoardService, DashBoardService>();
      services.AddScoped<IMenuService, MenuService>();
    }
  }
}
