using EmployeeManagement.Application.DTOs.Common;
using EmployeeManagement.Application.DTOs.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Interfaces.Services
{
    /// <summary>
    /// Interface for Employee business logic operations
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Get all employees with pagination and filtering
        /// </summary>
        Task<PagedResult<EmployeeDTO>> GetAllEmployeesAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            int? departmentId = null);

        /// <summary>
        /// Get a single employee by ID
        /// </summary>
        Task<EmployeeDTO?> GetEmployeeByIdAsync(int employeeId);

        /// <summary>
        /// Create a new employee
        /// </summary>
        Task<EmployeeDTO> CreateEmployeeAsync(CreateEmployeeRequest request);

        /// <summary>
        /// Update an existing employee
        /// </summary>
        Task<EmployeeDTO> UpdateEmployeeAsync(UpdateEmployeeRequest request);

        /// <summary>
        /// Delete an employee (soft delete - sets IsActive = false)
        /// </summary>
        Task<bool> DeleteEmployeeAsync(int employeeId);

        /// <summary>
        /// Check if email already exists (for validation)
        /// </summary>
        Task<bool> EmailExistsAsync(string email, int? excludeEmployeeId = null);
    }
}
