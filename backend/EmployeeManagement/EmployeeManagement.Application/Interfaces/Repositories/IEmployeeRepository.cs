using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Interfaces.Repositories
{
    /// <summary>
    /// Interface for Employee database operations (Repository Pattern)
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Get all employees with filtering
        /// </summary>
        Task<(List<Employee> Items, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            int? departmentId);

        /// <summary>
        /// Get employee by ID (includes Department)
        /// </summary>
        Task<Employee?> GetByIdAsync(int employeeId);

        /// <summary>
        /// Add new employee to database
        /// </summary>
        Task<Employee> AddAsync(Employee employee);

        /// <summary>
        /// Update existing employee
        /// </summary>
        Task<Employee> UpdateAsync(Employee employee);

        /// <summary>
        /// Delete employee (soft delete)
        /// </summary>
        Task<bool> DeleteAsync(int employeeId);

        /// <summary>
        /// Check if email exists
        /// </summary>
        Task<bool> EmailExistsAsync(string email, int? excludeEmployeeId = null);

        /// <summary>
        /// Save changes to database
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}
