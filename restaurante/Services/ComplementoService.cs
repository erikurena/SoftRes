using Microsoft.EntityFrameworkCore;
using restaurante.dbContext;
using restaurante.Interfaces;
using restaurante.Models;

namespace restaurante.Services
{
    public class ComplementoService : IComplemento
    {

        private readonly DbrestauranteContext _context;
        public ComplementoService(DbrestauranteContext context)
        {
            _context = context;
        }
        public async Task<List<Complemento>> GetAllComplementos(int id)
        {
            var complementos = await _context.Complementos.AsNoTracking()
                .Where(x => id == 1 ? x.IdCategoriaComplemento == 1 : x.IdCategoriaComplemento == 2)
                .Include(x => x.IdCategoriaComplementoNavigation).ToListAsync();
                
            return complementos;
        }
        public async Task<Complemento> GetComplementoById(int? id)
        {
            return await _context.Complementos
                .AsNoTracking()
                .Include(x => x.IdCategoriaComplementoNavigation)
                .FirstOrDefaultAsync(m => m.IdComplemento == id);
        }
        public async Task CreateComplemento(Complemento complemento)
        {
            _context.Complementos.Add(complemento);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateComplemento(int id, Complemento complemento)
        {
            _context.Complementos.Update(complemento);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteComplemento(int id)
        {
            var complemento = await GetComplementoById(id);
            if (complemento == null) return false;
            _context.Complementos.Remove(complemento);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<List<Categoriacomplemento>> GetAllCategoriasComplemento()
        {
            return await _context.Categoriacomplementos.AsNoTracking().ToListAsync();
        }
    }
}
