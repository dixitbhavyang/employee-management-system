using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.DTOs.Employee
{
    /// <summary>
    /// Request model for creating a new employee
    /// </summary>
    public class CreateEmployeeRequest
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public int DepartmentId { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public DateTime HireDate { get; set; } = DateTime.UtcNow;
    }
}
