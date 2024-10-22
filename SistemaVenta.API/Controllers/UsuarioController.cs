﻿using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utility;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsuarioController : ControllerBase
  {
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
      _usuarioService = usuarioService;
    }

    [HttpGet]
    [Route("Lista")]
    public async Task<IActionResult> Lista()
    {
      var response = new Response<List<UsuarioDTO>>();
      try
      {
        response.status = true;
        response.value = await _usuarioService.Lista();
        response.message = "Lista de usuarios";
      }
      catch (Exception ex)
      {
        response.status = false;
        response.message = ex.Message;
      }
      return Ok(response);
    }

    [HttpPost]
    [Route("IniciarSesion")]
    public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO login)
    {
      var response = new Response<SesionDTO>();
      try
      {
        response.status = true;
        response.value = await _usuarioService.ValidarCredenciales(login.Correo, login.Clave);
        response.message = "Inicio de sesión";
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
    public async Task<IActionResult> Guardar([FromBody] UsuarioDTO usuario)
    {
      var response = new Response<UsuarioDTO>();
      try
      {
        response.status = true;
        response.value = await _usuarioService.Crear(usuario);
        response.message = response.status ? "Usuario guardado" : "Usuario no guardado";
      }
      catch (Exception ex)
      {
        response.status = false;
        response.message = ex.Message;
      }
      return Ok(response);
    }

    [HttpPut]
    [Route("Editar")]
    public async Task<IActionResult> Editar([FromBody] UsuarioDTO usuario)
    {
      var response = new Response<bool>();
      try
      {
        response.status = true;
        response.value = await _usuarioService.Editar(usuario);
        response.message = response.status ? "Usuario actualizado" : "Usuario no actualizado";
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
        response.status = true;
        response.value = await _usuarioService.Eliminar(id);
        response.message = response.status ? "Usuario eliminado" : "Usuario no eliminado";
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
