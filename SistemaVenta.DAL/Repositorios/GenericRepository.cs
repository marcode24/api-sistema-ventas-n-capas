using Microsoft.EntityFrameworkCore;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios.Contrato;
using System.Linq.Expressions;

namespace SistemaVenta.DAL.Repositorios
{
  public class GenericRepository<TModelo> : IGenericRepository<TModelo> where TModelo : class
  {
    private readonly DbventaContext _dbventaContext;

    public GenericRepository(DbventaContext dbventaContext)
    {
      _dbventaContext = dbventaContext;
    }

    public async Task<IQueryable<TModelo>> Consultar(Expression<Func<TModelo, bool>>? filtro = null)
    {
      try
      {
        IQueryable<TModelo> query = _dbventaContext.Set<TModelo>();
        if (filtro != null) query = query.Where(filtro);
        return query;
      }
      catch (Exception)
      {
        throw new Exception("No se pudo consultar los modelos");
      }
    }

    public async Task<TModelo> Crear(TModelo modelo)
    {
      try
      {
        _dbventaContext.Set<TModelo>().Add(modelo);
        await _dbventaContext.SaveChangesAsync();
        return modelo;
      }
      catch (Exception)
      {
        throw new Exception("No se pudo registrar el modelo");
      }
    }

    public async Task<bool> Editar(TModelo modelo)
    {
      try
      {
        _dbventaContext.Set<TModelo>().Update(modelo);
        return await _dbventaContext.SaveChangesAsync() > 0;
      }
      catch (Exception)
      {
        throw new Exception("No se pudo editar el modelo");
      }
    }

    public async Task<bool> Eliminar(TModelo modelo)
    {
      try
      {
        _dbventaContext.Set<TModelo>().Remove(modelo);
        return await _dbventaContext.SaveChangesAsync() > 0;
      }
      catch (Exception)
      {
        throw new Exception("No se pudo eliminar el modelo");
      }
    }

    public async Task<TModelo> Obtener(Expression<Func<TModelo, bool>> filtro)
    {
      try
      {
        TModelo modelo = await _dbventaContext.Set<TModelo>().FirstOrDefaultAsync(filtro);
        return modelo;
      } catch (Exception ex)
      {
        throw new Exception("No se pudo obtener el modelo");
      }
    }
  }
}
