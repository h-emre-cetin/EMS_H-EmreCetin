using EMS_H_EmreCetin.Data;
using EMS_H_EmreCetin.Interfaces;
using EMS_H_EmreCetin.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS_H_EmreCetin.Repositories
{
    public class DepartmentStatisticsRepository : IDepartmentStatisticsRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentStatisticsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DepartmentStatistics>> GetDepartmentStatisticsAsync()
        {
            return await _context.Set<DepartmentStatistics>()
                .FromSqlRaw("EXEC sp_GetDepartmentStatistics")
                .ToListAsync();
        }

        /* Store Produce'u local'imde yazdığım için göremeyeceksiniz.
         * 
         
         CREATE OR ALTER PROCEDURE sp_GetDepartmentStatistics

         AS
         
         BEGIN
         
         SELECT
         
         d.Id AS DepartmentId,
         
         d.Name AS DepartmentName,
         
         COUNT(e.Id) AS TotalEmployees,
         
         ISNULL(AVG(CAST(e.Salary AS DECIMAL(18,2))), 0) AS AverageSalary
         
         FROM
         
         Departments d
         
         LEFT JOIN Employees e ON d.Id = e.DepartmentId
         
         GROUP BY
         
         d.Id, d.Name
         
         ORDER BY
         
         d.Name;
         
         END
                  
                  */
    }
}
