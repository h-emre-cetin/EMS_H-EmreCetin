using EMS_H_EmreCetin.Data;
using EMS_H_EmreCetin.Interfaces;
using EMS_H_EmreCetin.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS_H_EmreCetin.Repositories
{
    public class DepartmentRepository(ApplicationDbContext context) : GenericRepository<Department>(context), IDepartmentRepository
    {
        public async Task<IEnumerable<Department>> GetAllWithEmployeeCountAsync()
        {
            return await _dbSet
                .Include(d => d.Employees)
                .ToListAsync();
        }

        public async Task<Department> GetDepartmentWithEmployeesAsync(int id)
        {
            return await _dbSet
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
