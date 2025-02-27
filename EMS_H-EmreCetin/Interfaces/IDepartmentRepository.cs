using EMS_H_EmreCetin.Models;

namespace EMS_H_EmreCetin.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<Department> GetDepartmentWithEmployeesAsync(int id);

        Task<IEnumerable<Department>> GetAllWithEmployeeCountAsync();


    }
}
