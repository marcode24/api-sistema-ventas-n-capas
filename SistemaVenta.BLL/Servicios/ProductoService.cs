using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
  public class ProductoService : IProductoService
  {
    private readonly IGenericRepository<Producto> _productoRepository;
    private readonly IMapper _mapper;

    public ProductoService(IGenericRepository<Producto> productoRepository, IMapper mapper)
    {
      _productoRepository = productoRepository;
      _mapper = mapper;
    }

    public async Task<ProductoDTO> Crear(ProductoDTO modelo)
    {
      try
      {
        var productoCreado = await _productoRepository.Crear(_mapper.Map<Producto>(modelo));
        if (productoCreado.IdProducto == 0) throw new TaskCanceledException("No se pudo registrar el producto");

        return _mapper.Map<ProductoDTO>(productoCreado);
      }
      catch
      {
        throw new Exception("No se pudo registrar el producto");
      }
    }

    public async Task<bool> Editar(ProductoDTO modelo)
    {
      try
      {
        var productoModelo = _mapper.Map<Producto>(modelo);
        var productoEncontrado = await _productoRepository.Obtener(p => p.IdProducto == productoModelo.IdProducto);
        if (productoEncontrado == null) throw new TaskCanceledException("Producto no encontrado");

        productoEncontrado.Nombre = productoModelo.Nombre;
        productoEncontrado.IdCategoria = productoModelo.IdCategoria;
        productoEncontrado.Stock = productoModelo.Stock;
        productoEncontrado.Precio = productoModelo.Precio;
        productoEncontrado.EsActivo = productoModelo.EsActivo;

        bool respuesta = await _productoRepository.Editar(productoEncontrado);
        if(!respuesta) throw new TaskCanceledException("No se pudo editar el producto");

        return respuesta;
      }
      catch
      {
        throw new Exception("No se pudo editar el producto");
      }
    }

    public async Task<bool> Eliminar(int id)
    {
      try
      {
        var productoEncontrado = await _productoRepository.Obtener(p => p.IdProducto == id);
        if (productoEncontrado == null) throw new TaskCanceledException("Producto no encontrado");

        bool respuesta = await _productoRepository.Eliminar(productoEncontrado);
        if (!respuesta) throw new TaskCanceledException("No se pudo eliminar el producto");
        
        return respuesta;
      }
      catch
      {
        throw new Exception("No se pudo eliminar el producto");
      }
    }

    public async Task<List<ProductoDTO>> Lista()
    {
      try
      {
        var queryProducto = await _productoRepository.Consultar();
        var listaProductos = queryProducto.Include(cat => cat.IdCategoriaNavigation).ToList();

        return _mapper.Map<List<ProductoDTO>>(listaProductos);
      }
      catch
      {
        throw new Exception("No se pudo consultar los productos");
      }
    }
  }
}
