using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
  public class RolService : IRolService
  {
    private readonly IGenericRepository<Rol> _rolRepository;
    private readonly IMapper _mapper;

    public RolService(IGenericRepository<Rol> rolRepository, IMapper mapper)
    {
      _rolRepository = rolRepository;
      _mapper = mapper;
    }

    public async Task<List<RolDTO>> Lista()
    {
      try
      {
        var listaRoles = await _rolRepository.Consultar();
        return _mapper.Map<List<RolDTO>>(listaRoles.ToList());
      }
      catch
      {
        throw new Exception("No se pudo consultar los roles");
      }
    }
  }
}
