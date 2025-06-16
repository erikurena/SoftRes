using restaurante.Dtos;
using restaurante.Models;

namespace restaurante.Interfaces
{
    public interface IProducto
    {
        Task<List<ProductoDto>> GetProducto(int? id);
        Task<Producto> Details(int? id);
        Task<bool> CrearProducto(Producto producto);
        Task<bool> ModificarProducto(int id, Producto producto);
        Task<bool> EliminarProducto(int id);
        Task<List<Categorium>> GetCategoria();
    }
}
