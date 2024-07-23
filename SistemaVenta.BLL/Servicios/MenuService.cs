using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
  public class MenuService : IMenuService
  {
    private readonly IGenericRepository<Menu> _menuRepository;
    private readonly IGenericRepository<Usuario> _usuarioRepository;
    private readonly IGenericRepository<MenuRol> _menuRolRepository;
    private readonly IMapper _mapper;

    public MenuService(IGenericRepository<Menu> menuRepository, IGenericRepository<Usuario> usuarioRepository, IGenericRepository<MenuRol> menuRolRepository, IMapper mapper)
    {
      _menuRepository = menuRepository;
      _usuarioRepository = usuarioRepository;
      _menuRolRepository = menuRolRepository;
      _mapper = mapper;
    }

    public async Task<List<MenuDTO>> Lista(int idUsuario)
    {
      IQueryable<Usuario> tblUsuario = await _usuarioRepository.Consultar(u => u.IdUsuario == idUsuario);
      IQueryable<MenuRol> tblMenuRol = await _menuRolRepository.Consultar();
      IQueryable<Menu> tblMenu = await _menuRepository.Consultar();

      try
      {
        IQueryable<Menu> tblResultado = (from u in tblUsuario
                                         join mr in tblMenuRol on u.IdRol equals mr.IdRol
                                         join m in tblMenu on mr.IdMenu equals m.IdMenu
                                         select m).AsQueryable();
        var listaMenus = tblResultado.ToList();
        return _mapper.Map<List<MenuDTO>>(listaMenus);
      }
      catch
      {
        throw new Exception("No se pudo consultar los menús");
      }
    }
  }
}
