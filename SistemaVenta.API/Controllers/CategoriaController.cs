using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utility;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CategoriaController : ControllerBase
  {
    private readonly ICategoriaService _categoriaService;

    public CategoriaController(ICategoriaService categoriaService)
    {
      _categoriaService = categoriaService;
    }

    [HttpGet]
    [Route("Lista")]
    public async Task<IActionResult> Lista()
    {
      var response = new Response<List<CategoriaDTO>>();
      try
      {
        response.status = true;
        response.value = await _categoriaService.Lista();
        response.message = "Lista de categorias";
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
