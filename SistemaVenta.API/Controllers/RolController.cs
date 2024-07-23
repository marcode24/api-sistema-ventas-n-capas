using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utility;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class RolController : ControllerBase
  {
    private readonly IRolService _rolService;

    public RolController(IRolService rolService)
    {
      _rolService = rolService;
    }

    [HttpGet]
    [Route("Lista")]
    public async Task<IActionResult> Lista()
    {
      var response = new Response<List<RolDTO>>();
      try
      {
        response.status = true;
        response.value = await _rolService.Lista();
        response.message = "Lista de roles";
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
