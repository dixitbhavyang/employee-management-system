using EmployeeManagement.Application.DTOs.Common;
using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Application.DTOs.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Interfaces.Services
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDTO>> GetAllDepartmentsAsync();
        Task<DepartmentDTO> GetDepartmentByIdAsync(int departmentId);
        Task<DepartmentDTO> CreateDepartmentAsync(CreateDepartmentRequest request);
        Task<DepartmentDTO> UpdateDepartmentAsync(UpdateDepartmentRequest request);
        Task<bool> DeleteDepartmentAsync(int departmentId);
    }
}
