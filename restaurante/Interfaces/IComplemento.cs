using restaurante.Models;

namespace restaurante.Interfaces
{
    public interface IComplemento
    {
        Task<List<Complemento>> GetAllComplementos(int idCategoriaComplemento);
        Task<Complemento> GetComplementoById(int? id);
        Task CreateComplemento(Complemento complemento);
        Task<bool> UpdateComplemento(int id, Complemento complemento);
        Task<bool> DeleteComplemento(int id);
        Task<List<Categoriacomplemento>> GetAllCategoriasComplemento();
    }

}
