using EMS_H_EmreCetin.Data;
using EMS_H_EmreCetin.Interfaces;
using EMS_H_EmreCetin.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS_H_EmreCetin.Repositories
{
    public class EmployeeRepository(ApplicationDbContext context) : GenericRepository<Employee>(context) , IEmployeeRepository
    {
        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(int departmentId)
        {
            return await _dbSet
                .Include(e => e.Department)
                .Where(e => e.DepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByHireDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(e => e.Department)
                .Where(e => e.HireDate >= startDate && e.HireDate <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesBySalaryRangeAsync(decimal minSalary, decimal maxSalary)
        {
            return await _dbSet
                 .Include(e => e.Department)
                 .Where(e => e.Salary >= minSalary && e.Salary <= maxSalary)
                 .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesWithFilteringAndSortingAsync(
            int? departmentId = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string sortBy = null,
            bool ascending = true)
        {
            var query = _dbSet.Include(e => e.Department).AsQueryable();

            // Apply filtering
            query = ApplyFilters(query, departmentId, startDate, endDate, minSalary, maxSalary);

            // Apply sorting
            query = ApplySorting(query, sortBy, ascending);

            return await query.ToListAsync();
        }

        private IQueryable<Employee> ApplyFilters(
            IQueryable<Employee> query,
            int? departmentId = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            decimal? minSalary = null,
            decimal? maxSalary = null)
        {
            return query
                .Where(e => !departmentId.HasValue || e.DepartmentId == departmentId.Value)
                .Where(e => !startDate.HasValue || e.HireDate >= startDate.Value)
                .Where(e => !endDate.HasValue || e.HireDate <= endDate.Value)
                .Where(e => !minSalary.HasValue || e.Salary >= minSalary.Value)
                .Where(e => !maxSalary.HasValue || e.Salary <= maxSalary.Value);
        }

        private IQueryable<Employee> ApplySorting(
            IQueryable<Employee> query,
            string sortBy = null,
            bool ascending = true)
        {
            var sortExpressions = new Dictionary<string, Func<IQueryable<Employee>, IOrderedQueryable<Employee>>>
            {   
                { "name", q => ascending ? q.OrderBy(e => e.FullName) : q.OrderByDescending(e => e.FullName) },
                { "hiredate", q => ascending ? q.OrderBy(e => e.HireDate) : q.OrderByDescending(e => e.HireDate) },
                { "salary", q => ascending ? q.OrderBy(e => e.Salary) : q.OrderByDescending(e => e.Salary) }
            };

            var defaultSortExpression = sortExpressions["name"];

            if (!string.IsNullOrEmpty(sortBy))
            {
                var sortKey = sortBy.ToLower();
                if (sortExpressions.TryGetValue(sortKey, out var sortExpression))
                {
                    return sortExpression(query);
                }
                else
                {
                    return sortExpressions["name"](query);
                }
            }

            return defaultSortExpression(query);
        }
    }
}
