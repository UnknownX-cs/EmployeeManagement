using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeApp.Data;
using EmployeeApp.Models;

namespace EmployeeApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Default GET: /api/employees → only active employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees
                .Where(e => !e.IsArchived)
                .ToListAsync();
        }

        // GET by ID: /api/employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            return emp == null ? NotFound() : Ok(emp);
        }

        // POST: /api/employees
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }

        // PUT: /api/employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Employee employee)
        {
            if (id != employee.Id) return BadRequest();
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Soft-delete (archive)
        [HttpDelete("{id}")]
        public async Task<IActionResult> ArchiveEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            employee.IsArchived = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ Archived employees list: /api/employees/archived
        [HttpGet("archived")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetArchivedEmployees()
        {
            return await _context.Employees
                .Where(e => e.IsArchived)
                .ToListAsync();
        }

        // ✅ Restore archived: /api/employees/restore/5
        [HttpPatch("restore/{id}")]
        public async Task<IActionResult> RestoreEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            employee.IsArchived = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ Hard delete: /api/employees/hard-delete/5
        [HttpDelete("hard-delete/{id}")]
        public async Task<IActionResult> HardDeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}