using Microsoft.EntityFrameworkCore;

namespace EMS_H_EmreCetin.Models
{
    [Keyless]
    public class DepartmentStatistics
    {
        public int DepartmentId { get; set; }
        public required string DepartmentName { get; set; }
        public int TotalEmployees { get; set; }
        public decimal AverageSalary { get; set; }
    }
}