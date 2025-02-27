using EMS_H_EmreCetin.Models;

namespace EMS_H_EmreCetin.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId);
        Task<IEnumerable<Employee>> GetEmployeesBySalaryRangeAsync(decimal minSalary, decimal maxSalary);
        Task<IEnumerable<Employee>> GetEmployeesByHireDateRangeAsync(DateTime startDate, DateTime endDate);

        Task<IEnumerable<Employee>> GetEmployeesWithFilteringAndSortingAsync(
            int? departmentId = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string sortBy = null,
            bool ascending = true);
    }
}
