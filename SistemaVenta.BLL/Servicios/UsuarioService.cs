using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
  public class UsuarioService : IUsuarioService
  {
    private readonly IGenericRepository<Usuario> _usuarioRepository;
    private readonly IMapper _mapper;

    public UsuarioService(IGenericRepository<Usuario> usuarioRepository, IMapper mapper)
    {
      _usuarioRepository = usuarioRepository;
      _mapper = mapper;
    }

    public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
    {
      try
      {
        var usuarioCreado = await _usuarioRepository.Crear(_mapper.Map<Usuario>(modelo));
        if (usuarioCreado.IdUsuario == 0) throw new Exception("No se pudo registrar el usuario");

        var query = await _usuarioRepository.Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);
        usuarioCreado = query.Include(rol => rol.IdRolNavigation).First();
        return _mapper.Map<UsuarioDTO>(usuarioCreado);
      }
      catch
      {
        throw new Exception("No se pudo registrar el usuario");
      }
    }

    public async Task<bool> Editar(UsuarioDTO modelo)
    {
      try
      {
        var usuarioModelo = _mapper.Map<Usuario>(modelo);
        var usuarioEncontrado = await _usuarioRepository.Obtener(u => u.IdUsuario == usuarioModelo.IdUsuario);

        if (usuarioEncontrado == null) throw new Exception("Usuario no encontrado");

        usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
        usuarioEncontrado.Correo = usuarioModelo.Correo;
        usuarioEncontrado.IdRol = usuarioModelo.IdRol;
        usuarioEncontrado.Clave = usuarioModelo.Clave;
        usuarioEncontrado.EsActivo = usuarioModelo.EsActivo;

        bool respuesta = await _usuarioRepository.Editar(usuarioEncontrado);
        if (!respuesta) throw new Exception("No se pudo editar el usuario");

        return respuesta;
      } catch {
        throw new Exception("No se pudo editar el usuario");
      }
    }

    public async Task<bool> Eliminar(int id)
    {
      try
      {
        var usuarioEncontrado = await _usuarioRepository.Obtener(u => u.IdUsuario == id);
        if (usuarioEncontrado == null) throw new Exception("Usuario no encontrado");

        bool respuesta = await _usuarioRepository.Eliminar(usuarioEncontrado);
        if (!respuesta) throw new Exception("No se pudo eliminar el usuario");
      
        return respuesta;
      }
      catch
      {
        throw new Exception("No se pudo eliminar el usuario");
      }
    }

    public async Task<List<UsuarioDTO>> Lista()
    {
      try
      {
        var queryUsuario = await _usuarioRepository.Consultar();
        var listaUsuarios = queryUsuario.Include(rol => rol.IdRolNavigation).ToList();
        return _mapper.Map<List<UsuarioDTO>>(listaUsuarios);
      }
      catch
      {
        throw new Exception("No se pudo consultar los usuarios");
      }
    }

    public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
    {
      try
      {
        var queryUsuario = await _usuarioRepository.Consultar(usuario => usuario.Correo == correo && usuario.Clave == clave);
        if(queryUsuario.FirstOrDefault() == null) throw new Exception("Credenciales incorrectas");

        Usuario devolverUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).First();
        return _mapper.Map<SesionDTO>(devolverUsuario);
      }
      catch
      {
        throw new Exception("No se pudo validar las credenciales");
      }
    }
  }
}
