using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utility;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class MenuController : ControllerBase
  {
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
      _menuService = menuService;
    }

    [HttpGet]
    [Route("Lista")]
    public async Task<IActionResult> Lista(int idUsuario)
    {
      var response = new Response<List<MenuDTO>>();
      try
      {
        response.status = true;
        response.value = await _menuService.Lista(idUsuario);
        response.message = "Lista de menus";
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
