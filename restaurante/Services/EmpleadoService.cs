using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using restaurante.dbContext;
using restaurante.Interfaces;
using restaurante.Models;

namespace restaurante.Services
{
    public class EmpleadoService : IEmpleado
    {
        private readonly DbrestauranteContext _context;
        private readonly IPasswordHasher<Empleado> _passwordHasher;
        public EmpleadoService(DbrestauranteContext context, IPasswordHasher<Empleado> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task<List<Empleado>> GetEmpleados()
        {
            var empleados = await _context.Empleados.AsNoTracking().Include(e => e.IdCargoNavigation).ToListAsync();
            return empleados;
        }
        public async Task<Empleado> GetEmpleadoById(int? id)
        {
          var empleado = await _context.Empleados.AsNoTracking()
                .Include(e => e.IdCargoNavigation)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            return empleado;
        }
        public async Task CreateEmpleado(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

        }
        public async Task<bool> UpdateEmpleado(int? id, Empleado empleado)
        {
            if (id != empleado.IdEmpleado)
                return false;

              _context.Empleados.Update(empleado);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteEmpleado(int? id)
        {
            var empleado = await _context.Empleados.FindAsync(id);

            if (empleado == null)
                return false;

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<Cargo>> Cargos()
        {
            return await _context.Cargos.AsNoTracking().ToListAsync();
        } 
        public async Task CambiarPassword(int? id, Empleado empleado)
        {
            var objUsuario = await _context.Empleados.FirstOrDefaultAsync(x => x.IdEmpleado == id);
            if (objUsuario == null)
                return;

            objUsuario.Pass = _passwordHasher.HashPassword(empleado, empleado.Pass!);

            objUsuario.Pass = empleado.Pass;
            _context.Empleados.Attach(objUsuario);            
            await _context.SaveChangesAsync();
        }
    }
}
