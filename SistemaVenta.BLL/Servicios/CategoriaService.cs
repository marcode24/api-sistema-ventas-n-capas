using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Model;

namespace SistemaVenta.BLL.Servicios
{
  public class CategoriaService : ICategoriaService
  {
    private readonly IGenericRepository<Categoria> _categoriaRepository;
    private readonly IMapper _mapper;

    public CategoriaService(IGenericRepository<Categoria> categoriaRepository, IMapper mapper)
    {
      _categoriaRepository = categoriaRepository;
      _mapper = mapper;
    }

    public async Task<List<CategoriaDTO>> Lista()
    {
      try
      {
        var listaCategorias = await _categoriaRepository.Consultar();
        return _mapper.Map<List<CategoriaDTO>>(listaCategorias.ToList());
      }
      catch
      {
        throw new Exception("No se pudo consultar las categorías");
      }
    }
  }
}
