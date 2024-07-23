using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utility;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductoController : ControllerBase
  {
    private readonly IProductoService _productoService;

    public ProductoController(IProductoService productoService)
    {
      _productoService = productoService;
    }

    [HttpGet]
    [Route("Lista")]
    public async Task<IActionResult> Lista()
    {
      var response = new Response<List<ProductoDTO>>();
      try
      {
        response.status = true;
        response.value = await _productoService.Lista();
        response.message = "Lista de productos";
      }
      catch (Exception ex)
      {
        response.status = false;
        response.message = ex.Message;
      }
      return Ok(response);
    }

    [HttpPost]
    [Route("Guardar")]
    public async Task<IActionResult> Guardar([FromBody] ProductoDTO producto)
    {
      var response = new Response<ProductoDTO>();
      try
      {
        response.status = true;
        response.value = await _productoService.Crear(producto);
        response.message = response.status ? "Producto guardado" : "Error al guardar producto";
      }
      catch (Exception ex)
      {
        response.status = false;
        response.message = ex.Message;
      }
      return Ok(response);
    }

    [HttpPut]
    [Route("Actualizar")]
    public async Task<IActionResult> Actualizar([FromBody] ProductoDTO producto)
    {
      var response = new Response<ProductoDTO>();
      try
      {
        response.status = await _productoService.Editar(producto);
        response.value = producto;
        response.message = response.status ? "Producto actualizado" : "Error al actualizar producto";
      }
      catch (Exception ex)
      {
        response.status = false;
        response.message = ex.Message;
      }
      return Ok(response);
    }

    [HttpDelete]
    [Route("Eliminar/{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
      var response = new Response<bool>();
      try
      {
        response.status = await _productoService.Eliminar(id);
        response.value = response.status;
        response.message = response.status ? "Producto eliminado" : "Error al eliminar producto";
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
