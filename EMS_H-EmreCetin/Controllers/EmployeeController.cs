using EMS_H_EmreCetin.Interfaces;
using EMS_H_EmreCetin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMS_H_EmreCetin.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository, ILogger<EmployeeController> logger)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _logger = logger;
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _employeeRepository.AddAsync(employee);
                    TempData["Success"] = "Employee created successfully!";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Departments = new SelectList(await _departmentRepository.GetAllAsync(), "Id", "Name");
                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating an employee.");
                TempData["Error"] = "Failed to create employee. Please try again.";
                ViewBag.Departments = new SelectList(await _departmentRepository.GetAllAsync(), "Id", "Name");
                return View(employee);
            }
        }

        // GET: Employee
        public async Task<IActionResult> Index(
            int? departmentId,
            DateTime? startDate,
            DateTime? endDate,
            decimal? minSalary,
            decimal? maxSalary,
            string sortBy,
            string sortOrder = "asc")
        {
            try
            {
                var departments = await _departmentRepository.GetAllAsync();
                if (departments == null || !departments.Any())
                {
                    _logger.LogWarning("No departments found in the database.");
                    ViewBag.Departments = new SelectList(new List<Department>(), "Id", "Name"); // Empty list
                }
                else
                {
                    ViewBag.Departments = new SelectList(departments, "Id", "Name");
                }

                if (sortOrder != "asc" && sortOrder != "desc")
                {
                    sortOrder = "asc";
                }

                var employees = await _employeeRepository.GetEmployeesWithFilteringAndSortingAsync(
                    departmentId,
                    startDate,
                    endDate,
                    minSalary,
                    maxSalary,
                    sortBy,
                    sortOrder == "asc");

                if (employees == null || !employees.Any())
                {
                    _logger.LogWarning("No employees found with the specified criteria.");
                    return View(new List<Employee>()); // Return an empty list
                }


                return View(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching employees.");
                TempData["ErrorMessage"] = "An error occurred while fetching employees. Please try again later.";
                return View(new List<Employee>());
            }
            
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching employee details.");
                TempData["ErrorMessage"] = "An error occurred while fetching employee details. Please try again later.";
                return RedirectToAction("Index");
            }
            
        }

        // GET: Employee/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.Departments = new SelectList(await _departmentRepository.GetAllAsync(), "Id", "Name");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading the create form.");
                TempData["ErrorMessage"] = "An error occurred while loading the create form. Please try again later.";
                return RedirectToAction("Index");
            }

           
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                ViewBag.Departments = new SelectList(await _departmentRepository.GetAllAsync(), "Id", "Name");
                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading the edit form.");
                TempData["ErrorMessage"] = "An error occurred while loading the edit form. Please try again later.";
                return RedirectToAction("Index");
            }
            
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            try
            {
                if (id != employee.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _employeeRepository.UpdateAsync(employee);
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Departments = new SelectList(await _departmentRepository.GetAllAsync(), "Id", "Name");
                return View(employee);
            }
            catch (Exception ex )
            {
                _logger.LogError(ex, "An error occurred while updating the employee.");
                TempData["ErrorMessage"] = "An error occurred while updating the employee. Please try again later.";
                ViewBag.Departments = new SelectList(await _departmentRepository.GetAllAsync(), "Id", "Name");
                return View(employee);
            }
            
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading the delete confirmation.");
                TempData["ErrorMessage"] = "An error occurred while loading the delete confirmation. Please try again later.";
                return RedirectToAction("Index");
            }
            
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id);
                if (employee != null)
                {
                    await _employeeRepository.DeleteAsync(employee);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex )
            {
                _logger.LogError(ex, "An error occurred while deleting the employee.");
                TempData["ErrorMessage"] = "An error occurred while deleting the employee. Please try again later.";
                return RedirectToAction("Index");
            }
        }
    }
}
