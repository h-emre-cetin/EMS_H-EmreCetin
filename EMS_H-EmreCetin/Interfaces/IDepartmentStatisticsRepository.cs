using EMS_H_EmreCetin.Models;

namespace EMS_H_EmreCetin.Interfaces
{
    public interface IDepartmentStatisticsRepository
    {
        Task<IEnumerable<DepartmentStatistics>> GetDepartmentStatisticsAsync();
    }
}
