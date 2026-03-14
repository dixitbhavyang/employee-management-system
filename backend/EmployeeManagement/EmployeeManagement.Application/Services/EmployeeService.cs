using EmployeeManagement.Application.DTOs.Common;
using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Application.Interfaces.Services;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Services
{
    /// <summary>
    /// Service for Employee business logic operations
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Get all employees with pagination and filtering
        /// </summary>
        public async Task<PagedResult<EmployeeDTO>> GetAllEmployeesAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            int? departmentId = null)
        {
            // Validate pagination parameters
            if (pageNumber < 1)
                throw new ValidationException("Page number must be greater than 0");

            if (pageSize < 1 || pageSize > 100)
                throw new ValidationException("Page size must be between 1 and 100");

            // Get data from repository
            var (employees, totalCount) = await _employeeRepository.GetAllAsync(
                pageNumber, pageSize, searchTerm, departmentId);

            // Convert entities to DTOs
            var employeeDTOs = employees.Select(e => MapToDTO(e)).ToList();

            // Return paged result
            return new PagedResult<EmployeeDTO>
            {
                Items = employeeDTOs,
                TotalRecords = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// Get a single employee by ID
        /// </summary>
        public async Task<EmployeeDTO?> GetEmployeeByIdAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);

            if (employee == null)
                return null;

            return MapToDTO(employee);
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        public async Task<EmployeeDTO> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            // BUSINESS RULE: Validate email doesn't exist
            if (await _employeeRepository.EmailExistsAsync(request.Email))
            {
                throw new ValidationException($"Email '{request.Email}' already exists");
            }

            // BUSINESS RULE: Validate salary is positive
            if (request.Salary <= 0)
            {
                throw new ValidationException("Salary must be greater than 0");
            }

            // BUSINESS RULE: Validate hire date is not in future
            if (request.HireDate > DateTime.UtcNow)
            {
                throw new ValidationException("Hire date cannot be in the future");
            }

            // Convert DTO to Entity
            var employee = new Employee
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = request.Email.Trim().ToLower(),
                Phone = request.Phone?.Trim(),
                DepartmentId = request.DepartmentId,
                JobTitle = request.JobTitle.Trim(),
                Salary = request.Salary,
                HireDate = request.HireDate,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            // Save to database
            var created = await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            // Get the created employee with department info
            var result = await _employeeRepository.GetByIdAsync(created.EmployeeId);

            // Convert back to DTO
            return MapToDTO(result!);
        }

        /// <summary>
        /// Update an existing employee
        /// </summary>
        public async Task<EmployeeDTO> UpdateEmployeeAsync(UpdateEmployeeRequest request)
        {
            // Check if employee exists
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null)
            {
                throw new NotFoundException("Employee", request.EmployeeId);
            }

            // BUSINESS RULE: Validate email doesn't exist (exclude current employee)
            if (await _employeeRepository.EmailExistsAsync(request.Email, request.EmployeeId))
            {
                throw new ValidationException($"Email '{request.Email}' already exists");
            }

            // BUSINESS RULE: Validate salary is positive
            if (request.Salary <= 0)
            {
                throw new ValidationException("Salary must be greater than 0");
            }

            // Update entity properties
            employee.FirstName = request.FirstName.Trim();
            employee.LastName = request.LastName.Trim();
            employee.Email = request.Email.Trim().ToLower();
            employee.Phone = request.Phone?.Trim();
            employee.DepartmentId = request.DepartmentId;
            employee.JobTitle = request.JobTitle.Trim();
            employee.Salary = request.Salary;
            employee.HireDate = request.HireDate;
            employee.IsActive = request.IsActive;
            employee.ModifiedDate = DateTime.UtcNow;

            // Save changes
            await _employeeRepository.UpdateAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            // Get updated employee with department
            var result = await _employeeRepository.GetByIdAsync(employee.EmployeeId);

            return MapToDTO(result!);
        }

        /// <summary>
        /// Delete an employee (soft delete)
        /// </summary>
        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
            {
                return false;
            }

            // Soft delete
            var deleted = await _employeeRepository.DeleteAsync(employeeId);
            if (deleted)
            {
                await _employeeRepository.SaveChangesAsync();
            }

            return deleted;
        }

        /// <summary>
        /// Check if email already exists
        /// </summary>
        public async Task<bool> EmailExistsAsync(string email, int? excludeEmployeeId = null)
        {
            return await _employeeRepository.EmailExistsAsync(email, excludeEmployeeId);
        }

        // ========== PRIVATE HELPER METHODS ==========

        /// <summary>
        /// Convert Employee Entity to EmployeeDTO
        /// </summary>
        private EmployeeDTO MapToDTO(Employee employee)
        {
            return new EmployeeDTO
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Phone = employee.Phone,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.DepartmentName ?? "Unknown",
                JobTitle = employee.JobTitle,
                Salary = employee.Salary,
                HireDate = employee.HireDate,
                IsActive = employee.IsActive,
                CreatedDate = employee.CreatedDate
            };
        }
    }
}
