using EMS_H_EmreCetin.Interfaces;
using EMS_H_EmreCetin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EMS_H_EmreCetin.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(IDepartmentRepository departmentRepository, ILogger<DepartmentController> logger)
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _departmentRepository.AddAsync(department);
                    TempData["Success"] = "Department created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                return View(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a department.");
                TempData["ErrorMessage"] = "An error occurred while creating the department. Please try again later.";
                return View(department);
            }
        }

        // GET: Department
        public async Task<IActionResult> Index()
        {
            try
            {
                var departments = await _departmentRepository.GetAllWithEmployeeCountAsync();
                return View(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching departments.");
                TempData["ErrorMessage"] = "An error occurred while fetching departments. Please try again later.";
                return View(new List<Department>());
            }
        }

        // GET: Department/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var department = await _departmentRepository.GetDepartmentWithEmployeesAsync(id);
                if (department == null)
                {
                    return NotFound();
                }
                return View(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching department details.");
                TempData["ErrorMessage"] = "An error occurred while fetching department details. Please try again later.";
                return RedirectToAction(nameof(Index));
            }
            
        }

        // GET: Department/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Department/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(id);
                if (department == null)
                {
                    return NotFound();
                }
                return View(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading the edit form.");
                TempData["ErrorMessage"] = "An error occurred while loading the edit form. Please try again later.";
                return RedirectToAction(nameof(Index));
            }
            
        }

        // POST: Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            try
            {
                if (id != department.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _departmentRepository.UpdateAsync(department);
                    return RedirectToAction(nameof(Index));
                }
                return View(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the department.");
                TempData["ErrorMessage"] = "An error occurred while updating the department. Please try again later.";
                return View(department);
            }
            
        }

        // GET: Department/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(id);
                if (department == null)
                {
                    return NotFound();
                }
                return View(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading the delete confirmation.");
                TempData["ErrorMessage"] = "An error occurred while loading the delete confirmation. Please try again later.";
                return RedirectToAction(nameof(Index));
            }
           
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(id);
                if (department != null)
                {
                    await _departmentRepository.DeleteAsync(department);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the department.");
                TempData["ErrorMessage"] = "An error occurred while deleting the department. Please try again later.";
                return RedirectToAction(nameof(Index));
            }
            
        }
    }
}
