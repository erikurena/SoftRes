using restaurante.Models;

namespace restaurante.Interfaces
{
    public interface IEmpleado
    {
        Task<List<Empleado>> GetEmpleados();
        Task<Empleado> GetEmpleadoById(int? id);
        Task CreateEmpleado(Empleado empleado);
        Task<bool> UpdateEmpleado(int? id, Empleado empleado);
        Task<bool> DeleteEmpleado(int? id);
        Task<List<Cargo>> Cargos();
        Task CambiarPassword(int? id, Empleado empleado);
    }

}
