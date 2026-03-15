using EmployeeManagement.Application.DTOs.Common;
using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(
            IEmployeeService employeeService,
            ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        /// <summary>
        /// Get all employees with pagination and filtering
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 10, max: 100)</param>
        /// <param name="searchTerm">Search by name or email</param>
        /// <param name="departmentId">Filter by department</param>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<EmployeeDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<EmployeeDTO>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] int? departmentId = null)
        {
            _logger.LogInformation("Getting employees - Page: {PageNumber}, Size: {PageSize}",
                pageNumber, pageSize);

            var result = await _employeeService.GetAllEmployeesAsync(
                pageNumber, pageSize, searchTerm, departmentId);

            return Ok(result);
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeDTO>> GetById(int id)
        {
            _logger.LogInformation("Getting employee with ID: {EmployeeId}", id);

            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found", id);
                return NotFound(new { message = $"Employee with ID {id} not found" });
            }

            return Ok(employee);
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        /// <param name="request">Employee creation data</param>
        [HttpPost]
        [ProducesResponseType(typeof(EmployeeDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeDTO>> Create(
            [FromBody] CreateEmployeeRequest request)
        {
            _logger.LogInformation("Creating new employee: {Email}", request.Email);

            var employee = await _employeeService.CreateEmployeeAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = employee.EmployeeId },
                employee);
        }

        /// <summary>
        /// Update an existing employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="request">Employee update data</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EmployeeDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeDTO>> Update(
            int id,
            [FromBody] UpdateEmployeeRequest request)
        {
            if (id != request.EmployeeId)
            {
                return BadRequest(new { message = "ID in URL doesn't match ID in body" });
            }

            _logger.LogInformation("Updating employee with ID: {EmployeeId}", id);

            var employee = await _employeeService.UpdateEmployeeAsync(request);

            return Ok(employee);
        }

        /// <summary>
        /// Delete an employee (soft delete)
        /// </summary>
        /// <param name="id">Employee ID</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting employee with ID: {EmployeeId}", id);

            var result = await _employeeService.DeleteEmployeeAsync(id);

            if (!result)
            {
                return NotFound(new { message = $"Employee with ID {id} not found" });
            }

            return NoContent();
        }

        /// <summary>
        /// Check if email exists
        /// </summary>
        /// <param name="email">Email to check</param>
        /// <param name="excludeId">Employee ID to exclude (for updates)</param>
        [HttpGet("check-email")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CheckEmail(
            [FromQuery] string email,
            [FromQuery] int? excludeId = null)
        {
            var exists = await _employeeService.EmailExistsAsync(email, excludeId);
            return Ok(exists);
        }
    }
}
