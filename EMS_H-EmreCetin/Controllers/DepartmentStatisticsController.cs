using EMS_H_EmreCetin.Interfaces;
using EMS_H_EmreCetin.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS_H_EmreCetin.Controllers
{
    public class DepartmentStatisticsController : Controller
    {
        private readonly IDepartmentStatisticsRepository _statisticsRepository;
        private readonly ILogger<DepartmentStatisticsController> _logger;

        public DepartmentStatisticsController(IDepartmentStatisticsRepository statisticsRepository, ILogger<DepartmentStatisticsController> logger)
        {
            _statisticsRepository = statisticsRepository;
            _logger = logger;
        }

        // GET: DepartmentStatistics
        public async Task<IActionResult> Index()
        {
            try
            {
                var statistics = await _statisticsRepository.GetDepartmentStatisticsAsync();
                return View(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching department statistics.");
                TempData["ErrorMessage"] = "An error occurred while fetching department statistics. Please try again later.";
                return View(new List<DepartmentStatistics>());
            }
        }
    }
}
