using Microsoft.EntityFrameworkCore;
using restaurante.dbContext;
using restaurante.Dtos;
using restaurante.Interfaces;
using restaurante.Models;

namespace restaurante.Services
{
    public class ProductoService : IProducto
    {

        private readonly DbrestauranteContext _context;
        public ProductoService(DbrestauranteContext context)
        {
            _context = context;
        }
        public async Task<List<ProductoDto>> GetProducto(int? id)
        {
            var productos =   _context.Productos.AsNoTracking().Include(p => p.IdCategoriaNavigation)
            .Where(p =>  id == 2 ? p.IdCategoria == id : p.IdCategoria != 2)
              .Select(p => new ProductoDto
              {
                  IdProducto = p.IdProducto,
                  Nombre = p.Nombre,
                  Descripcion = p.Descripcion,
                  IdCategoria = p.IdCategoria,
                  Precio = p.Precio,
                  FotoProducto = p.FotoProducto,
                  IdCategoriaNavigation = new CategoriumDto
                  {
                      IdCategoria = p.IdCategoriaNavigation.IdCategoria,
                      TipoCategoria = p.IdCategoriaNavigation.TipoCategoria
                  }
              });

            return await productos.ToListAsync();
        }
        public async Task<Producto> Details(int? id)
        {
            return await _context.Productos.AsNoTracking().Include(x => x.IdCategoriaNavigation).FirstOrDefaultAsync(m => m.IdProducto == id);
        }
        public async Task<bool> CrearProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ModificarProducto(int id, Producto producto)
        {
            if (id != producto.IdProducto)
                return false;

            _context.Entry(producto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<Categorium>> GetCategoria()
        {
            return await _context.Categoria.ToListAsync();
        }
    }
}
