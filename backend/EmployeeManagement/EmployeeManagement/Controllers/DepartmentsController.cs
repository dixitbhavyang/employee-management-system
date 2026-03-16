using EmployeeManagement.Application.DTOs.Common;
using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(
            IDepartmentService departmentService,
            ILogger<DepartmentsController> logger)
        {
            _departmentService = departmentService;
            _logger = logger;
        }

        /// <summary>
        /// Get all departments
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<DepartmentDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DepartmentDTO>>> GetAll()
        {
            _logger.LogInformation("Getting all departments");

            var departments = await _departmentService.GetAllDepartmentsAsync();

            return Ok(departments);
        }

        /// <summary>
        /// Get department by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DepartmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DepartmentDTO>> GetById(int id)
        {
            _logger.LogInformation("Getting department with ID: {DepartmentId}", id);

            var department = await _departmentService.GetDepartmentByIdAsync(id);

            return Ok(department);
        }

        /// <summary>
        /// Create a new department
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(DepartmentDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DepartmentDTO>> Create(
            [FromBody] CreateDepartmentRequest request)
        {
            _logger.LogInformation("Creating new department: {DepartmentName}", request.DepartmentName);

            var department = await _departmentService.CreateDepartmentAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = department.DepartmentId },
                department);
        }

        /// <summary>
        /// Update an existing department
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DepartmentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DepartmentDTO>> Update(
            int id,
            [FromBody] UpdateDepartmentRequest request)
        {
            if (id != request.DepartmentId)
            {
                return BadRequest(new { message = "ID in URL doesn't match ID in body" });
            }

            _logger.LogInformation("Updating department with ID: {DepartmentId}", id);

            var department = await _departmentService.UpdateDepartmentAsync(request);

            return Ok(department);
        }

        /// <summary>
        /// Delete a department (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting department with ID: {DepartmentId}", id);

            var result = await _departmentService.DeleteDepartmentAsync(id);

            if (!result)
            {
                return NotFound(new { message = $"Department with ID {id} not found" });
            }

            return NoContent();
        }
    }
}
