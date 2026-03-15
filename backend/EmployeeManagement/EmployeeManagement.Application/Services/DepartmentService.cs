using EmployeeManagement.Application.DTOs.Common;
using EmployeeManagement.Application.DTOs.Department;
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
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<List<DepartmentDTO>> GetAllDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();

            return departments.Select(d => MapToDTO(d)).ToList();
        }

        public async Task<DepartmentDTO?> GetDepartmentByIdAsync(int departmentId)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);

            if (department == null)
                return null;

            return MapToDTO(department);
        }

        public async Task<DepartmentDTO> CreateDepartmentAsync(CreateDepartmentRequest request)
        {
            // Business rule: Check if department name already exists
            if (await _departmentRepository.DepartmentNameExistsAsync(request.DepartmentName))
            {
                throw new ValidationException($"Department '{request.DepartmentName}' already exists");
            }

            var department = new Department
            {
                DepartmentName = request.DepartmentName.Trim(),
                Location = request.Location?.Trim(),
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            var created = await _departmentRepository.AddAsync(department);
            await _departmentRepository.SaveChangesAsync();

            var result = await _departmentRepository.GetByIdAsync(created.DepartmentId);
            return MapToDTO(result!);
        }

        public async Task<DepartmentDTO> UpdateDepartmentAsync(UpdateDepartmentRequest request)
        {
            var department = await _departmentRepository.GetByIdAsync(request.DepartmentId);
            if (department == null)
            {
                throw new NotFoundException("Department", request.DepartmentId);
            }

            // Check if name is taken by another department
            if (await _departmentRepository.DepartmentNameExistsAsync(request.DepartmentName, request.DepartmentId))
            {
                throw new ValidationException($"Department '{request.DepartmentName}' already exists");
            }

            department.DepartmentName = request.DepartmentName.Trim();
            department.Location = request.Location?.Trim();
            department.IsActive = request.IsActive;
            department.ModifiedDate = DateTime.UtcNow;

            await _departmentRepository.UpdateAsync(department);
            await _departmentRepository.SaveChangesAsync();

            var result = await _departmentRepository.GetByIdAsync(department.DepartmentId);
            return MapToDTO(result!);
        }

        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
            {
                return false;
            }

            // Business rule: Check if department has employees
            var employeeCount = await _departmentRepository.GetEmployeeCountAsync(departmentId);
            if (employeeCount > 0)
            {
                throw new BusinessException($"Cannot delete department. It has {employeeCount} active employee(s).");
            }

            var deleted = await _departmentRepository.DeleteAsync(departmentId);
            if (deleted)
            {
                await _departmentRepository.SaveChangesAsync();
            }

            return deleted;
        }

        private DepartmentDTO MapToDTO(Department department)
        {
            return new DepartmentDTO
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                Location = department.Location,
                IsActive = department.IsActive,
                CreatedDate = department.CreatedDate,
                EmployeeCount = department.Employees?.Count(e => e.IsActive) ?? 0
            };
        }
    }
}
