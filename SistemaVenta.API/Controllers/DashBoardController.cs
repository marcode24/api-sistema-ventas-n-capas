using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utility;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DashBoardController : ControllerBase
  {
    private readonly IDashBoardService _dashBoardService;

    public DashBoardController(IDashBoardService dashBoardService)
    {
      _dashBoardService = dashBoardService;
    }

    [HttpGet]
    [Route("Resumen")]
    public async Task<IActionResult> Resumen()
    {
      var response = new Response<DashBoardDTO>();
      try
      {
        response.status = true;
        response.value = await _dashBoardService.Resumen();
        response.message = "Resumen de ventas";
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
