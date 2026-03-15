using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for Employee database operations
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all employees with filtering and pagination
        /// </summary>
        public async Task<(List<Employee> Items, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            int? departmentId)
        {
            // Start with base query
            var query = _context.Employees
                .Include(e => e.Department)  // Load department info (JOIN)
                .Where(e => e.IsActive)      // Only active employees
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(e =>
                    e.FirstName.ToLower().Contains(searchTerm) ||
                    e.LastName.ToLower().Contains(searchTerm) ||
                    e.Email.ToLower().Contains(searchTerm));
            }

            // Apply department filter
            if (departmentId.HasValue)
            {
                query = query.Where(e => e.DepartmentId == departmentId.Value);
            }

            // Get total count BEFORE pagination
            var totalCount = await query.CountAsync();

            // Apply pagination and sorting
            var items = await query
                .OrderByDescending(e => e.CreatedDate)  // Newest first
                .Skip((pageNumber - 1) * pageSize)      // Skip previous pages
                .Take(pageSize)                         // Take current page
                .ToListAsync();

            return (items, totalCount);
        }

        /// <summary>
        /// Get employee by ID with department info
        /// </summary>
        public async Task<Employee?> GetByIdAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.Department)  // Load department
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }

        /// <summary>
        /// Add new employee to database
        /// </summary>
        public async Task<Employee> AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            return employee;
        }

        /// <summary>
        /// Update existing employee
        /// </summary>
        public async Task<Employee> UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            return employee;
        }

        /// <summary>
        /// Soft delete employee (set IsActive = false)
        /// </summary>
        public async Task<bool> DeleteAsync(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);

            if (employee == null)
                return false;

            // Soft delete
            employee.IsActive = false;
            employee.ModifiedDate = DateTime.UtcNow;

            return true;
        }

        /// <summary>
        /// Check if email exists
        /// </summary>
        public async Task<bool> EmailExistsAsync(string email, int? excludeEmployeeId = null)
        {
            email = email.ToLower();

            if (excludeEmployeeId.HasValue)
            {
                // Exclude specific employee (for updates)
                return await _context.Employees
                    .AnyAsync(e => e.Email.ToLower() == email &&
                                  e.EmployeeId != excludeEmployeeId.Value &&
                                  e.IsActive);
            }

            // Check if any employee has this email
            return await _context.Employees
                .AnyAsync(e => e.Email.ToLower() == email && e.IsActive);
        }

        /// <summary>
        /// Save all changes to database
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
