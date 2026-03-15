using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int departmentId);
        Task<Department> AddAsync(Department department);
        Task<Department> UpdateAsync(Department department);
        Task<bool> DeleteAsync(int departmentId);
        Task<bool> DepartmentNameExistsAsync(string departmentName, int? excludeDepartmentId = null);
        Task<int> GetEmployeeCountAsync(int departmentId);
        Task<int> SaveChangesAsync();
    }
}
