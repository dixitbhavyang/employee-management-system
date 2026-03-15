using EmployeeManagement.Application.Interfaces.Repositories;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _context.Departments
                .Include(d => d.Employees.Where(e => e.IsActive))
                .Where(d => d.IsActive)
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int departmentId)
        {
            return await _context.Departments
                .Include(d => d.Employees.Where(e => e.IsActive))
                .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);
        }

        public async Task<Department> AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
            return department;
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
            return department;
        }

        public async Task<bool> DeleteAsync(int departmentId)
        {
            var department = await _context.Departments.FindAsync(departmentId);

            if (department == null)
                return false;

            // Soft delete
            department.IsActive = false;
            department.ModifiedDate = DateTime.UtcNow;

            return true;
        }

        public async Task<bool> DepartmentNameExistsAsync(string departmentName, int? excludeDepartmentId = null)
        {
            departmentName = departmentName.ToLower();

            if (excludeDepartmentId.HasValue)
            {
                return await _context.Departments
                    .AnyAsync(d => d.DepartmentName.ToLower() == departmentName &&
                                  d.DepartmentId != excludeDepartmentId.Value &&
                                  d.IsActive);
            }

            return await _context.Departments
                .AnyAsync(d => d.DepartmentName.ToLower() == departmentName && d.IsActive);
        }

        public async Task<int> GetEmployeeCountAsync(int departmentId)
        {
            return await _context.Employees
                .CountAsync(e => e.DepartmentId == departmentId && e.IsActive);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
